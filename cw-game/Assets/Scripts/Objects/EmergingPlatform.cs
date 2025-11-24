using UnityEngine;

public class EmergingPlatform : MonoBehaviour
{
    public float riseSpeed = 5f;   // Qué tan rápido sale del agua
    public float targetY = 0f;     // A qué altura debe detenerse (se asigna automáticamente)
    
    private bool hasReachedTop = false;

    void Update()
    {
        if (!hasReachedTop)
        {
            // Mover hacia arriba
            transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);

            // Si llegamos o pasamos la altura objetivo...
            if (transform.position.y >= targetY)
            {
                // Corregir posición exacta y detenerse
                transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
                hasReachedTop = true;
                
                // Opcional: Efecto de sonido o partículas de agua aquí
            }
        }
    }

    // Función para configurar la plataforma al nacer
    public void Setup(float finalHeight)
    {
        targetY = finalHeight;
    }
}