using UnityEngine;

public class PinguinoMelee : Enemy
{
    private float dashForce = 10f;

    private bool hasDashed = false;
    private bool onDash = false;
    private bool onWalk = false;

    private bool onDamage = false;
    private bool die = false;

    private Animator animator;

    private int dashDamage = 10;

    private bool damageDealt = false;
    private bool playerDetected = false;

    public override void Start()
    {
        base.Start();

        life = 45;
        attackRange = 0.29f;
        onAttack = false;
        damage = 15;

        animator = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();

        if (player == null)
        {
            SetAnimationStates(false, false, false, false, die);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (!playerDetected)
            {
                playerDetected = true;
            }

            if (distanceToPlayer <= attackRange)
            {
                onAttack = true;
                onWalk = false;
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
            }
        }
        else
        {
            onWalk = false;
            onAttack = false;
        }

        if (onDash)
        {
            onWalk = false;
        }

        SetAnimationStates(onDash, onWalk, onAttack, onDamage, die);

        if (!onDash)
        {
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (onDash)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.AddForce(new Vector2(direction.x * dashForce, 0), ForceMode2D.Impulse);
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        Player playerHealth = collision.gameObject.GetComponent<Player>();

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

    private void SetAnimationStates(bool dash, bool walk, bool attack, bool damage, bool die)
    {
        animator.SetBool("Dash", dash);
        animator.SetBool("Walk", walk);
        animator.SetBool("Attack", attack);
        animator.SetBool("Damage", damage);
        animator.SetBool("Die", die);
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

    public override void OnAttack()
    {
        base.OnAttack();
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Player playerHealth = player.GetComponent<Player>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    public void DisableDash()
    {
        onDash = false;
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

    private void Die()
    {
        Destroy(gameObject);
    }
}