using UnityEngine;

public class PinguinoMelee : MonoBehaviour
{
    public Transform player;
    private float detectionRange = 3f;
    private float speed = 3f;
    private float dashForce = 10f;
    private float attackRange = 0.29f;
    private int life = 50;

    private bool playerDetected = false;
    private bool hasDashed = false;
    private bool onDash = false;
    private bool onWalk = false;
    private bool onAttack = false;

    private bool onDamage = false;

    private Vector2 movement;
    private Animator animator;
    private Rigidbody2D rb;
    private int dashDamage = 10;
    private int meleeDamage = 5;
    private bool damageDealt = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 direction = (player.position - transform.position).normalized;

        if (distanceToPlayer <= detectionRange)
        {
            if (!playerDetected)
            {
                playerDetected = true;
            }

            if (direction.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (direction.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (distanceToPlayer <= attackRange)
            {
                onAttack = true;
                onWalk = false;
                movement = Vector2.zero;
            }
            else if (playerDetected && !hasDashed)
            {
                onDash = true;
                hasDashed = true;
                onWalk = false;
                onAttack = false;
            }
            else if (!onDash && !onAttack)
            {
                onWalk = true;
                movement = new Vector2(direction.x, 0);
            }
        }
        else
        {
            movement = Vector2.zero;
            onWalk = false;
            onAttack = false;
        }

        if (onDash)
        {
            onWalk = false;
        }

        animator.SetBool("Dash", onDash);
        animator.SetBool("Walk", onWalk);
        animator.SetBool("Attack", onAttack);
        animator.SetBool("Damage", onDamage);

        if (!onDash && !onAttack)
        {
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (onDash)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            movement = new Vector2(direction.x, 0);
            rb.AddForce(new Vector2(direction.x * dashForce, 0), ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Snowy playerHealth = collision.gameObject.GetComponent<Snowy>();

        if (playerHealth != null)
        {
            if (onDash && !damageDealt)
            {
                // float collisionSpeed = collision.relativeVelocity.magnitude;
                // // Daño = (Velocidad * Masa del Enemigo) / Constante de Ajuste
                // int calculatedDamage = Mathf.RoundToInt(collisionSpeed * rb.mass * 0.5f);

                // playerHealth.TakeDamage(calculatedDamage);
                playerHealth.TakeDamage(dashDamage);

                damageDealt = true;
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        onDamage = true;
        life -= damageAmount;
        // Debug.Log("Enemigo ha recibido " + damageAmount + " de daño. Vida restante: " + life);

        if (life <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemigo muerto.");
    }

    public void DealMeleeDamage()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Snowy playerHealth = player.GetComponent<Snowy>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(meleeDamage);
            }
        }
    }

    public void DisableDash()
    {
        onDash = false;
        damageDealt = false;
        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            onWalk = true;
        }
    }

    public void DisableAttack()
    {
        onAttack = false;
    }

    public void DisableDamage()
    {
        onDamage = false;
    }
}