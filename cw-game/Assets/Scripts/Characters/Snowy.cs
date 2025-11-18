using UnityEngine;
using UnityEngine.InputSystem;

public class Snowy : MonoBehaviour
{
    private float speed = 1f;
    private float force = 180f;

    private bool isGrounded = false;

    private bool jumping = false;

    private bool onDash = false;
    private bool attacking = false;

    private bool onDamage = false;

    private float velocityFireball = 1f;

    public int life = 100;

    private Vector2 direction;
    private int lastDirection = 1;
    private Animator animator;

    private Rigidbody2D rb;

    public GameObject fireballPrefab;

    void Start()
    {
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

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

        animator.SetFloat("Speed", direction.magnitude);
        animator.SetBool("Jump", jumping);
        animator.SetBool("Attack", attacking);
        animator.SetBool("Dash", onDash);
        animator.SetBool("Damage", onDamage);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            isGrounded = true;
            jumping = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        // Debug.Log("Dirección de movimiento: " + direction);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Debug.Log("Detectado salto");
        if (context.performed && isGrounded)
        {
            jumping = true;

            rb.AddForce(Vector2.up * force * 1.3f, ForceMode2D.Impulse);

            isGrounded = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        // Debug.Log("Detectado dash");
        if (context.performed && isGrounded)
        {
            onDash = true;
            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // Debug.Log("Detectado ataque");
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

    public void TakeDamage(int damageAmount)
    {
        onDamage = true;
        life -= damageAmount;
        // Debug.Log("Snowy ha recibido " + damageAmount + " de daño. Vida restante: " + life);

        if (life <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Snowy ha muerto.");
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
}