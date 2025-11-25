using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Asumiendo que usas el nuevo sistema de Input

public class PauseManager : MonoBehaviour
{
    // Usamos el patrón Singleton para asegurar que solo haya una instancia y sea accesible
    public static PauseManager Instance { get; private set; }

    [Tooltip("Panel que contiene toda la interfaz del menú de pausa.")]
    public GameObject pauseMenuUI;
    
    // Hacemos el estado estático para que se pueda consultar desde cualquier lugar
    public static bool IsPaused { get; private set; } = false;

    // Referencia al PlayerInput para desactivar el control del jugador al pausar.
    private PlayerInput playerInput; 

    void Awake()
    {
        // Implementación del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destruir duplicados
            return;
        }
        Instance = this;
        
        // NO usar DontDestroyOnLoad aquí, ya que puede causar duplicados al recargar escenas.
        // Lo ideal es tener un GameManager que inicialice la UI de pausa solo una vez.
        
        // Inicializar estado
        IsPaused = false;
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }

    void Start()
    {
        // Encontrar el componente PlayerInput en el jugador para controlarlo
        // Asume que el PlayerInput está en el GameObject con la etiqueta "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerInput = playerObject.GetComponent<PlayerInput>();
        }
    }

    void Update()
    {
        // Detectar la pulsación de Escape (usando el antiguo sistema de Input para compatibilidad con tu script original)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    /// <summary>
    /// Cambia entre Pausa y Reanudar, asegurando que no estemos en el menú principal.
    /// </summary>
    public void TogglePause()
    {
        // Ojo: Si usas "MainMenu" o el GameManager está intentando controlar el tiempo aquí, puede haber conflicto.
        if (SceneManager.GetActiveScene().name == "MainMenu")
            return;

        if (IsPaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
        
        // Reactivar el input del jugador
        if (playerInput != null) playerInput.enabled = true;
    }

    public void Pause()
    {
        // Si el juego ya está en pausa o la pantalla de muerte está activa, no pausar de nuevo
        if (IsPaused) return; 
        
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;

        // Desactivar el input del jugador
        if (playerInput != null) playerInput.enabled = false;
    }

    public void QuitGame()
    {
        // Antes de cargar la escena, el juego debe estar "despausado"
        Time.timeScale = 1f;
        IsPaused = false;
        
        // Cargar el menú. (Ya no necesitamos llamar a Resume() después de cargar la escena)
        SceneManager.LoadScene("MainMenu");
    }
}