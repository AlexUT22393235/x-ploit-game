using UnityEngine;

/// <summary>
/// Script de ejemplo para iniciar el diálogo al comienzo de una escena.
/// </summary>
public class DialogueInitializer : MonoBehaviour
{
    // Define tu conversación directamente en el Inspector
    public DialogueSequence initialConversation; 

    void Start()
    {
        // Asegurarse de que el Manager existe
        // Advertencia CS0618: Reemplazado FindObjectOfType por FindAnyObjectByType
        DialogueManager manager = FindAnyObjectByType<DialogueManager>();

        if (manager != null)
        {
            // VERIFICACIÓN ADICIONAL: Asegura que el GameObject del DialogueManager esté activo, 
            // lo que permite que su método StartDialogue se ejecute correctamente.
            if (!manager.gameObject.activeInHierarchy)
            {
                manager.gameObject.SetActive(true);
            }
            
            // Inicia la conversación que definiste en el Inspector.
            manager.StartDialogue(initialConversation);
        }
        else
        {
            Debug.LogError("No se encontró el DialogueManager en la escena.");
        }
    }
}