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
    AttackSkill,
    GetDamage,
    PickUp,
    Die,
    AfterDie,
}

public class PlayerController : MonoBehaviour
{
    // effect Test
    //public void SetSkill0()
    //{
    //    m_animator.SetTrigger("Skill_0");
    //}
    //public void SetSkill1()
    //{
    //    m_animator.SetTrigger("Skill_1");
    //}
    //public void SetSkill2()
    //{
    //    m_animator.SetTrigger("Skill_2");
    //}

    PlayerState m_currState = PlayerState.Idle;
    public Animator m_animator;

    public Transform m_targetPos;
    private float m_attackRange = 2.5f;
    public float fSpeed = 5f;

    private float m_prevTime = 0;
    public float m_attackInterval;
    private Vector3 m_point = Vector3.zero;
    private Vector3 vEnd;

    public bool isChanging = false;
    public bool ActiveMarker = false;
    public bool isSkillState = false;
    public bool OnShakeCamera { get; set; }

    private NPC talkingNpc;
    private float talkDistance = 3.0f;

    public bool usingPortal = false;

    private PickUpItem playerPickUpItem;

    public void Init()
    {
        m_animator = GetComponent<Animator>();
        ChangeState(PlayerState.Idle);

        // 시작했을때 캐릭터의 정면을 바라보도록 설정함
        m_point = transform.position + transform.forward;
        vEnd = transform.position;
    }

