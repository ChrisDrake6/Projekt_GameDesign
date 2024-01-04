using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    public Vector3 direction = Vector3.zero;
    float speed = 10;
    Vector3 currentDestination;
    float cellWidth;

    // TODO: Use coordinates instead
    float length = 5;

    public override void EnterState(PlayerStateManager player)
    {
        cellWidth = player.tilemap.cellSize.x;
        currentDestination = player.tilemap.GetCellCenterLocal(player.tilemap.LocalToCell(player.transform.position + direction * length * cellWidth));
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.transform.position = Vector2.MoveTowards(player.transform.position, currentDestination, speed * Time.deltaTime);
        if (currentDestination == player.transform.position)
        {
            player.CallForNextOrder();
        }
    }
}
