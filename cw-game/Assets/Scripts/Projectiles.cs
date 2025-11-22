using UnityEngine;

public class Projectiles : MonoBehaviour
{
    protected int damageAmount = 10;

    protected Animator animator;

    protected bool onDestroy = false;

    protected bool hasCollided = false;


    public virtual void Start()
    {
        animator = GetComponent<Animator>();
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

    protected virtual void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}