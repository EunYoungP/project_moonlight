using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum PlayerState
{
    Idle,
    Move,
    BaseAttack,
    SkillAttack,
    GetDamage,
    PickUp,
    ChangeWeapon,
    Talk,
    Die,
    AfterDie,
}

// input 입력 분리
public class PlayerController : MonoBehaviour
{
    private PlayerState currState = PlayerState.Idle;
    public Animator animator { get; set; }

    public Transform targetPos { get; set; }
    public float attackRange {get{ return 2.5f;}}
    private float speed = 10f;

    private float prevTime = 0;
    public float attackInterval;
    private Vector3 destination;

    public bool isWaitActSkill { get; set; }
    public bool isSkillState { get; set; }
    public bool isShakingCamera { get; set; }
    public bool isUsingPortal { get; set; }

    private NPC talkingNpc;
    private float talkDistance = 3.0f;

    private PickUpItem playerPickUpItem;
    private PlayerInput playerInput;

    public void Init()
    {
        animator = GetComponent<Animator>();
        ChangeState(PlayerState.Idle);

        destination = transform.position;
        playerInput = gameObject.GetComponent<PlayerInput>();
    }

    public void GetTargetPos(Vector3 destination, Transform targetPos = null)
    {
        this.destination = destination;
        this.targetPos = targetPos;
    }

    #region FSM
    #region 상태 전이
    public void ChangeState(PlayerState state)
    {
        if (currState == state && currState != PlayerState.BaseAttack)
            return;

        if (currState == PlayerState.Move)
            SoundManager.Instance.StopAudioSource(SOUND.Step);

        currState = state;
        switch (state)
        {
            case PlayerState.Idle:
                animator.SetBool("Run", false);
                break;
            case PlayerState.Move:
                animator.SetBool("Run", true);
                SoundManager.Instance.Play("Step/Running-4", SOUND.Step);
                break;
            case PlayerState.BaseAttack:
                animator.SetBool("Run", false);
                Vector3 dir = targetPos.position - transform.position;
                dir.y = 0;
                transform.rotation = Quaternion.LookRotation(dir);
                prevTime = Time.time;
                animator.SetTrigger("Attack");
                break;
            case PlayerState.SkillAttack:
                animator.SetBool("Run", false);
                CheckSkill();
                break;
            case PlayerState.GetDamage:
                animator.SetBool("Run", false);
                animator.SetTrigger("Damaged");
                break;
            case PlayerState.PickUp:
                animator.SetBool("Run", false);
                animator.SetTrigger("PickUp");
                break;
            case PlayerState.ChangeWeapon:
                break;
            case PlayerState.Talk:
                animator.SetBool("Run", false);
                break;
            case PlayerState.Die:
                animator.SetBool("Run", false);
                animator.SetTrigger("Dead");
                break;
            case PlayerState.AfterDie:
                animator.SetBool("Run", false);
                break;
        }
    }
    #endregion

    private void Update()
    {
        if (!IsExceptionInput())
           playerInput.GameInput();

        ActionState();
    }

    private bool IsExceptionInput()
    {
        bool uiInput = EventSystem.current.IsPointerOverGameObject();
        bool loadingState = SceneMng.Instance.LoadingState();
        if (uiInput
            || isWaitActSkill
            || isSkillState
            || isShakingCamera
            || loadingState)
        {
            return true;
        }
        return false;
    }

    #region 상태
    private void ActionState()
    {
        switch (currState)
        {
            case PlayerState.Idle:
                Idle();
                break;
            case PlayerState.Move:
                MoveState();
                break;
            case PlayerState.BaseAttack:
                AttackState();
                break;
            case PlayerState.SkillAttack:
                AttackSkill();
                break;
            case PlayerState.GetDamage:
                GetDamage();
                break;
            case PlayerState.PickUp:
                PickUp();
                break;
            case PlayerState.ChangeWeapon:
                ChangeWeapon();
                break;
            case PlayerState.Talk:
                TalkState();
                break;
            case PlayerState.Die:
                PlayerDie();
                break;
            case PlayerState.AfterDie:
                AfterDie();
                break;
        }
    }
    #endregion
    #endregion

