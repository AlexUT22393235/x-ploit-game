using UnityEngine;
using System.Collections;

public class TimedPlatform : MonoBehaviour
{
    [Header("Tiempos")]
    public float activeTime = 2f;   // Tiempo que es sólida
    public float inactiveTime = 2f; // Tiempo que desaparece
    public float startDelay = 0f;   // Retraso inicial (para desincronizarlas)

    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        StartCoroutine(TogglePlatform());
    }

    IEnumerator TogglePlatform()
    {
        // Espera inicial opcional
        yield return new WaitForSeconds(startDelay);

        while (true) // Bucle infinito
        {
            // --- FASE ACTIVA ---
            sr.enabled = true;   // Se ve
            col.enabled = true;  // Se toca
            yield return new WaitForSeconds(activeTime);

            // --- FASE INACTIVA ---
            sr.enabled = false;  // Invisible
            col.enabled = false; // Intangible
            yield return new WaitForSeconds(inactiveTime);
        }
    }
}