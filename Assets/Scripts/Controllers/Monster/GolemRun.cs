using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemRun : StateMachineBehaviour
{
    //MonsterController golem;
    //float fRunTime = 0;

    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (golem == null)
    //        golem = animator.GetComponent<MonsterController>();

    //    //golem.SetState(MonsterState.RUN);
    //}

    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if(golem.player != null && !golem.player.data.isDead)
    //    {
    //        golem.SetTarget();

    //        if (golem.DIS <= golem.fAttackRange)
    //        {
    //            animator.SetInteger("AniIndex", 3);
    //        }

    //        fRunTime += Time.deltaTime;
    //        if(fRunTime >= 10f)
    //        {
    //            if(golem.DIS > golem.fFindRange)
    //            {
    //                golem.target = null;
    //                animator.SetInteger("AniIndex", 0);
    //                fRunTime = 0;
    //            }
    //            else
    //            {
    //                golem.Rotate();
    //                golem.Move(golem.fRunSpeed);
    //                fRunTime = 0;
    //            }
    //        }
    //        golem.Rotate();
    //        golem.Move(golem.fRunSpeed);
    //    }
    //    else if(golem.player == null || golem.player.data.isDead)
    //    {
    //        if(golem.target!= null)
    //            golem.SetTarget();

    //        animator.SetInteger("AniIndex", 0);
    //    }
    //}
}
