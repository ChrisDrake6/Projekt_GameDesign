using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerStateManager : MonoBehaviour
{
    public Tilemap tilemap;

    // TODO: Remove this! Quick and dirty
    public Tilemap wallmap;
    public TileBase tileForEntranceBlocking;
    public SpriteRenderer spriteRenderer;

    public bool transitioningBetweenStages = false;

    PlayerBaseState currentState;
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerWalkingState walkState = new PlayerWalkingState();
    public PlayerOpeningDoorState openDoorState = new PlayerOpeningDoorState();

    bool flippedOnDefault;
    Compiler compiler;
    float collisionDuration = 0;

    void Start()
    {
        compiler = Compiler.Instance;
        SwitchState(idleState);
    }

    void Update()
    {
        LayerMask mask = LayerMask.GetMask("Obstacle");
        if (Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.size.y / 2), new Vector2(2, 2), 0, Vector2.down, 2, mask))
        {
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("Walls");
        }
        else
        {
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("Obstacles");
        }
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
        else if (!transitioningBetweenStages)
        {
            SwitchState(idleState);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter(this);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Failsave: if player happens to get stuck on any random collider, call oncollision
        collisionDuration += Time.deltaTime;
        if (collisionDuration >= 3)
        {
            collisionDuration = 0;
            currentState.OnCollisionEnter(this);
        }
    }

    public void SetPlayerPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
        spriteRenderer.flipX = flippedOnDefault;
    }

    public void ProgressToNextStage(Vector3 nextStartingPosition)
    {
        flippedOnDefault = spriteRenderer.flipX;
        transitioningBetweenStages = true;
        walkState.direction = nextStartingPosition;
        SwitchState(walkState);
    }
}
