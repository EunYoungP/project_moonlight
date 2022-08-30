using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    private PlayerController playerController;

    public void Init()
    {
        this.playerController = Player.Instance.playerController;
    }

    private void SetTraceTarget(Transform targetPos, float attackRange = 0)
    {
        Vector3 dir = targetPos.transform.position - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();

        playerController.Rotate(targetPos.transform.position);
        Vector3 destination = transform.position + dir * (distance - attackRange);

        playerController.GetTargetPos(destination, targetPos);
    }

    private void SetTracePos(Vector3 destination)
    {
        playerController.targetPos = null;
        playerController.Rotate(destination);

        playerController.GetTargetPos(destination);
    }

    public void GameInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                // 몬스터
                if (hitInfo.collider.CompareTag("Monster"))
                {
                    SetTraceTarget(hitInfo.collider.gameObject.transform, playerController.attackRange);
                }
                // 바닥
                else if (hitInfo.collider.CompareTag("Terrain"))
                {
                    AnimatorStateInfo info = playerController.animator.GetCurrentAnimatorStateInfo(0);
                    if (info.IsName("BaseAttack") || playerController.animator.IsInTransition(0))
                        return;

                    SetTracePos(hitInfo.point);
                }
                // 포탈
                else if (hitInfo.collider.CompareTag("Portal"))
                {
                    SetTracePos(hitInfo.point);
                }
                // 아이템
                else if (hitInfo.collider.GetComponent<PickUpItem>() != null)
                {
                    SetTraceTarget(hitInfo.collider.gameObject.transform);
                }
                // 엔피씨
                else if (hitInfo.collider.GetComponent<NPC>() != null)
                {
                    SetTraceTarget(hitInfo.collider.gameObject.transform, playerController.attackRange);
                }
                playerController.ChangeState(PlayerState.Move);
            }
        }
    }
}
