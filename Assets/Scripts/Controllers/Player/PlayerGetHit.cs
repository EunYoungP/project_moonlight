﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetHit : StateMachineBehaviour
{
    //Player player;

    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (player == null)
    //        player = animator.GetComponent<Player>();
    //}

    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // 04.23 애니메이션이 80% 이상 실행되었을 때 대기 애니메이션으로 바꿀 수 있도록 추가해 놓았습니다.
    //    if (stateInfo.normalizedTime >= 0.8f)
    //        animator.SetInteger("AniIndex", 0);
    //}

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if(!player.data.isDead)
    //        animator.SetInteger("AniIndex", 0);
    //}
}
