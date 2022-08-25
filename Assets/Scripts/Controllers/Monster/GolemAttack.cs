using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAttack : StateMachineBehaviour
{
    //MonsterController golem;

    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if(golem == null)
    //        golem = animator.GetComponent<MonsterController>();
    //}

    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (golem.player != null && !golem.player.data.isDead)
    //    {
    //        golem.SetTarget();

    //        if (golem.player.data.isDead == true)
    //        {
    //            golem.target = null;
    //            animator.SetInteger("AniIndex", 0);
    //        }
    //        else if (golem.DIS > golem.fAttackRange)
    //        {
    //            //golem.target = null;
    //            animator.SetInteger("AniIndex", 2);

    //            if (golem.DIS > golem.fFindRange)
    //            {
    //                animator.SetInteger("AniIndex", 2);
    //            }
    //        }
    //        golem.Rotate();
    //    }
    //    else if (golem.player == null || golem.player.data.isDead)
    //    {
    //        if(golem.target != null)
    //            golem.SetTarget();

    //        animator.SetInteger("AniIndex", 0);
    //    }
    //}

    //public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    //{
    //    //golem.Rotate();
    //}
}
