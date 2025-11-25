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

    [Header("UI Objects - Muerte")]
    [Tooltip("Canvas Group del panel de Muerte/Game Over. ¡Alpha debe ser 0 en el Editor!")]
    public CanvasGroup deathCanvasGroup;
    
    [Tooltip("Componente Image del fondo de la pantalla de Game Over para animar el color.")]
    public Image backgroundPanelImage; 

    [Header("UI Objects - Victoria")]
    [Tooltip("Canvas Group del panel de Victoria/Win Screen. ¡Alpha debe ser 0 en el Editor!")]
    public CanvasGroup winCanvasGroup; // ¡NUEVO CAMPO!
    
    [Tooltip("Componente Image del fondo de la pantalla de Victoria para animar el color.")]
    public Image winBackgroundPanelImage; // ¡NUEVO CAMPO!


    [Header("Configuración de Animación")]
    [Tooltip("Duración en segundos de la animación de fade-in de la pantalla de Game Over.")]
    public float fadeInDuration = 1.5f;
    
    // Colores para la animación de fondo
    private readonly Color startFadeColor = new Color(0.5f, 0f, 0f, 1f); // Game Over: Rojo Oscuro (R:128, G:0, B:0)
    private readonly Color endFadeColor = Color.black; // Game Over: Negro Puro
    
    private readonly Color winStartFadeColor = new Color(0f, 0.5f, 0.5f, 1f); // Victoria: Azul/Verde Suave
    private readonly Color winEndFadeColor = Color.black; // Victoria: Negro Puro

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
        // Inicialización de la pantalla de Muerte
        if (deathCanvasGroup != null)
        {
            deathCanvasGroup.alpha = 0f;
            deathCanvasGroup.interactable = false;
            deathCanvasGroup.blocksRaycasts = false;
        }
        if (backgroundPanelImage != null)
        {
            backgroundPanelImage.color = endFadeColor;
        }
        
        // Inicialización de la pantalla de Victoria
        if (winCanvasGroup != null)
        {
            winCanvasGroup.alpha = 0f;
            winCanvasGroup.interactable = false;
            winCanvasGroup.blocksRaycasts = false;
        }
        if (winBackgroundPanelImage != null)
        {
            winBackgroundPanelImage.color = winEndFadeColor;
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
    
    /// <summary>
    /// Llamado cuando el jugador completa el nivel o cumple la condición de victoria.
    /// Inicia el efecto dAe victoria.
    /// </summary>
    public void PlayerWon() // ¡NUEVA FUNCIÓN!
    {
        Debug.Log("¡Victoria! Iniciando animación de fade-in y color.");
        
        Time.timeScale = 0f; // Pausar el juego
        
        if (winCanvasGroup != null && winBackgroundPanelImage != null)
        {
            StartCoroutine(FadeInWinScreen());
        }
        else
        {
            Debug.LogError("GameManager: winCanvasGroup o winBackgroundPanelImage no están asignados en el Inspector.");
        }
    }

    // Coroutine para animar la opacidad y el color del Game Over
    private IEnumerator FadeInDeathScreen()
    {
        float timer = 0f;
        
        // El CanvasGroup bloquea raycasts e interacciones
        deathCanvasGroup.blocksRaycasts = true;

        // Establece el color inicial de la imagen (Rojo Oscuro) antes de empezar el fade
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
    
    // Coroutine para animar la opacidad y el color de la Pantalla de Victoria
    private IEnumerator FadeInWinScreen() // ¡NUEVA COROUTINE!
    {
        float timer = 0f;
        
        // El CanvasGroup bloquea raycasts e interacciones
        winCanvasGroup.blocksRaycasts = true;

        // Establece el color inicial de la imagen (Azul/Verde) antes de empezar el fade
        winBackgroundPanelImage.color = winStartFadeColor; 

        while (timer < fadeInDuration)
        {
            timer += Time.unscaledDeltaTime; 
            float t = timer / fadeInDuration;
            
            // 1. Animar la Opacidad (Canvas Group Alpha: 0 -> 1)
            winCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            
            // 2. Animar el Color (Image Color: Azul/Verde -> Negro Puro)
            winBackgroundPanelImage.color = Color.Lerp(winStartFadeColor, winEndFadeColor, t);
            
            yield return null; 
        }

        // Asegura el estado final
        winCanvasGroup.alpha = 1f;
        winCanvasGroup.interactable = true;
        winBackgroundPanelImage.color = winEndFadeColor;
    }


    /// <summary>
    /// Reinicia la escena actual. Asignar al botón "Reiniciar".
    /// </summary>
    public void RestartGame()
    {
        ResetStateForSceneLoad();
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    
    /// <summary>
    /// Carga la siguiente escena (asumiendo que es el siguiente nivel). Asignar al botón "Siguiente Nivel".
    /// </summary>
    public void LoadNextLevel() // ¡NUEVA FUNCIÓN!
    {
        ResetStateForSceneLoad();
        
        // Carga la siguiente escena en el orden de compilación
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    /// <summary>
    /// Carga la escena del Menú Principal. Asignar al botón "Menú Principal".
    /// </summary>
    public void LoadMainMenu()
    {
        ResetStateForSceneLoad();
        SceneManager.LoadScene("MainMenu"); 
    }
    
    // Función de utilidad para restablecer el estado antes de cargar una escena.
    private void ResetStateForSceneLoad()
    {
        Time.timeScale = 1f; // Asegura que el juego esté reanudado

        // Desactivar UI de Muerte
        if (deathCanvasGroup != null)
        {
            deathCanvasGroup.alpha = 0f;
            deathCanvasGroup.interactable = false;
            deathCanvasGroup.blocksRaycasts = false;
        }
        if (backgroundPanelImage != null)
        {
            backgroundPanelImage.color = endFadeColor;
        }
        
        // Desactivar UI de Victoria
        if (winCanvasGroup != null)
        {
            winCanvasGroup.alpha = 0f;
            winCanvasGroup.interactable = false;
            winCanvasGroup.blocksRaycasts = false;
        }
        if (winBackgroundPanelImage != null)
        {
            winBackgroundPanelImage.color = winEndFadeColor;
        }
    }
}