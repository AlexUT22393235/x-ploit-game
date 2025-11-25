using UnityEngine;

[System.Serializable]
public class DialogueEntry
{
    [Tooltip("El nombre del personaje que está hablando.")]
    public string speakerName;

    [Tooltip("El texto completo del diálogo.")]
    [TextArea(3, 10)]
    public string dialogueText;

    [Tooltip("Sprite del personaje (opcional).")]
    public Sprite characterSprite;

    // Define la posición del sprite (izquierda o derecha)
    public enum CharacterPosition { Left, Right }
    [Tooltip("La posición en la que debe aparecer el sprite del personaje.")]
    public CharacterPosition position;
}

[System.Serializable]
public class DialogueSequence
{
    [Tooltip("Una lista ordenada de todas las líneas de diálogo en esta secuencia.")]
    public DialogueEntry[] conversation;
}