using UnityEngine;

public class SnowyWizard : Player
{
    private float velocityFireball = 2f;

    private GameObject fireballPrefab;

    protected override void Start()
    {
        base.Start();
        fireballPrefab = GameObject.Find("Fireball");

        if (fireballPrefab == null)
        {
            Debug.LogError("¡No se encontró el objeto 'Fireball' en la escena!");
        }
    }

    protected override void PlayAttackSound()
    {
        AudioManager.instance.PlayWizzardIgnis();
    }
    private void ThrowFireball()
    {
        Vector3 spawnOffset = new Vector3(.25f * lastDirection, 0f, 0f);
        GameObject nuevoProyectil = Instantiate(fireballPrefab, transform.position + spawnOffset, transform.rotation);
        Rigidbody2D rb = nuevoProyectil.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(lastDirection < 0 ? -velocityFireball : velocityFireball, 0f);
            AudioManager.instance.PlayWizzardFireball();
        }
    }
}