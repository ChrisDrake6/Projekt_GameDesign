using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    public Vector3 direction = Vector3.zero;
    float speed = 10;
    Vector3 currentDestination;
    float cellWidth;

    // TODO: Use coordinates instead
    float length = 20;

    public override void EnterState(PlayerStateManager player)
    {
        if (direction.x != 0)
        {
            cellWidth = player.tilemap.cellSize.x;
        }
        else if (direction.y != 0)
        {
            cellWidth = player.tilemap.cellSize.y;
        }
        if (!player.transitioningBetweenStages)
        {
            currentDestination = player.tilemap.GetCellCenterLocal(player.tilemap.LocalToCell(player.transform.position + direction * length * cellWidth));
        }
        else
        {
            currentDestination = direction;
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.transform.position = Vector2.MoveTowards(player.transform.position, currentDestination, speed * Time.deltaTime);
        if (currentDestination == player.transform.position)
        {
            if (!player.transitioningBetweenStages)
            {
                player.CallForNextOrder();
            }
            else
            {
                player.transitioningBetweenStages = false;
                player.SwitchState(player.idleState);
            }
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player)
    {
        player.CallForNextOrder();
    }
}
