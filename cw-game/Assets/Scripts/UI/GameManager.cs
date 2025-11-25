using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Necesario para usar Coroutines

/// <summary>
/// Gestiona el estado del juego, la interfaz de usuario de muerte y las transiciones de escena.
/// Usa el patrón Singleton.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

    [Header("UI Objects")]
    [Tooltip("Canvas Group del panel de Muerte/Game Over. ¡Alpha debe ser 0 en el Editor!")]
    public CanvasGroup deathCanvasGroup;

    [Header("Configuración de Animación")]
    [Tooltip("Duración en segundos de la animación de fade-in de la pantalla de Game Over.")]
    public float fadeInDuration = 1.5f;

    private void Awake()
    {
        // Inicialización del Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Asegúrate de que el panel de muerte tenga su CanvasGroup deshabilitado y Alpha en 0 al inicio.
        if (deathCanvasGroup != null)
        {
            // Inicialmente debe ser invisible e inactivo para interacción
            deathCanvasGroup.alpha = 0f;
            deathCanvasGroup.interactable = false;
            deathCanvasGroup.blocksRaycasts = false;
        }
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Llamado por el Player cuando su vida llega a cero.
    /// Inicia el efecto de Game Over.
    /// </summary>
    public void PlayerDied()
    {
        Debug.Log("Game Over. Iniciando animación de fade-in.");
        
        // Detener el tiempo inmediatamente para pausar la acción del juego
        Time.timeScale = 0f;
        
        // Iniciar la Coroutine de desvanecimiento
        if (deathCanvasGroup != null)
        {
            StartCoroutine(FadeInDeathScreen());
        }
    }
    
    // Coroutine para animar la opacidad del Game Over
    private IEnumerator FadeInDeathScreen()
    {
        float timer = 0f;
        
        // Asegura que el CanvasGroup bloquee raycasts e interacciones
        deathCanvasGroup.blocksRaycasts = true;

        while (timer < fadeInDuration)
        {
            timer += Time.unscaledDeltaTime; // Usamos unscaledDeltaTime porque Time.timeScale = 0f
            
            // Calcula la nueva opacidad suavemente (lerp)
            deathCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            
            yield return null; // Espera al siguiente frame
        }

        // Asegura que el alpha final sea 1 y que sea interactuable
        deathCanvasGroup.alpha = 1f;
        deathCanvasGroup.interactable = true;
    }


    /// <summary>
    /// Reinicia la escena actual. Asignar al botón "Reiniciar".
    /// </summary>
    public void RestartGame()
    {
        // Asegura que el tiempo esté reanudado y el panel desactivado antes de cargar
        Time.timeScale = 1f;
        if (deathCanvasGroup != null)
        {
            // Desactiva la interacción y opacidad antes de reiniciar
            deathCanvasGroup.alpha = 0f;
            deathCanvasGroup.interactable = false;
            deathCanvasGroup.blocksRaycasts = false;
        }
        
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    /// <summary>
    /// Carga la escena del Menú Principal. Asignar al botón "Menú Principal".
    /// </summary>
    public void LoadMainMenu()
    {
        // Asegura que el tiempo esté reanudado y el panel desactivado antes de cargar
        Time.timeScale = 1f;
        if (deathCanvasGroup != null)
        {
            // Desactiva la interacción y opacidad antes de la transición
            deathCanvasGroup.alpha = 0f;
            deathCanvasGroup.interactable = false;
            deathCanvasGroup.blocksRaycasts = false;
        }
        
        SceneManager.LoadScene("MainMenu"); 
    }
}