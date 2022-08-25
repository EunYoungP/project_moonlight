using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemWalk : StateMachineBehaviour
{
    //    MonsterController golem;

    //    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //    {
    //        if(golem==null)
    //            golem = animator.GetComponent<MonsterController>();

    //        //golem.SetState(MonsterState.WALK);
    //    }

    //    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //    {
    //        if( golem.player != null && golem.player.data != null && !golem.player.data.isDead)
    //        {

    //            // 플레이어 찾는 범위안이라면
    //            if (golem.DIS <= golem.fFindRange)
    //            {
    //                golem.SetTarget();
    //                animator.SetInteger("AniIndex", 2);
    //            }
    //            // 플레이어 찾는 범위밖이라면
    //            else
    //            {
    //                golem.target = null;

    //                if (golem.transform.position == golem.vEnd)
    //                    golem.SetNextState();

    //                golem.Rotate();
    //                golem.Move(golem.fSpeed);
    //            }
    //        }
    //        else if (golem.player == null || golem.player.data.isDead)
    //        {
    //            if(golem.target!= null)
    //                golem.SetTarget();
    //            else
    //            {
    //                if (golem.transform.position == golem.vEnd)
    //                    golem.SetNextState();

    //                golem.Rotate();
    //                golem.Move(golem.fSpeed);
    //            }
    //        }
    //    }
}
