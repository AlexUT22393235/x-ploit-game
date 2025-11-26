using UnityEngine;

public class FireballPrefab : Projectiles
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (!collision.gameObject.CompareTag("ObjectBackground") && !collision.gameObject.CompareTag("Player"))
        {

            onDestroy = true;
            hasCollided = true;

            if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemyHealth = collision.gameObject.GetComponent<Enemy>();
                Boss BossHealth = collision.gameObject.GetComponent<Boss>();

                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageAmount);
                } else if (BossHealth != null)
                {
                    BossHealth.TakeDamage(damageAmount);
                }
            }
        }
    }
}
