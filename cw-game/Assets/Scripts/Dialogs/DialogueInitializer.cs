using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas
using System; // Necesario para el delegado Action

/// <summary>
/// Script de ejemplo para iniciar el diálogo al comienzo de una escena.
/// Lee el rol seleccionado por el jugador para mostrar el sprite correcto.
/// </summary>
public class DialogueInitializer : MonoBehaviour
{
    // Define tu conversación directamente en el Inspector
    public DialogueSequence initialConversation; 

    [Header("Sprites del Protagonista")]
    [Tooltip("Sprite de Snowy (Foca) en el rol de Caballero.")]
    public Sprite knightSprite; 
    [Tooltip("Sprite de Snowy (Foca) en el rol de Mago.")]
    public Sprite mageSprite;
    
    // Clave y valores constantes deben coincidir con MainMenu.cs
    private const string SelectedRoleKey = "SelectedRole";
    private const int RoleKnight = 0;
    private const int RoleMage = 1;
    private const string TowerSceneName = "Tower"; // Nombre de la escena principal del juego


    void Start()
    {
        // Suscribirse al evento de finalización del diálogo para saber cuándo cargar la siguiente escena.
        // NOTA: Asegúrate de que DialogueManager tiene 'public static event Action OnDialogueEnd;'
        DialogueManager.OnDialogueEnd += HandleDialogueEnd;

        // 1. Determinar el rol y obtener el sprite
        Sprite protagonistSprite = GetProtagonistSprite();

        // 2. Inyectar el sprite en la conversación (opcional: línea 1, donde habla Snowy)
        if (protagonistSprite != null && initialConversation != null && initialConversation.conversation.Length > 1)
        {
            // Asumiendo que la línea 1 (índice 1) es donde habla Snowy (el protagonista)
            initialConversation.conversation[1].characterSprite = protagonistSprite;
        }

        // 3. Iniciar el diálogo (logica original)
        DialogueManager manager = FindAnyObjectByType<DialogueManager>();

        if (manager != null)
        {
            if (!manager.gameObject.activeInHierarchy)
            {
                manager.gameObject.SetActive(true);
            }
            
            manager.StartDialogue(initialConversation);
        }
        else
        {
            Debug.LogError("No se encontró el DialogueManager en la escena.");
        }
    }
    
    private void OnDestroy()
    {
        // Desuscribirse del evento para evitar errores cuando este objeto es destruido
        DialogueManager.OnDialogueEnd -= HandleDialogueEnd;
    }

    /// <summary>
    /// Se llama cuando el diálogo termina (disparado por DialogueManager).
    /// Inicia la escena principal del juego.
    /// </summary>
    private void HandleDialogueEnd()
    {
        Debug.Log("Diálogo de introducción finalizado. Cargando escena: " + TowerSceneName);
        // Carga la escena de la torre, reemplazando la escena "Intro" actual.
        SceneManager.LoadScene(TowerSceneName);
    }

    /// <summary>
    /// Lee el rol guardado en PlayerPrefs y retorna el sprite correspondiente.
    /// </summary>
    /// <returns>El sprite de Caballero o Mago.</returns>
    private Sprite GetProtagonistSprite()
    {
        if (PlayerPrefs.HasKey(SelectedRoleKey))
        {
            int selectedRole = PlayerPrefs.GetInt(SelectedRoleKey);
            
            if (selectedRole == RoleKnight)
            {
                Debug.Log("Cargando diálogo para Caballero.");
                return knightSprite;
            }
            else if (selectedRole == RoleMage)
            {
                Debug.Log("Cargando diálogo para Mago.");
                return mageSprite;
            }
        }
        
        Debug.LogWarning("No se encontró un rol seleccionado. Usando sprite de Caballero por defecto.");
        return knightSprite; // Retorna Caballero por defecto si no hay clave.
    }
}