using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOpeningDoorState : PlayerBaseState
{
    public float rayCastDistance = 2;

    public override void EnterState(PlayerStateManager player)
    {
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, player.walkState.direction, rayCastDistance);

        if (hit.transform != null && hit.transform.CompareTag("Door"))
        {
            // TODO: Check for key
            GameManager.Instance.SetWinConditionAchieved();
            hit.transform.gameObject.SetActive(false);
        }
        player.CallForNextOrder();
    }

    public override void OnCollisionEnter(PlayerStateManager player)
    {
    }

    public override void UpdateState(PlayerStateManager player)
    {
    }
}
