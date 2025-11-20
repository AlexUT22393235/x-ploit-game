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

    protected Vector2 direction;
    protected Rigidbody2D rb;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Update()
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
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with " + collision.collider.name);
    }

    public virtual void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        Debug.Log("Dirección de movimiento: " + direction);
    }

    public virtual void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Detectado salto");
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
            }
        }
    }

    public virtual void OnDash(InputAction.CallbackContext context)
    {
        Debug.Log("Detectado dash");
        if (context.performed && isGrounded)
        {
            Debug.Log("Haciendo dash");
            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        }
    }

    public virtual void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Detectado ataque");
    }

    public virtual void TakeDamage(int damageAmount)
    {
        life -= damageAmount;
        Debug.Log(gameObject.name + " ha recibido " + damageAmount + " de daño. Vida restante: " + life);
    }
}