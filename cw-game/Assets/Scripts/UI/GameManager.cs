using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Objects - Muerte")]
    [Tooltip("Canvas Group del panel de Muerte/Game Over. ¡Alpha debe ser 0 en el Editor!")]
    public CanvasGroup deathCanvasGroup;
    
    [Tooltip("Componente Image del fondo de la pantalla de Game Over para animar el color.")]
    public Image backgroundPanelImage; 

    [Header("UI Objects - Victoria")]
    [Tooltip("Canvas Group del panel de Victoria/Win Screen. ¡Alpha debe ser 0 en el Editor!")]
    public CanvasGroup winCanvasGroup;
    
    [Tooltip("Componente Image del fondo de la pantalla de Victoria para animar el color.")]
    public Image winBackgroundPanelImage;

    [Header("General UI")]
    [Tooltip("Objeto de UI principal del juego (e.g., vida, puntuación). Se desactiva al morir.")]
    public GameObject ui;

    [Header("Configuración de Animación")]
    [Tooltip("Duración en segundos de la animación de fade-in de la pantalla de Game Over.")]
    public float fadeInDuration = 1.5f;
    
    private readonly Color startFadeColor = new Color(0.5f, 0f, 0f, 1f);
    private readonly Color endFadeColor = Color.black;
    
    private readonly Color winStartFadeColor = new Color(0f, 0.5f, 0.5f, 1f);
    private readonly Color winEndFadeColor = Color.black;

    private void Awake()
    {
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

        if (ui != null)
        {
            ui.SetActive(true);
        }
        
        Time.timeScale = 1f;
    }

    public void PlayerDied()
    {
        Debug.Log("Game Over. Iniciando animación de fade-in y color.");
        
        if (ui != null)
        {
            ui.SetActive(false);
        }

        Time.timeScale = 0f;
        
        if (deathCanvasGroup != null && backgroundPanelImage != null)
        {
            StartCoroutine(FadeInDeathScreen());
        }
        else
        {
            Debug.LogError("GameManager: deathCanvasGroup o backgroundPanelImage no están asignados en el Inspector.");
        }
    }
    
    public void PlayerWon()
    {
        Debug.Log("¡Victoria! Iniciando animación de fade-in y color.");
        
        if (ui != null)
        {
            ui.SetActive(false);
        }

        Time.timeScale = 0f;
        
        if (winCanvasGroup != null && winBackgroundPanelImage != null)
        {
            StartCoroutine(FadeInWinScreen());
        }
        else
        {
            Debug.LogError("GameManager: winCanvasGroup o winBackgroundPanelImage no están asignados en el Inspector.");
        }
    }

    private IEnumerator FadeInDeathScreen()
    {
        float timer = 0f;
        
        deathCanvasGroup.blocksRaycasts = true;

        backgroundPanelImage.color = startFadeColor; 

        while (timer < fadeInDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / fadeInDuration;
            
            deathCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            
            backgroundPanelImage.color = Color.Lerp(startFadeColor, endFadeColor, t);
            
            yield return null;
        }

        deathCanvasGroup.alpha = 1f;
        deathCanvasGroup.interactable = true;
        backgroundPanelImage.color = endFadeColor;
    }
    
    private IEnumerator FadeInWinScreen()
    {
        float timer = 0f;
        
        winCanvasGroup.blocksRaycasts = true;

        winBackgroundPanelImage.color = winStartFadeColor; 

        while (timer < fadeInDuration)
        {
            timer += Time.unscaledDeltaTime; 
            float t = timer / fadeInDuration;
            
            winCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            
            winBackgroundPanelImage.color = Color.Lerp(winStartFadeColor, winEndFadeColor, t);
            
            yield return null; 
        }

        winCanvasGroup.alpha = 1f;
        winCanvasGroup.interactable = true;
        winBackgroundPanelImage.color = winEndFadeColor;
    }


    public void RestartGame()
    {
        ResetStateForSceneLoad();
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    
    public void LoadNextLevel()
    {
        ResetStateForSceneLoad();
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadMainMenu()
    {
        ResetStateForSceneLoad();
        SceneManager.LoadScene("MainMenu"); 
    }
    
    private void ResetStateForSceneLoad()
    {
        Time.timeScale = 1f;

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

        if (ui != null)
        {
            ui.SetActive(true);
        }
    }
}