    public void ChangeState(PlayerState state)
    {
        if (m_currState == state && m_currState != PlayerState.BaseAttack)
            return;

        if (m_currState == PlayerState.Move)
            SoundManager.Instance.StopAudioSource(SOUND.Step);

        m_currState = state;
        switch (state)
        {
            case PlayerState.Idle:
                m_animator.SetBool("Run", false);
                break;
            case PlayerState.Move:
                m_animator.SetBool("Run", true);
                SoundManager.Instance.Play("Step/Running-4",SOUND.Step);
                break;
            case PlayerState.BaseAttack:
                {
                    m_animator.SetBool("Run", false);
                    Vector3 dir = m_targetPos.position - transform.position;
                    dir.y = 0;
                    transform.rotation = Quaternion.LookRotation(dir);
                    m_prevTime = Time.time;
                    m_animator.SetTrigger("Attack");
                }
                break;
            case PlayerState.AttackSkill:
                m_animator.SetBool("Run", false);
                CheckSkill();
                break;
            case PlayerState.GetDamage:
                m_animator.SetBool("Run", false);
                m_animator.SetTrigger("Damaged");
                break;
            case PlayerState.PickUp:
                m_animator.SetBool("Run", false);
                m_animator.SetTrigger("PickUp");
                break;
            case PlayerState.Die:
                m_animator.SetBool("Run", false);
                m_animator.SetTrigger("Dead");
                break;
            case PlayerState.AfterDie:
                m_animator.SetBool("Run", false);
                break;
        }
    }

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
                m_animator.SetTrigger("Skill_0");
                break;
            case 1:
                m_animator.SetTrigger("Skill_1");
                break;
            case 2:
                m_animator.SetTrigger("Skill_2");
                break;
        }
    }
    
    void AttackSkill()
    {
        if (!isSkillState)
        {
            isSkillState = false;
            ChangeState(PlayerState.Idle);
            return;
        }

        AnimatorStateInfo stateinfo = m_animator.GetCurrentAnimatorStateInfo(0);
        // 단순히 테스트 스킬이 2개 연속이기때문에 1.9 초를 기준으로 삼았기때문에 후에 추가수정해야함
        //if (!stateinfo.IsName("Idle") && stateinfo.normalizedTime >= 1.9)
        if (!stateinfo.IsName("Idle") && stateinfo.normalizedTime >= 1)
        {
            ChangeState(PlayerState.Idle);
            // 이부분을 활성화 시키면 effect 첫번째 순서에서 실행이 안됨
            //SkillManager.Instance.currSelectedSkill = null;
            isSkillState = false;
        }
        // 스킬의 콜라이더 체크
    }

    // 타겟까지 이동
    public void SetTargetSkill(Vector3 skillTargetPos)
    {
        // 타겟 지정
        // m_targetPos == null 오류
        //m_targetPos.position = skillTargetPos;

        // 방향
        Vector3 dir = m_targetPos.transform.position - transform.position;
        // 거리
        float distance = dir.magnitude;
        dir.Normalize();

        Rotate(m_targetPos.transform.position);
        vEnd = transform.position + dir * (distance - m_attackRange);

        ChangeState(PlayerState.Move);
    }

    public void SetAreaSkill(Vector3 mousePoint)
    {
        Rotate(mousePoint);
        SkillManager.skillColliderManager.end = mousePoint;

        ChangeState(PlayerState.AttackSkill);
    }

    public void Rotate(Vector3 targetPos)
    {
        Vector3 _target = targetPos - transform.position;
        _target.y = 0;
        transform.rotation = Quaternion.LookRotation(_target);
    }

    private void Move()
    {
        if (m_targetPos == null)
            transform.position = Vector3.MoveTowards(transform.position, vEnd, Time.deltaTime * fSpeed);
        else
            transform.position = Vector3.MoveTowards(transform.position, m_targetPos.transform.position, Time.deltaTime * fSpeed);
    }

    private void Idle()
    {
        Player.Instance.rigid.velocity = Vector3.zero;
    }

    private void MoveState()
    {
        Move();

        if (m_targetPos != null)
        {
            // 타겟의 태그값이 몬스터일 경우 처리합니다.
            if (m_targetPos.tag.Equals("Monster"))
            {
                // 기본공격
                if(!isSkillState)
                {
                    if (Vector3.Distance(transform.position, m_targetPos.transform.position) <= m_attackRange)
                    {
                        ChangeState(PlayerState.BaseAttack);
                    }
                }
                // 스킬공격
                else if(isSkillState)
                {
                    Skill currentSkill = SkillManager.Instance.currSelectedSkill;
                    if(Vector3.Distance(transform.position, m_targetPos.transform.position) <= currentSkill.skillRange)
                    {
                        ChangeState(PlayerState.AttackSkill);
                    }
                }
            }
            // 타겟의 태그값이 아이템이라면 거리에 따라 획득할 수 있도록 합니다.
            else if (m_targetPos.tag.Equals("Item"))
            {
                if (Vector3.Distance(transform.position, m_targetPos.transform.position) <= 0.5f)
                {
                    PickUpItem pickup = m_targetPos.GetComponent<PickUpItem>();
                    playerPickUpItem = pickup;

                    ChangeState(PlayerState.PickUp);
                }
            }
            else if(m_targetPos.gameObject.tag.Equals("NPC"))
            {
                if (Vector3.Distance(transform.position, m_targetPos.transform.position) <= talkDistance)
                {
                    talkingNpc = m_targetPos.gameObject.GetComponent<NPC>();

                    if(Player.Instance.TalkNpcEvent!= null)
                        Player.Instance.TalkNpcEvent(talkingNpc);

                    Player.Instance.TalkNpc(talkingNpc.npcID, talkingNpc);
                    ChangeState(PlayerState.Idle);
                }
            }
        }
        // terrain 클릭시 움직임
        else if(m_targetPos== null || vEnd != null)
        {
            if (Vector3.Distance(transform.position, vEnd) <= 0.5f)
            {
                ChangeState(PlayerState.Idle);
            }
        }
    }

    // 타겟과 플레이어 거리측정, 공격 속도 조절
    private void AttackState()
    {
        if ((Time.time - m_prevTime) >= m_attackInterval)
        {
            if (m_targetPos == null)
            {
                ChangeState(PlayerState.Idle);
                return;
            }

            if (Vector3.Distance(transform.position, vEnd) <= 3)
            {
                //m_prevTime = Time.time;
                ChangeState(PlayerState.BaseAttack);
            }
            else
            {
                AnimatorStateInfo info = m_animator.GetCurrentAnimatorStateInfo(0);
                // 현재 애니메이션이 변경중인 상태가 아니고, 플레이 되고 있는 애니메이션이 Attack이 아닐경우
                if (!m_animator.IsInTransition(0) && !info.IsTag("Attack"))
                {
                    ChangeState(PlayerState.Move);
                }
            }
        }
    }

    private void PickUp()
    {
        AnimatorStateInfo info = m_animator.GetCurrentAnimatorStateInfo(0);

        // 현재 애니메이션이 변경중인 상태가 아니고, 플레이 되고 있는 애니메이션이 PickUp 이 아닐경우
        if (!m_animator.IsInTransition(0) && info.IsName("PickUp") && info.normalizedTime > 0.9)
            {
                playerPickUpItem.PickUp();
                ChangeState(PlayerState.Idle);
            }
    }


    public void GetDamage()
    {
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        // 04.23 애니메이션이 80% 이상 실행되었을 때 대기 애니메이션으로 바꿀 수 있도록 추가
        if (stateInfo.normalizedTime >= 0.8f || !Player.Instance.data.isDead)
        {
            if (Player.Instance.data.curHp <= 0)
            {
                ChangeState(PlayerState.Die);
            }
            else
                ChangeState(PlayerState.Idle);
        }
    }

    void PlayerDie()
    {
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("GetHit"))
            return;
        if(stateInfo.IsName("Die") && stateInfo.normalizedTime >= 0.9f)
        {
            if (Player.Instance.data.isDead == false && Player.Instance.data.curHp <= 0)
            {
                Player.Instance.data.curHp = 0;
                Player.Instance.data.isDead = true;
                m_targetPos = null;
                
                ChangeState(PlayerState.AfterDie);
            }
        }
    }

    public void ReLive()
    {
        gameObject.SetActive(true);
        Player.Instance.data.curHp = 1000;
        Player.Instance.data.isDead = false;
        ChangeState(PlayerState.Idle);
    }

    public void AfterDie()
    {
        //몬스터 일정시간 반투명코드 추가예정
        gameObject.SetActive(false);
    }

    void GameInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Monster"))
                {
                    Debug.Log("몬스터입니다");
                    // 타겟 지정
                    m_targetPos = GameObject.Find(hitInfo.collider.gameObject.name).transform;
                    // 방향
                    Vector3 dir = m_targetPos.transform.position - transform.position;
                    // 거리
                    float distance = dir.magnitude;
                    dir.Normalize();

                    Rotate(m_targetPos.transform.position);
                    vEnd = transform.position + dir * (distance - m_attackRange);

                    ChangeState(PlayerState.Move);
                }
                else if (hitInfo.collider.CompareTag("Terrain") || hitInfo.collider.CompareTag("Portal"))
                {
                    Debug.Log("바닥입니다");
                    // 공격중일때 이동,회전 불가능
                    AnimatorStateInfo info = m_animator.GetCurrentAnimatorStateInfo(0);

                    if (info.IsName("BaseAttack") || m_animator.IsInTransition(0))
                        return;

                    m_prevTime = 0;
                    //m_currState = PlayerState.None;

                    m_point = hitInfo.point;
                    vEnd = hitInfo.point;
                    m_targetPos = null;
                    Rotate(m_point);

                    ChangeState(PlayerState.Move);
                }
                else if (hitInfo.collider.GetComponent<PickUpItem>() != null)
                {
                    Debug.Log("아이템입니다");
                    // 누른 객체가 아이템이라면
                    // 아이템을 타겟으로 하고 
                    // 아이템위치에 도착하면 줍는 애니메이션 실행.
                    // PickUpItem에서 PickUp함수 실행

                    m_targetPos = hitInfo.collider.gameObject.transform;
                    Vector3 dir = m_targetPos.transform.position - transform.position;
                    float distance = dir.magnitude;
                    dir.Normalize();
                    Rotate(hitInfo.point);
                    vEnd = transform.position + dir * distance;

                    ChangeState(PlayerState.Move);
                }
                else if (hitInfo.collider.GetComponent<NPC>() != null)
                {
                    Debug.Log("NPC입니다");
                    // 타겟 지정
                    m_targetPos = hitInfo.collider.gameObject.transform;
                    // 방향
                    Vector3 dir = m_targetPos.transform.position - transform.position;
                    // 거리
                    float distance = dir.magnitude;
                    dir.Normalize();

                    Rotate(m_targetPos.transform.position);
                    vEnd = transform.position + dir * (distance - talkDistance);

                    ChangeState(PlayerState.Move);
                }
            }
        }
    }

    void Update()
    {
        if (OnShakeCamera)
            return;

        // 플레이어의 대화상태 체크
        if (Player.Instance.isTalking)
            TalkState();

        // 무기변경상태 체크
        if (isChanging)
            ChangeAnim();

        if (CheckUI() == false && ActiveMarker == false && isSkillState == false)
            GameInput();

       switch(m_currState)
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
            case PlayerState.AttackSkill:
                AttackSkill();
                break;
            case PlayerState.GetDamage:
                GetDamage();
                break;
            case PlayerState.PickUp:
                PickUp();
                break;
            case PlayerState.Die:
                PlayerDie();
                break;
            case PlayerState.AfterDie:
                AfterDie();
                break;
        }
    }

    public bool CheckUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void ChangeAnim()
    {
        AnimatorStateInfo currAnimInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if (currAnimInfo.normalizedTime >= 0.9)
        {
            m_animator.runtimeAnimatorController = WeaponManager.Instance.currWeaponAnim.runtimeAnimatorController;
            isChanging = false;
        }
    }

    private void TalkState()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Player.Instance.TalkNpc(talkingNpc.npcID, talkingNpc);
        }
    }

    // 맵이동
    private void OnTriggerEnter(Collider other)
    {
        if (usingPortal)
            return;
        // null Exception
        if (other.gameObject.CompareTag("Portal"))
        {
            usingPortal = true;
            Portal portal = other.gameObject.GetComponent<Portal>();
            PortalManager.Instance.GetPortalInfo(portal);

            SceneMng.Instance.ChangeScene(portal.connectScene, true);
        }
    }
}

