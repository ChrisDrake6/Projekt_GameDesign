using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerStateManager : MonoBehaviour
{
    public Tilemap tilemap;

    PlayerBaseState currentState;
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerWalkingState walkState = new PlayerWalkingState();

    Compiler compiler;

    void Start()
    {
        compiler = Compiler.Instance;
        SwitchState(idleState);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void CallForNextOrder()
    {
        CodeBlock order = compiler.GetNextOrder();
        if (order != null)
        {
            switch (order)
            {
                case CodeBlockWalk walkOrder:
                    walkState.direction = walkOrder.direction;
                    SwitchState(walkState);
                    break;
                default:
                    throw new Exception("CodeBlock unknown!");
            }
        }
        else
        {
            SwitchState(idleState);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter(this);
    }

    public void SetPlayerPosition(Vector3Int targetPosition)
    {
        transform.position = tilemap.GetCellCenterLocal(targetPosition);
    }

    public void ProgressToNextStage(Vector3 nextStartingPosition)
    {
        walkState.direction = nextStartingPosition;
        SwitchState(walkState);
    }
}
