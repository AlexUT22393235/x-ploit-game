using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    private Player playerComponent;

    void Start()
    {
        FindPlayer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Colisión detectada con: " + other.name);
        if (other.CompareTag("Player"))
        {
            // Debug.Log("¡Te alcanzó el agua!");
            playerComponent.TakeDamage(100);
            GameManager.Instance?.PlayerDied();
        }
    }

    private void FindPlayer()
    {
        playerComponent = FindFirstObjectByType<Player>();

        if (playerComponent == null)
        {
            Debug.LogError("No se encontró ningún objeto con el componente Player en la escena.");
        }
    }
}