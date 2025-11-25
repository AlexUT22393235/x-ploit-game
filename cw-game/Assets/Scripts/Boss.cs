using UnityEngine;

public class Boss : MonoBehaviour
{
    protected float speed = 3f;
    private float patrolDistance = 2f;
    protected int damage = 20;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected Vector2 startPosition;
    protected Player playerComponent;
    protected Transform playerTransform;

    protected float currentDirection = 1f;
    protected float damageCooldown = 3f;
    protected float lastDamageTime = 0f;

    public int life = 150;
    private int pointsOnDefeat = 100;

    private bool die = false;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        FindPlayer();

        startPosition = transform.position;
    }

    protected virtual void Update()
    {
        float currentX = transform.position.x;

        if (currentDirection > 0 && currentX >= startPosition.x + patrolDistance)
        {
            currentDirection = -1f;
        }
        else if (currentDirection < 0 && currentX <= startPosition.x - patrolDistance)
        {
            currentDirection = 1f;
        }

        FlipSprite(currentDirection);
    }

    protected virtual void FixedUpdate()
    {
        Vector2 direction = new Vector2(currentDirection, 0);
        Vector2 movement = new Vector2(direction.x * speed, rb.linearVelocity.y);

        rb.linearVelocity = movement;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerComponent.TakeDamage(damage);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
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

    private void FindPlayer()
    {
        playerComponent = FindFirstObjectByType<Player>();

        if (playerComponent != null)
        {
            playerTransform = playerComponent.transform;
        }
        else
        {
            Debug.LogError("Enemigo: No se encontró ningún objeto con el componente Player en la escena.");
        }
    }

    protected void HandlePatrolMovement()
    {
        float currentX = transform.position.x;

        if (currentDirection > 0 && currentX >= startPosition.x + patrolDistance)
        {
            currentDirection = -1f;
        }
        else if (currentDirection < 0 && currentX <= startPosition.x - patrolDistance)
        {
            currentDirection = 1f;
        }
    }

    protected virtual void FlipSprite(float directionX)
    {
        if (directionX < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (directionX > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public virtual void TakeDamage(int damageAmount)
    {
        life -= damageAmount;
        Debug.Log(gameObject.name + " ha recibido " + damageAmount + " de daño. Vida restante: " + life);

        // onDamage = true;
        // AudioManager.instance.PlayProtaDamage();

        if (life <= 0)
        {
            die = true;
            // AudioManager.instance.PlayProtaDead();
            // animator.SetBool("Die", die);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        if (Score.Instance != null)
        {
            Score.Instance.AddPoints(pointsOnDefeat);
        }
    }
}