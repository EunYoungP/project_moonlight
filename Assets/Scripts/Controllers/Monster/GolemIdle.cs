using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//랜덤으로 걷기 - 기본 상태 바꾸는 AI
//플레이어가 범위안에들어오면 일정시간 동안 달려서 쫓아가고,
// 시간이 다됐을때, 범위밖이라면 다시 랜덤 기본-걷기, 범위안이라면 계속 달려 쫓아가기
//만약 달리는중 플레이어가 공격범위안에들어오면 공격,
//공격하다가 플레이어가 범위밖으로 나가면 달리기

//1. 다른 상태로 전환시 부자연스러움
//2. 플레이어 발견시 발견했다는 동작 넣기

// 타겟이 player가 아닐수도 있기때문에 
// 범위에들어온 오브젝트들을 검사해서 타겟설정하는 기능추가

public class GolemIdle : StateMachineBehaviour
{
    //MonsterController golem;
    //float timer = 0;
    //float waitTime = 2.0f;

    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if(golem==null)
    //        golem = animator.GetComponent<MonsterController>();

    //    //golem.SetState(MonsterState.IDLE);
    //}

    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (golem.player != null && !golem.player.data.isDead)
    //    {
    //        if (golem.DIS <= golem.fFindRange)
    //        {
    //            golem.SetTarget();
    //            animator.SetInteger("AniIndex", 3);

    //            if (golem.DIS <= golem.fAttackRange)
    //            {
    //                golem.SetTarget();
    //                animator.SetInteger("AniIndex", 4);
    //            }
    //        }
    //        else if (golem.DIS > golem.fFindRange)
    //        {
    //            golem.target = null;
    //            if (golem.transform.position == golem.vEnd)
    //            {
    //                timer += Time.deltaTime;

    //                if (timer > waitTime)
    //                {
    //                    golem.SetNextState();
    //                    timer = 0;
    //                }
    //            }
    //            else
    //            {
    //                animator.SetInteger("AniIndex", 1);
    //            }
    //        }
    //    }
    //    else if (golem.player != null || golem.player.data.isDead)
    //    {
    //        if (golem.transform.position == golem.vEnd)
    //        {
    //            timer += Time.deltaTime;

    //            if (timer > waitTime)
    //            {
    //                golem.SetNextState();
    //                timer = 0;
    //            }
    //        }
    //        else
    //        {
    //            animator.SetInteger("AniIndex", 1);
    //        }
    //    }
    //}
}
