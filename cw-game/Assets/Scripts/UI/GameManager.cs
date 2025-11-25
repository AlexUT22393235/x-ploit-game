using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Objects")]
    [Tooltip("Panel que contiene la pantalla de Muerte/Game Over. ¡Debe estar DESACTIVADO en el Editor!")]
    public GameObject deathScreenPanel;

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
        if (deathScreenPanel != null)
        {
            deathScreenPanel.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    public void PlayerDied()
    {
        Debug.Log("Game Over. Mostrando pantalla de muerte.");
        
        Time.timeScale = 0f;
        
        // Mostrar la pantalla de muerte
        // if (deathScreenPanel != null)
        // {
            deathScreenPanel.SetActive(true);
        // }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        SceneManager.LoadScene(currentSceneName);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }
}