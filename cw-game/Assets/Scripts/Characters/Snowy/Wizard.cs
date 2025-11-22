using UnityEngine;

public class SnowyWizard : Player
{
    private float velocityFireball = 2f;

    public GameObject fireballPrefab;

    public void ThrowFireball()
    {
        Vector3 spawnOffset = new Vector3(.25f * lastDirection, 0f, 0f);
        GameObject nuevoProyectil = Instantiate(fireballPrefab, transform.position + spawnOffset, transform.rotation);
        Rigidbody2D rb = nuevoProyectil.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(lastDirection < 0 ? -velocityFireball : velocityFireball, 0f);
        }
    }
}