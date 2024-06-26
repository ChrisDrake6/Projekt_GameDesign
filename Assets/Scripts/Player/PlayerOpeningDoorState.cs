using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOpeningDoorState : PlayerBaseState
{
    public float rayCastDistance = 2;

  

    public override void EnterState(PlayerStateManager player)
    {
        LayerMask mask = LayerMask.GetMask("Interactable");
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, player.walkState.direction, rayCastDistance, mask);

        if (hit.transform != null && hit.transform.CompareTag("Door"))
        {
            // TODO: Check for key
            hit.transform.gameObject.SetActive(false);
            SoundManager.Instance.PlaySound(SoundManager.Instance.dooropen);

            GameManager.Instance.ValidateUserInput(true, "");



        }
        else
        {
            GameManager.Instance.ValidateUserInput(false, "There was no door!");
            player.SwitchState(player.idleState);
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player)
    {
    }

    public override void UpdateState(PlayerStateManager player)
    {
    }
}