    #region 스킬
    private void CheckSkill()
    {
        Skill selectedSkill = SkillManager.Instance.currSelectedSkill;

        if (selectedSkill.UseWeaponState != WeaponManager.Instance.currWeaponType)
        {
            ChangeState(PlayerState.Idle);
            Debug.Log("스킬에 맞지 않는 무기를 장착중입니다.");
            return;
        }

        switch (selectedSkill.key)
        {
            case 0:
                animator.SetTrigger("Skill_0");
                break;
            case 1:
                animator.SetTrigger("Skill_1");
                break;
            case 2:
                animator.SetTrigger("Skill_2");
                break;
        }
    }

    private void AttackSkill()
    {
        if (!isSkillState)
        {
            ChangeState(PlayerState.Idle);
            return;
        }

        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!stateinfo.IsName("Idle") && stateinfo.normalizedTime >= 1)
        {
            ChangeState(PlayerState.Idle);
            isSkillState = false;
        }
    }

    // 타겟까지 이동
    // skillTargetPos 안쓰임
    public void SetTargetSkill(Vector3 skillTargetPos)
    {
        Vector3 dir = targetPos.transform.position - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();

        Rotate(targetPos.transform.position);
        destination = transform.position + dir * (distance - attackRange);

        ChangeState(PlayerState.Move);
    }

    public void SetAreaSkill(Vector3 mousePoint)
    {
        Rotate(mousePoint);
        SkillManager.skillColliderManager.end = mousePoint;

        ChangeState(PlayerState.SkillAttack);
    }
    #endregion

    #region 이동

    public void Rotate(Vector3 targetPos)
    {
        Vector3 targetDir = targetPos - transform.position;
        targetDir.y = 0;
        transform.rotation = Quaternion.LookRotation(targetDir);
    }

    private void Move()
    {
        if (targetPos == null)
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
        else
            transform.position = Vector3.MoveTowards(transform.position, targetPos.transform.position, Time.deltaTime * speed);
    }
    #endregion

    private void Idle()
    {
        Player.Instance.rigid.velocity = Vector3.zero;
    }

    private void MoveState()
    {
        Move();

        if (targetPos != null)
        {
            // 타겟의 태그값이 몬스터일 경우 처리합니다.
            if (targetPos.tag.Equals("Monster"))
            {
                // 기본공격
                if(!isSkillState)
                {
                    if (Vector3.Distance(transform.position, targetPos.transform.position) <= attackRange)
                    {
                        ChangeState(PlayerState.BaseAttack);
                    }
                }
                // 스킬공격
                else if(isSkillState)
                {
                    Skill currentSkill = SkillManager.Instance.currSelectedSkill;
                    if(Vector3.Distance(transform.position, targetPos.transform.position) <= currentSkill.skillRange)
                    {
                        ChangeState(PlayerState.SkillAttack);
                    }
                }
            }
            // 타겟의 태그값이 아이템이라면 거리에 따라 획득할 수 있도록 합니다.
            else if (targetPos.tag.Equals("Item"))
            {
                if (Vector3.Distance(transform.position, targetPos.transform.position) <= 0.5f)
                {
                    PickUpItem pickup = targetPos.GetComponent<PickUpItem>();
                    playerPickUpItem = pickup;

                    ChangeState(PlayerState.PickUp);
                }
            }
            // 타겟의 태그값이 엔피씨라면 거리에따라 대화를 시작합니다.
            //else if(targetPos.gameObject.tag.Equals("NPC"))
            //{
            //    if (Vector3.Distance(transform.position, targetPos.transform.position) <= talkDistance)
            //    {
            //        talkingNpc = targetPos.gameObject.GetComponent<NPC>();

            //        if(Player.Instance.TalkNpcEvent!= null)
            //            Player.Instance.TalkNpcEvent(talkingNpc);

            //        Player.Instance.TalkNpc(talkingNpc.npcID, talkingNpc);
            //        //ChangeState(PlayerState.Idle);
            //    }
            //}
            else if (targetPos.gameObject.tag.Equals("NPC"))
            {
                if (Vector3.Distance(transform.position, targetPos.transform.position) <= talkDistance)
                {
                    talkingNpc = targetPos.gameObject.GetComponent<NPC>();

                    if (Player.Instance.TalkNpcEvent != null)
                        Player.Instance.TalkNpcEvent(talkingNpc);

                    ChangeState(PlayerState.Talk);

                    //ChangeState(PlayerState.Idle);
                }
            }
        }
        // 목적지로 이동을 완료하면 대기상태로 전환합니다.
        else if(targetPos== null || destination != null)
        {
            if (Vector3.Distance(transform.position, destination) <= 0.5f)
                ChangeState(PlayerState.Idle);
        }
        else
        {
            Debug.Log("타겟과 목적지가 존재하지 않습니다.");
        }
    }

    // 타겟과 플레이어 거리측정, 공격 속도 조절
    private void AttackState()
    {
        // 공격 실행 대기시간이 끝난상태
        if ((Time.time - prevTime) >= attackInterval)
        {
            if (targetPos == null)
            {
                ChangeState(PlayerState.Idle);
                return;
            }

            // 타겟이 공격범위에 들어왔을 경우
            if (Vector3.Distance(transform.position, destination) <= 3)
            {
                ChangeState(PlayerState.BaseAttack);
            }
            // 타겟이 공격범위 바깥에 있을경우
            else
            {
                AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

                // 타겟위치로 이동중
                if (!animator.IsInTransition(0) && !info.IsTag("Attack"))
                {
                    ChangeState(PlayerState.Move);
                }
            }
        }
    }

    private void PickUp()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 픽업 애니메이션이 끝나는 시점
        if (!animator.IsInTransition(0) && stateInfo.IsName("PickUp") && stateInfo.normalizedTime >= 0.7)
            {
                playerPickUpItem.PickUp();
                ChangeState(PlayerState.Idle);
            }
        else
        {
            Debug.Log("픽업 애니메이션이 끝나는 시점이 아닙니다.");
        }
    }

    private void ChangeWeapon()
    {
        AnimatorStateInfo currAnimInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (currAnimInfo.normalizedTime >= 0.9)
        {
            animator.runtimeAnimatorController = WeaponManager.Instance.currWeaponAnim.runtimeAnimatorController;
            ChangeState(PlayerState.Idle);
        }
    }

    private void TalkState()
    {
        if(!Player.Instance.isTalking)
        {
            Player.Instance.TalkNpc(talkingNpc.npcID, talkingNpc);
            //ChangeState(PlayerState.Idle);
        }
        else
        {
            // 대화중이라면 마우스입력이 있을때 다음 대화창 생성
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    Player.Instance.TalkNpc(talkingNpc.npcID, talkingNpc);
            }
        }
    }

    public void GetDamage()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // 애니메이션이 80% 이상 실행되었을 때 대기 애니메이션으로 변경
        if (stateInfo.normalizedTime >= 0.8f || !Player.Instance.data.isDead)
        {
            if (Player.Instance.data.curHp <= 0)
            {
                ChangeState(PlayerState.Die);
            }
            else
            {
                ChangeState(PlayerState.Idle);
            }
        }
    }

    private void PlayerDie()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("GetHit"))
            return;

        // die 애니메이션이 끝나는 시점에 플레이어 데이터 세팅
        if(stateInfo.IsName("Die") && stateInfo.normalizedTime >= 0.9f)
        {
            if (Player.Instance.data.isDead == false && Player.Instance.data.curHp <= 0)
            {
                Player.Instance.data.curHp = 0;
                Player.Instance.data.isDead = true;
                targetPos = null;
                
                ChangeState(PlayerState.AfterDie);
            }
        }
    }

    public void ReLive()
    {
        gameObject.SetActive(true);
        Player.Instance.data.curHp = Player.Instance.data.maxHp;
        Player.Instance.data.isDead = false;
        ChangeState(PlayerState.Idle);
    }

    public void AfterDie()
    {
        // 몬스터 일정시간 반투명코드 추가 예정

        gameObject.SetActive(false);
    }

    // 맵 이동
    private void OnTriggerEnter(Collider other)
    {
        if (isUsingPortal)
            return;
        // null Exception
        if (other.gameObject.CompareTag("Portal"))
        {
            isUsingPortal = true;
            Portal portal = other.gameObject.GetComponent<Portal>();
            PortalManager.Instance.GetPortalInfo(portal);

            SceneMng.Instance.ChangeScene(portal.connectScene, true);
        }
    }
}