using UnityEngine;

public class fireballPrefab : MonoBehaviour
{
    private int damageAmount = 10;

    private Animator animator;

    private bool onDestroy = false;

    private bool hasCollided = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool("Destroy", onDestroy);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCollided)
        {
            return;
        }

        if (!collision.gameObject.CompareTag("ObjectBackground") && !collision.gameObject.CompareTag("Player"))
        {

            onDestroy = true;
            hasCollided = true;

            if (collision.gameObject.CompareTag("Enemy"))
            {
                // Debug.Log("Fireball hit an enemy!");
                Enemy enemyHealth = collision.gameObject.GetComponent<Enemy>();

                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageAmount);
                }
            }

            // Debug.Log("Fireball collided with " + collision.gameObject.name);
            Destroy(gameObject, 0.3f);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
