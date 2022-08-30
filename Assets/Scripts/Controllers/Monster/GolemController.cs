using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MonsterState
{
    None,
    Attack,
    Idle,
    Patrol,
    TraceTheEnemy,
    GetDamage,
    Die,
}

public class GolemController : MonoBehaviour, BaseController
{
    Monster monster;

    MonsterState m_currState = MonsterState.None;
    private Animator m_animator;
    private float m_elapsedTime;
    private float m_attackTime = 2f;
    private List<Vector3> m_patrolList = new List<Vector3>();
    private int m_patrolIndex = 0;

    private Transform m_target;
    private float m_findRange = 10f;
    private float m_attackRange = 2.5f;

    private float m_idlePrevTime;
    private float m_idleInterval = 2f;

    void Start()
    {
        monster = gameObject.GetComponent<Monster>();
        m_animator = GetComponent<Animator>();
        Transform t = transform.Find("Patrol");
        if (t != null)
        {
            for (int i = 0; i < t.childCount; ++i)
            {
                m_patrolList.Add(t.GetChild(i).position);
            }
        }
        ChangeState(MonsterState.Patrol);
    }

    public void ChangeState(MonsterState state)
    {
        if (m_currState == state )
            return;

        m_currState = state;
        switch (state)
        {
            case MonsterState.Idle:
                m_animator.SetBool("Walk", false);
                m_animator.SetBool("Run", false);
                m_idlePrevTime = Time.time;
                break;
            case MonsterState.Patrol:
                m_animator.SetBool("Walk", true);
                m_animator.SetBool("Run", false);
                break;
            case MonsterState.TraceTheEnemy:
                m_animator.SetBool("Walk", false);
                m_animator.SetBool("Run", true);
                break;
            case MonsterState.Attack:
                {
                    m_animator.SetBool("Walk", false);
                    m_animator.SetBool("Run", false);
                    Vector3 dir = m_target.position - transform.position;
                    dir.y = 0;
                    transform.rotation = Quaternion.LookRotation(dir);
                    m_animator.SetTrigger("Attack");
                }
                break;
            case MonsterState.GetDamage:
                m_animator.SetBool("Walk", false);
                m_animator.SetBool("Run", false);
                m_animator.SetTrigger("Damage");
                break;
            case MonsterState.Die:
                m_animator.SetBool("Walk", false);
                m_animator.SetBool("Run", false);
                m_animator.SetTrigger("Die");
                break;
        }
    }

    public void SetTarget(Transform target)
    {
        m_target = target;

        if(m_target != null)
            ChangeState(MonsterState.TraceTheEnemy);
    }

    public Transform GetTarget()
    {
        return m_target;
    }

    void Idle()
    {
        // 시간간격동안 idle 상태 유지
        if((Time.time - m_idlePrevTime) >= m_idleInterval)
        {
            ChangeState(MonsterState.Patrol);
        }
    }

    void Patrol()
    {
        transform.position =  Vector3.MoveTowards(transform.position, m_patrolList[m_patrolIndex], 0.05f);

        Vector3 dir =  m_patrolList[m_patrolIndex] - transform.position;

        // 높낮이 있을 경우를 대비
        dir.y = 0;  
        transform.rotation = Quaternion.LookRotation(dir);

        float distance = Vector3.Distance(transform.position, m_patrolList[m_patrolIndex]);
        if (distance <= 0.5f )
        {
            ++m_patrolIndex;
            if (m_patrolIndex >= m_patrolList.Count)
                m_patrolIndex = 0;
        }
    }

    void TraceTheEnemy()
    {
        if (m_target == null)
            return;

        Vector3 _target = m_target.transform.position - transform.position;
        _target.y = 0;
        transform.rotation = Quaternion.LookRotation(_target);

        float distance = Vector3.Distance(transform.position, m_target.transform.position);

        if(distance >= m_findRange)
        {
            ChangeState(MonsterState.Idle);
        }
        else if(distance <= m_attackRange)
        {
            ChangeState(MonsterState.Attack);
        }

        transform.position = Vector3.MoveTowards(transform.position, m_target.position, 0.1f);
    }

    void Attack()
    {
        m_elapsedTime += Time.deltaTime;

        if (m_target == null)
        {
            AnimatorStateInfo stateinfo = m_animator.GetCurrentAnimatorStateInfo(0);
            if(stateinfo.normalizedTime >= 0.8)
                ChangeState(MonsterState.Idle);
            return;
        }

        float distance = Vector3.Distance(transform.position, m_target.position);
        if(distance <= m_attackRange)
        {
            if (m_attackTime <= m_elapsedTime)
            {
                m_elapsedTime = 0;
                m_animator.SetTrigger("Attack");
            }
        }
        else
        {
            AnimatorStateInfo info = m_animator.GetCurrentAnimatorStateInfo(0);
            if(!m_animator.IsInTransition(0) && !info.IsTag("Attack"))
            {
                if(distance <= m_findRange)
                {
                    ChangeState(MonsterState.TraceTheEnemy);
                }
                else
                {
                    ChangeState(MonsterState.Patrol);
                }
            }
        }
    }

    public void GetDamage()
    {
        if (monster.data.curHp <= 0)
        {
            ChangeState(MonsterState.Die);
            SoundManager.Instance.Play("Monster/GolemDie");
        }

        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 0.8f)
            ChangeState(MonsterState.Idle);
    }

    public void MonsterDie()
    {
        if (monster.data.isDead == false && monster.data.curHp <= 0)
        {
            monster.data.curHp = 0;
            monster.data.isDead = true;
            m_target = null;

            Player.Instance.data.LevelUpdate(monster.data);
            Player.Instance.SetTarget(null);

            // 몬스터 토벌 퀘스트 추가
            if(Player.Instance.MonsterHuntEvent != null)
                Player.Instance.MonsterHuntEvent(monster);

            MonsterManager.Instance.MonsterDead(monster);
        }
    }

    void Update()
    {
        switch(m_currState)
        {
            case MonsterState.Patrol:
                Patrol();
                break;
            case MonsterState.Idle:
                Idle();
                break;
            case MonsterState.TraceTheEnemy:
                TraceTheEnemy();
                break;
            case MonsterState.Attack:
                Attack();
                break;
            case MonsterState.GetDamage:
                GetDamage();
                break;
            case MonsterState.Die:
                MonsterDie();
                break;
        }
    }
}
