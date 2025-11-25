using UnityEngine;
using UnityEngine.SceneManagement; 
using System; 

/// <summary>
/// Gestiona la inicialización del diálogo al comienzo de la escena (Intro o Final).
/// Lee el rol guardado para inyectar el sprite de personaje (Caballero/Mago) correcto.
/// </summary>
public class DialogueInitializer : MonoBehaviour
{
    // Secuencia de diálogo que se rellena en el Inspector
    [Tooltip("La secuencia de conversación a reproducir.")]
    public DialogueSequence initialConversation; 

    [Header("Sprites del Protagonista")]
    [Tooltip("Sprite de Snowy (Foca) en el rol de Caballero.")]
    public Sprite knightSprite; 
    [Tooltip("Sprite de Snowy (Foca) en el rol de Mago.")]
    public Sprite mageSprite;
    
    // Constantes de configuración. Deben coincidir con MainMenu.cs
    private const string SelectedRoleKey = "SelectedRole";
    private const int RoleKnight = 0;
    private const int RoleMage = 1;
    // Usamos TowerSceneName solo como ejemplo. En FinalScene, querrías cargar la escena de créditos.
    private const string TowerSceneName = "Tower"; 


    void Start()
    {
        // 1. Suscribirse al evento para saber cuándo el diálogo ha terminado y debemos cargar el nivel.
        DialogueManager.OnDialogueEnd += HandleDialogueEnd;

        // 2. Obtener el sprite del protagonista basado en la selección del menú.
        Sprite protagonistSprite = GetProtagonistSprite();

        // 3. Inyectar el sprite correcto en todas las líneas de diálogo de "Snowy".
        InjectProtagonistSprite(protagonistSprite);

        // 4. Iniciar el sistema de diálogo.
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
    
    /// <summary>
    /// Itera sobre toda la conversación e inyecta el sprite del rol seleccionado
    /// en cada línea donde el Speaker Name es "Snowy".
    /// </summary>
    /// <param name="protagonistSprite">El sprite seleccionado (Caballero o Mago).</param>
    private void InjectProtagonistSprite(Sprite protagonistSprite)
    {
        if (protagonistSprite == null || initialConversation == null) return;

        // Recorre CADA entrada de la conversación.
        for (int i = 0; i < initialConversation.conversation.Length; i++)
        {
            DialogueEntry entry = initialConversation.conversation[i];

            // Verifica si el orador es "Snowy" (ignorando mayúsculas/minúsculas).
            if (entry.speakerName.Equals("Snowy", StringComparison.OrdinalIgnoreCase))
            {
                // Solo si el orador es Snowy, inyectamos el sprite seleccionado.
                initialConversation.conversation[i].characterSprite = protagonistSprite;
            }
        }
        Debug.Log("Inyección de sprites de Snowy completada.");
    }

    /// <summary>
    /// Se llama al destruir el objeto para limpiar la suscripción al evento.
    /// </summary>
    private void OnDestroy()
    {
        DialogueManager.OnDialogueEnd -= HandleDialogueEnd;
    }

    /// <summary>
    /// Se ejecuta cuando el DialogueManager ha completado toda la conversación.
    /// (En la escena Intro cargará Tower; en una escena Final, cargaría Créditos).
    /// </summary>
    private void HandleDialogueEnd()
    {
        // Aquí puedes cambiar a la escena de créditos si estás en la escena final.
        Debug.Log("Diálogo finalizado. Cargando escena: " + TowerSceneName);
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
                return knightSprite;
            }
            else if (selectedRole == RoleMage)
            {
                return mageSprite;
            }
        }
        
        Debug.LogWarning("No se encontró un rol seleccionado. Usando sprite de Caballero por defecto.");
        return knightSprite; // Retorna Caballero por defecto.
    }
}