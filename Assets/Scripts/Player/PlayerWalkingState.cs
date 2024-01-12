using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    public Vector3 direction = Vector3.zero;
    public float rayCastDistance = 2;
    public float rayCastOffset = 1.5F;
    public float speed = 10;

    Vector3 currentDestination;
    float cellWidth;

    // TODO: Use coordinates instead
    float length = 20;

    public override void EnterState(PlayerStateManager player)
    {
        Vector3 origin = player.transform.position;
        origin.y -= rayCastOffset;
        if (Physics2D.Raycast(origin, player.walkState.direction, rayCastDistance))
        {
            //ToDo: Make this a losing condition? This would mean a move order too much has been called.
            player.CallForNextOrder();
            return;
        }

        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        cellWidth = player.tilemap.cellSize.y;

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

                // TODO: Remove this! Quick and dirty
                // Set Collider at Entrance to block bachtracking
                player.wallmap.SetTile(
                    player.wallmap.LocalToCell(new Vector3(player.transform.position.x - cellWidth, player.transform.position.y - player.transform.lossyScale.y / 2 - 1, 0)),
                    player.tileForEntranceBlocking);

                player.SwitchState(player.idleState);
            }
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player)
    {
        player.CallForNextOrder();
    }
}
