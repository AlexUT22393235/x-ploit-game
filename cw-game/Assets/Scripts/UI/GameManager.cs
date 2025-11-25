using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // NECESARIO para usar el componente Image
using System.Collections; 

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
    
    [Tooltip("Componente Image del fondo de la pantalla de Game Over para animar el color.")]
    public Image backgroundPanelImage; // ¡NUEVO CAMPO!

    [Header("Configuración de Animación")]
    [Tooltip("Duración en segundos de la animación de fade-in de la pantalla de Game Over.")]
    public float fadeInDuration = 1.5f;
    
    // Colores para la animación de fondo
    private readonly Color startFadeColor = new Color(0.5f, 0f, 0f, 1f); // Rojo Oscuro (R:128, G:0, B:0)
    private readonly Color endFadeColor = Color.black; // Negro Puro

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
        
        // Asegura que el color inicial del fondo sea el color final (negro) antes de que empiece la animación
        if (backgroundPanelImage != null)
        {
            backgroundPanelImage.color = endFadeColor;
        }
        
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Llamado por el Player cuando su vida llega a cero.
    /// Inicia el efecto de Game Over.
    /// </summary>
    public void PlayerDied()
    {
        Debug.Log("Game Over. Iniciando animación de fade-in y color.");
        
        // Detener el tiempo inmediatamente para pausar la acción del juego
        Time.timeScale = 0f;
        
        // Iniciar la Coroutine de desvanecimiento
        if (deathCanvasGroup != null && backgroundPanelImage != null)
        {
            StartCoroutine(FadeInDeathScreen());
        }
        else
        {
            Debug.LogError("GameManager: deathCanvasGroup o backgroundPanelImage no están asignados en el Inspector.");
        }
    }
    
    // Coroutine para animar la opacidad y el color del Game Over
    private IEnumerator FadeInDeathScreen()
    {
        float timer = 0f;
        
        // El CanvasGroup bloquea raycasts e interacciones
        deathCanvasGroup.blocksRaycasts = true;

        // Establece el color inicial de la imagen (Rojo Oscuro) antes de empezar el fade
        // Nota: El alpha de la imagen lo dejamos en 1.0, el CanvasGroup controla el alpha general.
        backgroundPanelImage.color = startFadeColor; 

        while (timer < fadeInDuration)
        {
            timer += Time.unscaledDeltaTime; // Usamos unscaledDeltaTime porque Time.timeScale = 0f
            float t = timer / fadeInDuration;
            
            // 1. Animar la Opacidad (Canvas Group Alpha: 0 -> 1)
            deathCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            
            // 2. Animar el Color (Image Color: Rojo Oscuro -> Negro Puro)
            backgroundPanelImage.color = Color.Lerp(startFadeColor, endFadeColor, t);
            
            yield return null; // Espera al siguiente frame
        }

        // Asegura el estado final
        deathCanvasGroup.alpha = 1f;
        deathCanvasGroup.interactable = true;
        backgroundPanelImage.color = endFadeColor;
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
            // Restablece la opacidad y la interacción
            deathCanvasGroup.alpha = 0f;
            deathCanvasGroup.interactable = false;
            deathCanvasGroup.blocksRaycasts = false;
        }
        if (backgroundPanelImage != null)
        {
            // Restablece el color final (Negro)
            backgroundPanelImage.color = endFadeColor;
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
            // Restablece la opacidad y la interacción
            deathCanvasGroup.alpha = 0f;
            deathCanvasGroup.interactable = false;
            deathCanvasGroup.blocksRaycasts = false;
        }
        if (backgroundPanelImage != null)
        {
            // Restablece el color final (Negro)
            backgroundPanelImage.color = endFadeColor;
        }
        
        SceneManager.LoadScene("MainMenu"); 
    }
}