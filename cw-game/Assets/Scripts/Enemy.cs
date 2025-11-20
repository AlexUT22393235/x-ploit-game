using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Transform player;
    protected Rigidbody2D rb;
    protected Vector2 movement;

    protected float detectionRange = 3f;
    protected float speed = 3f;
    protected int damage = 5;
    protected int lastDirection = 1;

    protected int life;
    protected float attackRange;

    protected bool onAttack = false;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        FindPlayer();
    }

    public virtual void Update()
    {
        if (player == null)
        {
            return;
        }

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
                movement = Vector2.zero;
            }
            else if (!onAttack)
            {
                movement = new Vector2(direction.x, 0);
            }
        }
        else
        {
            movement = Vector2.zero;
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Player playerHealth = collision.gameObject.GetComponent<Player>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void FindPlayer()
    {
        Player playerComponent = FindFirstObjectByType<Player>();

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
        life -= damageAmount;
        Debug.Log(gameObject.name + " ha recibido " + damageAmount + " de daño. Vida restante: " + life);
    }

    public virtual void OnAttack()
    {
        Debug.Log("Realizando ataque.");
    }
}
