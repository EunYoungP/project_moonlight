using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDead : StateMachineBehaviour
{
    Player player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
            player = animator.GetComponent<Player>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(player.data.isDead == true)
        {
            // ui띄워줌
            player.enabled = false;
        }
    }
}
