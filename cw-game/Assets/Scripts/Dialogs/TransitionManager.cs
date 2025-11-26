using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Si usas texto normal, cambia esto por UnityEngine.UI

public class TransitionManager : MonoBehaviour
{
    [Header("Configuración de Escena")]
    public string nombreEscenaBoss;
    
    [Header("Configuración Visual")]
    public CanvasGroup panelNegro; // Arrastra aquí el Panel con el CanvasGroup
    public TextMeshProUGUI textoHistoria; // Arrastra aquí tu objeto de Texto
    
    [Header("Narrativa")]
    [TextArea(3, 10)] // Esto hace la caja de texto más grande en el inspector
    public string mensajeNarrativo; 

    [Header("Tiempos")]
    public float velocidadFade = 1f; // Qué tan rápido se oscurece
    public float tiempoLectura = 3f; // Cuántos segundos esperar para leer

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(SecuenciaTransicion());
        }
    }

    IEnumerator SecuenciaTransicion()
    {
        // 1. Asignar el texto que escribiste en el inspector
        textoHistoria.text = mensajeNarrativo;

        // 2. Fade In (Hacer que aparezca el panel negro y el texto)
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * velocidadFade;
            panelNegro.alpha = alpha;
            yield return null; // Esperar al siguiente frame
        }

        // 3. Esperar tiempo de lectura (con la pantalla en negro y el texto)
        yield return new WaitForSeconds(tiempoLectura);

        // 4. Cargar la escena del Boss
        SceneManager.LoadScene(nombreEscenaBoss);
    }
}