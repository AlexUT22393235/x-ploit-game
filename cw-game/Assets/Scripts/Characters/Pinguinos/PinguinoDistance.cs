using UnityEngine;

public class PinguinoDistance : Enemy
{
    public GameObject iceSpikePrefab;

    private float velocityIceSpike = 3f;

    public override void Start()
    {
        base.Start();

        life = 30;
        attackRange = 3f;
    }

    public override void Update()
    {
        if (playerComponent == null)
        {
            SetAnimationStates(false, false, false);
            return;
        }
        
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        if (direction.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                lastDirection = -1;
            }
            
            else if (direction.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                lastDirection = 1;
            }

        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            onAttack = true;
        }
        else
        {
            onAttack = false;
        }

        SetAnimationStates(onWalk, onAttack, onDamage);
    }

    public override void SetAnimationStates(bool walk, bool attack, bool damage)
    {
        animator.SetBool("Attack", attack);
        animator.SetBool("Damage", damage);
    }

    public override void OnAttack()
    {
        base.OnAttack();

        Vector3 spawnOffset = new Vector3(.12f * lastDirection, 0.1f, 0f);
        GameObject nuevoProyectil = Instantiate(iceSpikePrefab, transform.position + spawnOffset, transform.rotation);
        Rigidbody2D rb = nuevoProyectil.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(lastDirection < 0 ? -velocityIceSpike : velocityIceSpike, 0f);
        }
    }
}