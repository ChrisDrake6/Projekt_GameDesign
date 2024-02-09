using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    private Animator animator;

    public override void EnterState(PlayerStateManager player)
    {
        animator = player.GetComponent<Animator>();
        animator.SetBool("isWalkingLeft", false);
        animator.SetBool("isWalkingRight", false);
        animator.SetBool("isWalkingDown", false);
        animator.SetBool("isWalkingUp", false);
    }
    public override void UpdateState(PlayerStateManager player)
    {
    }

    public override void OnCollisionEnter(PlayerStateManager player)
    {
    }
}
