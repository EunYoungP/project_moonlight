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

public class PlayerController : MonoBehaviour
{
    private PlayerState currState = PlayerState.Idle;
    public Animator animator { get; set; }

    public Transform targetPos { get; set; }
    public float attackRange {get{ return 1.5f;}}
    private float speed = 6.5f;

    private float prevTime = 0;
    private float attackInterval = 2f;
    private Vector3 destination;

    public bool isWaitActSkill { get; set; }
    public bool isSkillState { get; set; }
    public bool isShakingCamera { get; set; }
    public bool isUsingPortal { get; set; }

    private NPC talkingNpc;
    private float talkDistance = 2.5f;

    private PickUpItem playerPickUpItem;
    private PlayerInput playerInput;

    // Edit MouseInputID
    public int pointerID { get; set; }

    public void Init()
    {
#if UNITY_EDITOR
        pointerID = -1;
#elif UNITY_ANDROID
        pointerID = 0;
#endif

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

    private void Update()
    {
        if (!IsExceptionInput())
            playerInput.GameInput();

        ActionState();
    }

    private bool IsExceptionInput()
    {
        bool uiInput = EventSystem.current.IsPointerOverGameObject(pointerID);
        bool loadingState = SceneMng.Instance.LoadingState();
        if (uiInput == true
            || isWaitActSkill == true
            || isSkillState == true
            || isShakingCamera == true
            || loadingState == true)
        {
            return true;
        }
        return false;
    }

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
                SoundManager.Instance.Play("Step/Running-4", SOUND.Step, 1.5f);
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

#region 상태
    private void ActionState()
    {
        switch (currState)
        {
            case PlayerState.Idle:
                StateIdle();
                break;
            case PlayerState.Move:
                StateMove();
                break;
            case PlayerState.BaseAttack:
                StateBaseAttack();
                break;
            case PlayerState.SkillAttack:
                StateSkillAttack();
                break;
            case PlayerState.GetDamage:
                StateGetDamage();
                break;
            case PlayerState.PickUp:
                StatePickUp();
                break;
            case PlayerState.ChangeWeapon:
                StateChangeWeapon();
                break;
            case PlayerState.Talk:
                StateTalk();
                break;
            case PlayerState.Die:
                StateDie();
                break;
            case PlayerState.AfterDie:
                StateAfterDie();
                break;
        }
    }
#endregion

#endregion

#region 스킬
    private void CheckSkill()
    {
        Skill selectedSkill = SkillManager.Instance.currSelectedSkill;

        // 스킬타입 무기타입 비교
        if (selectedSkill.UseWeaponState != WeaponManager.Instance.currWeaponType)
        {
            ChangeState(PlayerState.Idle);
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

    private void StateSkillAttack()
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

    private void StateIdle()
    {
        Player.Instance.rigid.velocity = Vector3.zero;
    }

    private void StateMove()
    {
        Move();

        if (targetPos != null)
        {
            // 타겟의 태그값이 몬스터일 경우 처리합니다.
            if (targetPos.CompareTag("Monster"))
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
            else if (targetPos.CompareTag("Item"))
            {
                if (Vector3.Distance(transform.position, targetPos.transform.position) <= 0.5f)
                {
                    PickUpItem pickup = targetPos.GetComponent<PickUpItem>();
                    playerPickUpItem = pickup;

                    ChangeState(PlayerState.PickUp);
                }
            }
            else if (targetPos.gameObject.CompareTag("NPC"))
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
    private void StateBaseAttack()
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

    private void StatePickUp()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 픽업 애니메이션이 끝나는 시점
        if (!animator.IsInTransition(0) && stateInfo.IsName("PickUp") && stateInfo.normalizedTime >= 0.7)
            {
                playerPickUpItem.PickUp();
                ChangeState(PlayerState.Idle);
            }
    }

    private void StateChangeWeapon()
    {
        AnimatorStateInfo currAnimInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (currAnimInfo.normalizedTime >= 0.9)
        {
            animator.runtimeAnimatorController = WeaponManager.Instance.currWeaponAnim.runtimeAnimatorController;
            ChangeState(PlayerState.Idle);
        }
    }

    private void StateTalk()
    {
        if(!Player.Instance.isTalking)
        {
            Player.Instance.TalkNpc(talkingNpc.npcID, talkingNpc);
        }
        else
        {
            // 대화중이라면 마우스입력이 있을때 다음 대화창 생성
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject(pointerID))
                    Player.Instance.TalkNpc(talkingNpc.npcID, talkingNpc);
            }
        }
    }

    public void StateGetDamage()
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

    private void StateDie()
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

    public void StateReLive()
    {
        gameObject.SetActive(true);
        Player.Instance.data.curHp = Player.Instance.data.maxHp;
        Player.Instance.data.isDead = false;
        ChangeState(PlayerState.Idle);
    }

    public void StateAfterDie()
    {
        // 몬스터 일정시간 반투명코드 추가 예정

        gameObject.SetActive(false);
    }

    // 맵 이동
    private void OnTriggerEnter(Collider other)
    {
        if (isUsingPortal)
            return;

        if (other.gameObject.CompareTag("Portal"))
        {
            isUsingPortal = true;
            Portal portal = other.gameObject.GetComponent<Portal>();
            PortalManager.Instance.GetPortalInfo(portal);

            SceneMng.Instance.ChangeScene(portal.connectScene, true);
        }
    }
}