//// Attack 상태가되면,
//// 1. attack 시간측정해서
//// 2. 비율/시간 으로 공격타이밍에 기능실행
//public void SetAttack()
//{
//    if (m_targetPos != null)
//    {
//        Monster target = m_targetPos.GetComponent<Monster>();

//        if (target == null)
//            return;

//        target.baseController.SetTarget(gameObject.transform);
//        target.SetDamage();
//        target.baseController.ChangeState(MonsterState.GetDamage);
//        Debug.Log(target.name + "'s HP : " + target.data.curHp);
//    }
//}

//void AttackSkill0()
//{
//    AnimatorStateInfo stateinfo = m_animator.GetCurrentAnimatorStateInfo(0);
//    if (stateinfo.normalizedTime >= 0.9)
//        ChangeState(PlayerState.Idle);
//}

//void AttackSkill1()
//{
//    AnimatorStateInfo stateinfo = m_animator.GetCurrentAnimatorStateInfo(0);
//    if (stateinfo.normalizedTime >= 0.9)
//        ChangeState(PlayerState.Idle);
//}

//void AttackSkill2()
//{
//    AnimatorStateInfo stateinfo = m_animator.GetCurrentAnimatorStateInfo(0);
//    if (stateinfo.normalizedTime >= 0.9)
//        ChangeState(PlayerState.Idle);
//}




