using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EdgeSensor edgeSensor;
    protected Player playerComponent;
    protected Transform player;
    protected Rigidbody2D rb;
    protected Vector2 movement;
    protected Animator animator;

    protected float detectionRange = 3f;
    protected float speed = .75f;
    protected int damage = 5;
    protected int lastDirection = 1;
    protected float damageCooldown = .5f;
    protected float lastDamageTime = 0f;
    protected float attackDelay = 1.5f;

    protected float nextAttackTime;
    protected int life;
    protected float attackRange;

    protected bool onWalk = false;
    protected bool onAttack = false;
    protected bool onDamage = false;
    protected bool die = false;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        FindPlayer();
        nextAttackTime = Time.time;
    }

    public virtual void Update()
    {
        if (player == null)
        {
            return;
        }

        bool platformAhead = edgeSensor.IsPlatformAhead;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 direction = (player.position - transform.position).normalized;

        if (distanceToPlayer <= detectionRange)
        {
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

            if (distanceToPlayer <= attackRange)
            {
                onWalk = false;
                movement = Vector2.zero;
            }

            else if (!onAttack)
            {
                onWalk = true;
                movement = new Vector2(direction.x, 0);
            }

            if (!platformAhead)
            {
                onWalk = false;
                movement = Vector2.zero;
            }
        }

        else
        {
            movement = Vector2.zero;
            onWalk = false;
        }

        SetAnimationStates(onWalk, onAttack, onDamage);
    }

    public virtual void FixedUpdate()
    {
        if (onWalk)
        {
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerComponent.TakeDamage(damage);
        }
    }

    public virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                playerComponent.TakeDamage(damage);

                lastDamageTime = Time.time;
            }
        }
    }

    public virtual void SetAnimationStates(bool walk, bool attack, bool damage)
    {
        animator.SetBool("Walk", walk);
        animator.SetBool("Attack", attack);
        animator.SetBool("Damage", damage);
    }

    private void FindPlayer()
    {
        playerComponent = FindFirstObjectByType<Player>();

        if (playerComponent != null)
        {
            player = playerComponent.transform;
        }
        else
        {
            Debug.LogError("Enemigo: No se encontró ningún objeto con el componente Player en la escena.");
        }
    }

    public virtual void TakeDamage(int damageAmount)
    {
        Debug.Log(gameObject.name + " ha recibido " + damageAmount + " de daño. Vida restante: " + life);
        life -= damageAmount;

        onDamage = true;

        if (life <= 0)
        {
            die = true;
            animator.SetBool("Die", die);
        }
    }

    public virtual void OnAttack()
    {
        // Debug.Log("Realizando ataque.");
        nextAttackTime = Time.time + attackDelay;
    }

    protected void DisableAttack()
    {
        onAttack = false;
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
