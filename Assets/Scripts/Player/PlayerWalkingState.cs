using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    public Vector3 direction = Vector3.zero;
    public float rayCastDistance = 2;
    public float speed = 7.5F;
    private Animator animator;

    Vector3 currentDestination;
    float cellWidth;
    // TODO: Use coordinates instead
    float length = 20;

    public override void EnterState(PlayerStateManager player)
    {
        LayerMask mask = LayerMask.GetMask("Interactable", "Obstacles");
        if (Physics2D.Raycast(player.transform.position, player.walkState.direction, rayCastDistance, mask))
        {
            GameManager.Instance.ValidateUserInput(false, "I cannot walk through walls, you know?");
            player.SwitchState(player.idleState);
            return;
        }

        animator = player.GetComponent<Animator>();

        if (direction.x < 0)
        {
            animator.SetBool("isWalkingLeft", true);
            animator.SetBool("isWalkingRight", false);
            animator.SetBool("isWalkingDown", false);
            animator.SetBool("isWalkingUp", false);
        }
        else if (direction.x > 0)
        {
            animator.SetBool("isWalkingLeft", false);
            animator.SetBool("isWalkingRight", true);
            animator.SetBool("isWalkingDown", false);
            animator.SetBool("isWalkingUp", false);
        }
        else if (direction.y < 0)
        {
            animator.SetBool("isWalkingLeft", false);
            animator.SetBool("isWalkingRight", false);
            animator.SetBool("isWalkingDown", true);
            animator.SetBool("isWalkingUp", false);
        }
        else if (direction.y > 0)
        {
            animator.SetBool("isWalkingLeft", false);
            animator.SetBool("isWalkingRight", false);
            animator.SetBool("isWalkingDown", false);
            animator.SetBool("isWalkingUp", true);
        }

        cellWidth = player.tilemap.cellSize.y;

        if (!player.transitioningBetweenStages)
        {
            currentDestination = player.transform.position + direction * length * cellWidth;
        }
        else
        {
            currentDestination = direction;
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.transform.position = Vector2.MoveTowards(player.transform.position, currentDestination, speed * Time.deltaTime);

        if (!SoundManager.Instance.audiosource.isPlaying)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.codystep, 0.5f);
        }

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
                    player.wallmap.LocalToCell(new Vector3(player.transform.position.x - cellWidth, player.transform.position.y, 0)),
                    player.tileForEntranceBlocking);

                player.SwitchState(player.idleState);

                GameManager.Instance.progressing = false;
            }
        }
    }

    public override void OnCollisionEnter(PlayerStateManager player)
    {

        SoundManager.Instance.PlaySound(SoundManager.Instance.codycollision);

        player.CallForNextOrder();
    }
}
