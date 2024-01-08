using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerStateManager : MonoBehaviour
{
    public Tilemap tilemap;
    public bool transitioningBetweenStages = false;

    PlayerBaseState currentState;
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerWalkingState walkState = new PlayerWalkingState();
    public PlayerOpeningDoorState openDoorState = new PlayerOpeningDoorState();

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
                case CodeBlockOpenDoor openDoorOrder:
                    SwitchState(openDoorState);
                    break;
                default:
                    throw new Exception("CodeBlock unknown!");
            }
        }
        else if(!transitioningBetweenStages)
        { 
            SwitchState(idleState); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter(this);
    }

    public void SetPlayerPosition(Vector3 targetPosition)
    {
        transform.position = tilemap.GetCellCenterLocal(tilemap.LocalToCell(targetPosition));
    }

    public void ProgressToNextStage(Vector3 nextStartingPosition)
    {
        transitioningBetweenStages = true;
        walkState.direction = nextStartingPosition;
        SwitchState(walkState);
    }

    public void CheckForWin()
    {
        SwitchState(idleState);
        GameManager.Instance.CheckForWin();
    }
}
