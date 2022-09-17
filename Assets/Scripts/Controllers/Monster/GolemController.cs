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
    private Monster monster;

    private MonsterState curState = MonsterState.None;
    private Animator animator;
    private float elapsedTime;
    private float attackTime = 2f;
    private List<Vector3> patrolList = new List<Vector3>();
    private int patrolIndex = 0;

    private Transform target;
    private float findRange = 10f;
    private float attackRange = 2.5f;
    private float patrolSpeed = 2;
    private float tracteSpeed = 5;

    private float idlePrevTime;
    private float idleInterval = 2f;

    private void Start()
    {
        monster = gameObject.GetComponent<Monster>();
        animator = GetComponent<Animator>();
        Transform t = transform.Find("Patrol");
        if (t != null)
        {
            for (int i = 0; i < t.childCount; ++i)
            {
                patrolList.Add(t.GetChild(i).position);
            }
        }
        ChangeState(MonsterState.Patrol);
    }

    public void ChangeState(MonsterState state)
    {
        if (curState == state )
            return;

        curState = state;
        switch (state)
        {
            case MonsterState.Idle:
                animator.SetBool("Walk", false);
                animator.SetBool("Run", false);
                idlePrevTime = Time.time;
                break;
            case MonsterState.Patrol:
                animator.SetBool("Walk", true);
                animator.SetBool("Run", false);
                break;
            case MonsterState.TraceTheEnemy:
                animator.SetBool("Walk", false);
                animator.SetBool("Run", true);
                break;
            case MonsterState.Attack:
                {
                    animator.SetBool("Walk", false);
                    animator.SetBool("Run", false);
                    Vector3 dir = target.position - transform.position;
                    dir.y = 0;
                    transform.rotation = Quaternion.LookRotation(dir);
                    animator.SetTrigger("Attack");
                }
                break;
            case MonsterState.GetDamage:
                animator.SetBool("Walk", false);
                animator.SetBool("Run", false);
                animator.SetTrigger("Damage");
                break;
            case MonsterState.Die:
                animator.SetBool("Walk", false);
                animator.SetBool("Run", false);
                animator.SetTrigger("Die");
                break;
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;

        if(this.target != null)
            ChangeState(MonsterState.TraceTheEnemy);
    }

    public Transform GetTarget()
    {
        return target;
    }

    private void StateIdle()
    {
        // 시간간격동안 idle 상태 유지
        if((Time.time - idlePrevTime) >= idleInterval)
        {
            ChangeState(MonsterState.Patrol);
        }
    }

    private void StatePatrol()
    {
        transform.position =  Vector3.MoveTowards(transform.position, patrolList[patrolIndex], Time.deltaTime * patrolSpeed);

        Vector3 dir =  patrolList[patrolIndex] - transform.position;

        // 높낮이 있을 경우를 대비
        dir.y = 0;  
        transform.rotation = Quaternion.LookRotation(dir);

        float distance = Vector3.Distance(transform.position, patrolList[patrolIndex]);
        if (distance <= 0.5f )
        {
            ++patrolIndex;
            if (patrolIndex >= patrolList.Count)
                patrolIndex = 0;
        }
    }

    private void StateTraceTheEnemy()
    {
        if (target == null)
            return;

        Vector3 _target = target.transform.position - transform.position;
        _target.y = 0;
        transform.rotation = Quaternion.LookRotation(_target);

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if(distance >= findRange)
        {
            ChangeState(MonsterState.Idle);
        }
        else if(distance <= attackRange)
        {
            ChangeState(MonsterState.Attack);
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * tracteSpeed);
    }

    private void StateAttack()
    {
        elapsedTime += Time.deltaTime;

        if (target == null)
        {
            AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
            if(stateinfo.normalizedTime >= 0.8)
                ChangeState(MonsterState.Idle);
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);
        if(distance <= attackRange)
        {
            if (attackTime <= elapsedTime)
            {
                elapsedTime = 0;
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if(!animator.IsInTransition(0) && !info.IsTag("Attack"))
            {
                if(distance <= findRange)
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

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 0.8f)
            ChangeState(MonsterState.Idle);
    }

    private void StateMonsterDie()
    {
        if (monster.data.isDead == false && monster.data.curHp <= 0)
        {
            monster.data.curHp = 0;
            monster.data.isDead = true;
            target = null;

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
        switch(curState)
        {
            case MonsterState.Patrol:
                StatePatrol();
                break;
            case MonsterState.Idle:
                StateIdle();
                break;
            case MonsterState.TraceTheEnemy:
                StateTraceTheEnemy();
                break;
            case MonsterState.Attack:
                StateAttack();
                break;
            case MonsterState.GetDamage:
                GetDamage();
                break;
            case MonsterState.Die:
                StateMonsterDie();
                break;
        }
    }
}
