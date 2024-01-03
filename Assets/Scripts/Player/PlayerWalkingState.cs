using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    public Vector3 direction;
    public float speed = 10;
    Vector3 currentDestination;

    // TODO: Use coordinates instead
    float length = 10;

    public override void EnterState(PlayerStateManager player)
    {
        currentDestination = player.transform.position + direction * length;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.transform.position = Vector2.MoveTowards(player.transform.position, currentDestination, speed * Time.deltaTime);
        if(currentDestination == player.transform.position)
        {
            player.CallForNextOrder();
        }
    }
}
