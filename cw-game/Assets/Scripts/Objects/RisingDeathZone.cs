using UnityEngine;
using UnityEngine.SceneManagement;

public class RisingDeathZone : MonoBehaviour
{
    [Header("Velocidad y Progresión")]
    public float currentSpeed = 0.5f;       // Velocidad inicial (más lenta)
    public float speedIncreaseAmount = 0.2f; // Cuánto aumenta la velocidad
    public float increaseInterval = 60f;    // Cada cuánto tiempo aumenta (60 seg = 1 min)

    private float timer;

    void Update()
    {
        // 1. Mover el agua hacia arriba constantemente
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);

        // 2. Contador de tiempo para aumentar la dificultad
        timer += Time.deltaTime;

        if (timer >= increaseInterval)
        {
            IncreaseSpeed();
        }
    }

    void IncreaseSpeed()
    {
        currentSpeed += speedIncreaseAmount;
        timer = 0f; // Reiniciar el contador
        Debug.Log("⚠️ ¡El nivel del agua sube más rápido! Nueva velocidad: " + currentSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Te alcanzó el agua!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}