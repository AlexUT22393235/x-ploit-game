using UnityEngine;
using UnityEngine.SceneManagement; // Importante: Necesario para cargar escenas

public class CambioDeEscena : MonoBehaviour
{
    public string BossBattle; 

    // Usamos OnTriggerEnter2D para juegos 2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos que sea el Jugador quien tocó la zona
        if (collision.CompareTag("Player")) 
        {
            SceneManager.LoadScene(BossBattle);
        }
    }
}