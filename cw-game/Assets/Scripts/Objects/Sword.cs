using UnityEngine;

public class Sword : MonoBehaviour
{
    private int damageAmount = 15;

    public void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Sword collided with (Trigger): " + other.gameObject.name);

        if (other.gameObject.CompareTag("Enemy"))
        {
            // Debug.Log("Sword hit an enemy!");

            Enemy enemyHealth = other.gameObject.GetComponent<Enemy>();
            Boss BossHealth = other.gameObject.GetComponent<Boss>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }
            else if (BossHealth != null)
            {
                BossHealth.TakeDamage(damageAmount);
            }
        }
    }
}