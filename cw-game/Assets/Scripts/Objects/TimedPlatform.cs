using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    [Header("Configuración de Tiempo")]
    public float fallDelay = 1f;    // Tiempo que tarda en caer después de pisarla
    public float respawnTime = 3f;  // Tiempo que tarda en volver a aparecer

    private SpriteRenderer sr;
    private BoxCollider2D col;
    private bool isFalling = false; // Para evitar que se active dos veces

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>(); // Asegúrate de usar el collider correcto (Box o Polygon)
    }

    // Detectamos cuando el jugador pisa la plataforma
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificamos que sea el Jugador y que la plataforma no esté cayendo ya
        // IMPORTANTE: Asegúrate de que tu jugador tenga el Tag "Player"
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            // Verificamos que el jugador esté ARRIBA (opcional, para que no se caiga si le pegas con la cabeza)
            if (collision.relativeVelocity.y <= 0f) 
            {
                StartCoroutine(FallAndRespawn());
            }
        }
    }

    IEnumerator FallAndRespawn()
    {
        isFalling = true;

        // --- 1. FASE DE ADVERTENCIA (Opcional: Temblor) ---
        // Aquí podrías poner una animación de temblor si quisieras
        yield return new WaitForSeconds(fallDelay);

        // --- 2. FASE DE CAÍDA (Desaparecer) ---
        sr.enabled = false;  // Se vuelve invisible
        col.enabled = false; // Se vuelve intangible (el jugador cae)

        // --- 3. FASE DE RESPAWN (Esperar para volver) ---
        yield return new WaitForSeconds(respawnTime);

        // --- 4. REAPARECER ---
        sr.enabled = true;
        col.enabled = true;
        isFalling = false; // Lista para usarse de nuevo
    }
}