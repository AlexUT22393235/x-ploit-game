using UnityEngine;

public class PinguinoDistance : Enemy
{
    private bool onDamage = false;
    private bool die = false;

    private Animator animator;
    public GameObject iceSpikePrefab;

    private float velocityIceSpike = 3f;

    public override void Start()
    {
        base.Start();

        life = 30;
        attackRange = 3f;
        onAttack = false;

        animator = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();

        if (player == null)
        {
            SetAnimationStates(false, false, die);
            return;
        }

        movement = Vector2.zero;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            onAttack = true;
        }
        else
        {
            onAttack = false;
        }

        SetAnimationStates(onAttack, onDamage, die);
    }

    private void SetAnimationStates(bool attack, bool damage, bool die)
    {
        animator.SetBool("Attack", attack);
        animator.SetBool("Damage", damage);
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

        Vector3 spawnOffset = new Vector3(.1f * lastDirection, 0.1f, 0f);
        GameObject nuevoProyectil = Instantiate(iceSpikePrefab, transform.position + spawnOffset, transform.rotation);
        Rigidbody2D rb = nuevoProyectil.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(lastDirection < 0 ? -velocityIceSpike : velocityIceSpike, 0f);
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