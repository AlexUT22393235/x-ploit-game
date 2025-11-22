using UnityEngine;

public class Projectiles : MonoBehaviour
{
    protected int damageAmount = 10;

    protected Animator animator;
    protected Player playerComponent;
    protected Transform playerTransform;

    protected bool onDestroy = false;

    protected bool hasCollided = false;


    public virtual void Start()
    {
        FindPlayer();

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        animator = GetComponent<Animator>();

        if (direction.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public virtual void Update()
    {
        animator.SetBool("Destroy", onDestroy);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("The projectile collided with " + collision.gameObject.name);

        if (hasCollided)
        {
            return;
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

    protected virtual void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}