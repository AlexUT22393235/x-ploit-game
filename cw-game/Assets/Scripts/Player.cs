using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    protected float speed = 1f;
    protected float force = 180f;
    public int life = 100;
    protected int jumps = 0;
    protected int lastDirection = 1;

    protected bool isGrounded = false;
    protected bool jumping = false;
    protected bool onDash = false;
    protected bool attacking = false;
    protected bool onDamage = false;
    protected bool die = false;

    protected Vector2 direction;
    protected Rigidbody2D rb;
    protected Animator animator;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public virtual void Update()
    {
        if (die)
        {
            SetAnimationStates(direction.magnitude, false, false, false, false);
            return;
        }
        
        transform.Translate(direction * speed * Time.deltaTime);

        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            lastDirection = -1;
        }
        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            lastDirection = 1;
        }

        SetAnimationStates(direction.magnitude, jumping, attacking, onDash, onDamage);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with " + collision.collider.name);

        if (collision.collider.CompareTag("Floor") || collision.collider.CompareTag("Enemy"))
        {
            isGrounded = true;
            jumping = false;
        }
    }

    public virtual void SetAnimationStates(float speed, bool jump, bool attack, bool dash, bool damage)
    {
        animator.SetFloat("Speed", speed);
        animator.SetBool("Jump", jump);
        animator.SetBool("Attack", attack);
        animator.SetBool("Dash", dash);
        animator.SetBool("Damage", damage);
    }

    protected virtual void OnMove(InputAction.CallbackContext context)
    {
        // Debug.Log("Dirección de movimiento: " + direction);
        direction = context.ReadValue<Vector2>();
    }

    protected virtual void OnJump(InputAction.CallbackContext context)
    {
        if (die) return;
        // Debug.Log("Detectado salto");

        if (context.performed)
        {
            Debug.Log("Haciendo salto");
            if (isGrounded || jumps < 2)
            {
                if (isGrounded)
                {
                    jumps = 0;
                }

                // rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Resetear la velocidad vertical para un salto consistente
                // rb.AddForce(Vector2.up * force * 1.5f, ForceMode2D.Impulse);
                rb.AddForce(Vector2.up * force * 1.6f, ForceMode2D.Impulse);

                jumps++;
                isGrounded = false;

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
    }

    protected virtual void OnDash(InputAction.CallbackContext context)
    {
        if(die) return;
        // Debug.Log("Detectado dash");

        if (context.performed && isGrounded)
        {
            Debug.Log("Haciendo dash");
            onDash = true;
            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        }
    }

    protected virtual void OnAttack(InputAction.CallbackContext context)
    {
        if(die) return;
        // Debug.Log("Detectado ataque");

        if (context.performed && !attacking && isGrounded)
        {
            attacking = true;
        }
    }

    public virtual void TakeDamage(int damageAmount)
    {
        life -= damageAmount;
        // Debug.Log(gameObject.name + " ha recibido " + damageAmount + " de daño. Vida restante: " + life);

        onDamage = true;

        if (life <= 0)
        {
            die = true;
            animator.SetBool("Die", die);
        }
    }

    protected void DisableJump()
    {
        jumping = false;
    }

    protected void DisableDash()
    {
        onDash = false;
    }

    protected void DisableAttack()
    {
        attacking = false;
    }

    protected void DisableDamage()
    {
        onDamage = false;
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}