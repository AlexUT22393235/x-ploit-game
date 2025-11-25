using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // Asegúrate de que estás usando TextMeshPro (Recomendado para mejor texto)

/// <summary>
/// Gestiona la visualización de diálogos, el efecto de máquina de escribir y el avance de la conversación.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [Header("UI Components")]
    [Tooltip("Panel que contiene todos los elementos de diálogo. Se oculta y muestra.")]
    public GameObject dialogueBox;
    
    [Tooltip("Componente de texto para mostrar el nombre del hablante.")]
    public TextMeshProUGUI nameText; // Usar TextMeshPro
    
    [Tooltip("Componente de texto para mostrar el diálogo (con animación).")]
    public TextMeshProUGUI dialogueText; // Usar TextMeshPro
    
    [Tooltip("Componente Image para el sprite del personaje en la izquierda.")]
    public Image leftCharacterSprite;
    
    [Tooltip("Componente Image para el sprite del personaje en la derecha.")]
    public Image rightCharacterSprite;

    [Header("Configuración de Velocidad")]
    [Tooltip("Segundos de espera entre la impresión de cada carácter.")]
    public float typingSpeed = 0.05f;

    // Estado interno del diálogo
    private DialogueSequence currentDialogue;
    private int currentEntryIndex = 0;
    private bool isTyping = false;
    private bool dialogueActive = false;

    // Inicialización del sistema
    private void Start()
    {
        // Ocultar la caja de diálogo al inicio
        dialogueBox.SetActive(false);
        // Asegurarse de que los sprites están ocultos/atenuados al inicio
        leftCharacterSprite.gameObject.SetActive(false);
        rightCharacterSprite.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Solo procesar la entrada si el diálogo está activo
        if (dialogueActive)
        {
            // Detectar clic del ratón (0) o cualquier tecla (ej: barra espaciadora o Enter)
            if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                HandleInput();
            }
        }
    }

    /// <summary>
    /// Inicia una nueva secuencia de diálogo.
    /// Esta función debe ser llamada desde otro script (ej: un 'LevelInitializer').
    /// </summary>
    /// <param name="dialogue">La secuencia de diálogo a mostrar.</param>
    public void StartDialogue(DialogueSequence dialogue)
    {
        if (dialogue.conversation.Length == 0) return;

        currentDialogue = dialogue;
        currentEntryIndex = 0;
        dialogueActive = true;
        dialogueBox.SetActive(true);
        
        // Pausar el juego si el diálogo es cinemático
        Time.timeScale = 0f; 

        DisplayNextEntry();
    }

    /// <summary>
    /// Maneja la entrada del usuario (clic/tecla) para avanzar el diálogo o saltar el efecto de tipeo.
    /// </summary>
    private void HandleInput()
    {
        if (isTyping)
        {
            // Si se está escribiendo, saltar inmediatamente al texto completo
            StopAllCoroutines();
            dialogueText.text = currentDialogue.conversation[currentEntryIndex].dialogueText;
            isTyping = false;
        }
        else
        {
            // Si ya terminó de escribir, pasar a la siguiente línea de diálogo
            currentEntryIndex++;
            DisplayNextEntry();
        }
    }

    /// <summary>
    /// Muestra la siguiente entrada de diálogo o termina la conversación.
    /// </summary>
    private void DisplayNextEntry()
    {
        if (currentEntryIndex >= currentDialogue.conversation.Length)
        {
            // El diálogo ha terminado
            EndDialogue();
            return;
        }

        DialogueEntry entry = currentDialogue.conversation[currentEntryIndex];

        // 1. Actualizar Nombre y Texto
        nameText.text = entry.speakerName;
        // Reiniciar el coroutine para el nuevo texto
        StopAllCoroutines();
        StartCoroutine(TypeSentence(entry.dialogueText));

        // 2. Actualizar Sprites y Posición
        UpdateCharacterSprites(entry.characterSprite, entry.position);
    }

    /// <summary>
    /// Actualiza qué sprite se muestra y en qué lado (izquierda o derecha), y aplica atenuación.
    /// </summary>
    private void UpdateCharacterSprites(Sprite newSprite, DialogueEntry.CharacterPosition position)
    {
        // Ocultar y atenuar todos los sprites
        leftCharacterSprite.gameObject.SetActive(false);
        rightCharacterSprite.gameObject.SetActive(false);
        leftCharacterSprite.color = new Color(0.5f, 0.5f, 0.5f, 1f); // Atenuar
        rightCharacterSprite.color = new Color(0.5f, 0.5f, 0.5f, 1f); // Atenuar

        if (newSprite != null)
        {
            if (position == DialogueEntry.CharacterPosition.Left)
            {
                leftCharacterSprite.gameObject.SetActive(true);
                leftCharacterSprite.sprite = newSprite;
                leftCharacterSprite.color = Color.white; // Resaltar
            }
            else // Right
            {
                rightCharacterSprite.gameObject.SetActive(true);
                rightCharacterSprite.sprite = newSprite;
                rightCharacterSprite.color = Color.white; // Resaltar
            }
        }
    }

    /// <summary>
    /// Simula el efecto de escritura (máquina de escribir) carácter por carácter.
    /// Usa WaitForSecondsRealtime para ignorar Time.timeScale = 0f.
    /// </summary>
    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = ""; 

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed); 
        }

        isTyping = false;
    }

    /// <summary>
    /// Termina el diálogo y oculta la UI.
    /// </summary>
    private void EndDialogue()
    {
        dialogueBox.SetActive(false);
        dialogueActive = false;
        
        // Reanudar el juego
        Time.timeScale = 1f; 

        Debug.Log("Fin del Diálogo. Juego reanudado.");
    }
}