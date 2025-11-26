using UnityEngine;

public class Projectiles : MonoBehaviour
{
    protected int damageAmount = 10;

    private Animator animator;
    private Player playerComponent;
    private Transform playerTransform;

    protected bool onDestroy = false;
    protected bool hasCollided = false;

    protected virtual void Start()
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

    protected virtual void Update()
    {
        animator.SetBool("Destroy", onDestroy);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log("The projectile collided with " + collision.gameObject.name);

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
        // else
        // {
        //     // Debug.LogError("Enemigo: No se encontró ningún objeto con el componente Player en la escena.");
        // }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}