using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOpeningDoorState : PlayerBaseState
{
    public float rayCastDistance = 2;
    public float rayCastOffset = 1.5F;

    public override void EnterState(PlayerStateManager player)
    {
        Vector3 origin = player.transform.position;
        origin.y -= rayCastOffset;

        RaycastHit2D hit = Physics2D.Raycast(origin, player.walkState.direction, rayCastDistance);

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
