using UnityEngine;

public class IceSpikePrefab : Projectiles
{
    // public override void Start()
    // {
    //     base.Start();
    // }

    // public override void Update()
    // {
    //     base.Update();
    // }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        
        if (!collision.gameObject.CompareTag("ObjectBackground") && !collision.gameObject.CompareTag("Enemy"))
        {

            onDestroy = true;
            hasCollided = true;

            if (collision.gameObject.CompareTag("Player"))
            {
                Player playerHealth = collision.gameObject.GetComponent<Player>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                }
            }
        }
    }
}