// 사용안되는 부분
//public void SetTarget(Transform target)
//{
//    m_target = target;
//    ChangeState(PlayerState.Move);
//}

// MoveState의 Monster 발견 조건문에 포함된 부분
//void TraceTheEnemy()
//{
//    //m_prevTime = Time.time;
//    float distance = Vector3.Distance(transform.position, m_target.transform.position);
//    transform.position = Vector3.MoveTowards(transform.position, m_target.position, Time.deltaTime * fSpeed);

//    if (distance <= m_attackRange)
//    {
//        ChangeState(PlayerState.Attack);
//    }
//}


// void Attack()
// {
//    m_elapsedTime += Time.deltaTime;

//    float distance = Vector3.Distance(transform.position, m_target.position);
//    if (distance <= m_attackRange)
//    {
//        if (m_attackTime <= m_elapsedTime)
//        {
//            m_elapsedTime = 0;
//            m_animator.SetTrigger("Attack");
//        }
//    }
//    else
//    {
//        AnimatorStateInfo info = m_animator.GetCurrentAnimatorStateInfo(0);
//        // 현재 애니메이션이 변경중인 상태가 아니고, 플레이 되고 있는 애니메이션이 Attack이 아닐때
//        if (!m_animator.IsInTransition(0) && !info.IsTag("Attack"))
//        {
//           ChangeState(PlayerState.TraceTheEnemy);

//            if (m_target == null)
//            {
//                ChangeState(PlayerState.Idle);
//            }
//        }
//    }
//}