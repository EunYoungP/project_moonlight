using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemDead : StateMachineBehaviour
{
    //MonsterController golem;

    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (golem == null)
    //        golem = animator.GetComponent<MonsterController>();
    //}

    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // 죽어도 여기서 계속 실행됨
    //    // 아이템 계속 드랍됨
    //    if (golem != null && golem.data.isDead == true)
    //    {
    //        MonsterManager.Instance.MonsterDead(golem);
    //        // 이 함수안에 아이템 드랍 기능 있음
    //        golem.AfterDead(golem);
    //        golem.enabled = false;
    //        golem = null;

    //        return;
    //    }
    //}

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}
}
