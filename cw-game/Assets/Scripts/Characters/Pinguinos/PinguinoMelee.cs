using UnityEngine;

public class PinguinoMelee : Enemy
{
    private float dashForce = 10f;
    private int dashDamage = 10;

    private bool hasDashed = false;
    private bool onDash = false;

    private bool damageDealt = false;

    public override void Start()
    {
        base.Start();

        life = 45;
        attackRange = 0.29f;
        damage = 15;
        attackDelay = .6f;
    }

    public override void Update()
    {
        if (player == null)
        {
            return;
        }
        
        base.Update();

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
            {
                onAttack = true;
                onWalk = false;
            }

            else if (!hasDashed)
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

        SetAnimationStates(onDash, onWalk, onAttack, onDamage);
    }

    public override void FixedUpdate()
    {
        if (onDash)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.AddForce(new Vector2(direction.x * dashForce, 0), ForceMode2D.Impulse);
        }

        else
        {
            base.FixedUpdate();
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

    private void SetAnimationStates(bool dash, bool walk, bool attack, bool damage)
    {
        animator.SetBool("Dash", dash);
        animator.SetBool("Walk", walk);
        animator.SetBool("Attack", attack);
        animator.SetBool("Damage", damage);
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

    protected void DisableDash()
    {
        onDash = false;
        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            onWalk = true;
        }
    }
}