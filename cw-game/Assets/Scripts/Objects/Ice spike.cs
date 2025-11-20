using UnityEngine;

public class IceSpikePrefab : MonoBehaviour
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

        if (!collision.gameObject.CompareTag("ObjectBackground") && !collision.gameObject.CompareTag("Enemy"))
        {

            onDestroy = true;
            hasCollided = true;

            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Ice spike hit an enemy!");
                Player playerHealth = collision.gameObject.GetComponent<Player>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                }
            }

            Debug.Log("Ice spike collided with " + collision.gameObject.name);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
