using UnityEngine;
using UnityEngine.SceneManagement;
using System; 

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    [Tooltip("El objeto Canvas que contiene la UI del menú de pausa.")]
    public GameObject pauseMenuUI;

    private bool isPaused = false;
    
    [Tooltip("El nombre EXACTO de la escena del menú principal.")]
    private const string MainMenuSceneName = "MainMenu"; 

    private string[] nonGameScenes = { "MainMenu", "Intro", "FinalScene" };


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isGameScene = IsGameScene(scene.name);

        if (isGameScene)
        {
            Time.timeScale = 1f;
            isPaused = false;
            pauseMenuUI.SetActive(false);
            
            Debug.Log("Cargada escena de juego. Pausa disponible.");
        }
        else
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
            
            Debug.Log($"Cargada escena {scene.name}. Pausa deshabilitada.");
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsGameScene(SceneManager.GetActiveScene().name))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    
    private bool IsGameScene(string sceneName)
    {
        foreach (string name in nonGameScenes)
        {
            if (sceneName.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }
        return true; 
    }

 
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        
        Time.timeScale = 1f;
        
        isPaused = false;
        Debug.Log("Juego reanudado.");
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        
        Time.timeScale = 0f;
        
        isPaused = true;
        Debug.Log("Juego pausado.");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f; 
        isPaused = false;
        
        SceneManager.LoadScene(MainMenuSceneName);
        Debug.Log($"Cargando escena: {MainMenuSceneName}");
    }
}