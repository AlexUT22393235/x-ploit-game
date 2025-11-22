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
        pointsOnDefeat = 50;
    }

    public override void Update()
    {
        if (playerComponent == null)
        {
            SetAnimationStates(false, false, false, false);
            return;
        }

        base.Update();

        bool platformAhead = edgeSensor.IsPlatformAhead;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

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

            if (!platformAhead)
            {
                onWalk = false;
                movement = Vector2.zero;
            }
        }

        SetAnimationStates(onDash, onWalk, onAttack, onDamage);
    }

    public override void FixedUpdate()
    {
        if (onDash)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.AddForce(new Vector2(direction.x * dashForce, 0), ForceMode2D.Impulse);
        }

        else
        {
            base.FixedUpdate();
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (onDash && !damageDealt)
        {
            // float collisionSpeed = collision.relativeVelocity.magnitude;
            // // Daño = (Velocidad * Masa del Enemigo) / Constante de Ajuste
            // int calculatedDamage = Mathf.RoundToInt(collisionSpeed * rb.mass * 0.5f);

            // playerHealth.TakeDamage(calculatedDamage);
            playerComponent.TakeDamage(dashDamage);

            damageDealt = true;
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
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRange)
        {
            playerComponent.TakeDamage(damage);
        }
    }

    protected void DisableDash()
    {
        onDash = false;
        if (Vector2.Distance(transform.position, playerTransform.position) > attackRange)
        {
            onWalk = true;
        }
    }
}