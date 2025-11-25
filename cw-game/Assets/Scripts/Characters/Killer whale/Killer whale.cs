using UnityEngine;
using Random = UnityEngine.Random;

public class KillerWhale : Boss
{
    private float detectionRangeX = .3f;
    private float jumpForce = 15000f;
    private float returnDirection = 0f;

    private bool onAttack = false;
    private bool onStunned = false;
    private bool onReturn = false;

    protected override void Update()
    {
        if (playerTransform != null)
        {
            float distanceX = Mathf.Abs(playerTransform.position.x - transform.position.x);

            if (distanceX < detectionRangeX && !onAttack && !onStunned && !onReturn)
            {
                float directionToPlayer = Mathf.Sign(playerTransform.position.x - transform.position.x);
                FlipSprite(directionToPlayer);
                Jump();
            }
        }

        animator.SetBool("Attack", onAttack);
        animator.SetBool("Stunned", onStunned);
        animator.SetBool("Return", onReturn);

        if (!onAttack && !onStunned && !onReturn)
        {
            base.HandlePatrolMovement();
            FlipSprite(currentDirection);
        }
    }

    protected override void FixedUpdate()
    {
        if (onAttack || onStunned)
        {
            return;
        }

        if (onReturn)
        {
            currentDirection = returnDirection;
            FlipSprite(-currentDirection);
        }

        base.FixedUpdate();
    }

    private void Jump()
    {
        if (onAttack) return;

        // Debug.Log("Orca está saltando!");
        float directionToPlayer = Mathf.Sign(playerTransform.position.x - transform.position.x);
        Vector2 jumpVector = new Vector2(directionToPlayer * speed * 0.5f, jumpForce);

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(jumpVector, ForceMode2D.Impulse);
        onAttack = true;
    }

    private float GetRandomDirection()
    {
        int randomValue = Random.Range(0, 2);

        if (randomValue == 0)
        {
            return -1f;
        }
        else
        {
            return 1f;
        }
    }

    private void DisableAttack()
    {
        onAttack = false;
        onStunned = true;
    }

    private void DisableStunned()
    {
        onStunned = false;
        returnDirection = GetRandomDirection();
        onReturn = true;
    }

    private void DisableReturn()
    {
        onReturn = false;
    }
}