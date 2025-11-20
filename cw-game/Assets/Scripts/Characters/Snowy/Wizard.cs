using UnityEngine;
using UnityEngine.InputSystem;

public class SnowyWizard : Player
{
    private bool jumping = false;
    private bool onDash = false;
    private bool attacking = false;
    private bool onDamage = false;
    private bool die = false;

    private float velocityFireball = 2f;

    private Animator animator;
    public GameObject fireballPrefab;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();
        if (die) return;
        animator.SetFloat("Speed", direction.magnitude);
        animator.SetBool("Jump", jumping);
        animator.SetBool("Attack", attacking);
        animator.SetBool("Dash", onDash);
        animator.SetBool("Damage", onDamage);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.collider.CompareTag("Floor"))
        {
            isGrounded = true;
            jumping = false;
        }
    }

    public override void OnJump(InputAction.CallbackContext context)
    {
        base.OnJump(context);

        if (context.performed)
        {
            if (jumps >= 1 && jumps <= 2)
            {
                switch (jumps)
                {
                    case 1:
                        jumping = true;
                        onDash = false;
                        break;
                    case 2:
                        jumping = false;
                        onDash = true;
                        break;
                }
            }
        }
    }

    public override void OnDash(InputAction.CallbackContext context)
    {
        base.OnDash(context);
        if (context.performed && isGrounded)
        {
            onDash = true;
        }
    }

    public override void OnAttack(InputAction.CallbackContext context)
    {
        base.OnAttack(context);
        if (context.performed && !attacking && isGrounded)
        {
            attacking = true;

            Invoke("ThrowFireball", 0.85f);
        }
    }

    void ThrowFireball()
    {
        Vector3 spawnOffset = new Vector3(.25f * lastDirection, 0f, 0f);
        GameObject nuevoProyectil = Instantiate(fireballPrefab, transform.position + spawnOffset, transform.rotation);
        Rigidbody2D rb = nuevoProyectil.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(lastDirection < 0 ? -velocityFireball : velocityFireball, 0f);
        }
    }

    public override void TakeDamage(int damageAmount)
    {
        onDamage = true;
        base.TakeDamage(damageAmount);

        if (life <= 0)
        {
            die = true;
            animator.SetBool("Die", die);
        }
    }

    public void DisableJump()
    {
        jumping = false;
    }

    public void DisableDash()
    {
        onDash = false;
    }

    public void DisableAttack()
    {
        attacking = false;
    }

    public void DisableDamage()
    {
        onDamage = false;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}