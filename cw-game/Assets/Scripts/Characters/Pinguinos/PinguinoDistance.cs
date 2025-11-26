using UnityEngine;

public class PinguinoDistance : Enemy
{
    private GameObject iceSpikePrefab;

    private float velocityIceSpike = 3f;

    protected override void Start()
    {
        base.Start();

        iceSpikePrefab = GameObject.Find("Ice spike");

        if (iceSpikePrefab == null)
        {
            Debug.LogError("¡No se encontró el objeto 'Ice spike' en la escena!");
        }

        life = 30;
        attackRange = 3f;
        pointsOnDefeat = 25;

        edgeSensor = null;

        Vector2 direction = (playerTransform.position - transform.position).normalized;

        if (direction.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    protected override void Update()
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

    protected override void SetAnimationStates(bool walk, bool attack, bool damage)
    {
        animator.SetBool("Attack", attack);
        animator.SetBool("Damage", damage);
    }

    protected override void OnAttack()
    {
        base.OnAttack();

        Vector3 spawnOffset = new Vector3(.12f * lastDirection, 0.1f, 0f);
        GameObject nuevoProyectil = Instantiate(iceSpikePrefab, transform.position + spawnOffset, transform.rotation);
        Rigidbody2D rb = nuevoProyectil.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(lastDirection < 0 ? -velocityIceSpike : velocityIceSpike, 0f);
            AudioManager.instance.PlayPinDisAtack();
        }
    }
}