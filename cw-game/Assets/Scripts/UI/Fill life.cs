using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Filllife : MonoBehaviour
{
    public Image fillLife;
    private Player player;

    private float maxLife;

    IEnumerator Start()
    {
        // 1. Esperamos un frame al inicio (buena práctica para dejar que otros Awake corran)
        yield return null;

        // 2. Buscamos al Player
        player = FindFirstObjectByType<Player>();

        if (player != null)
        {
            // --- ÉXITO: Player encontrado ---
            maxLife = player.life;
            fillLife.fillAmount = 1f;

            // Opcional: Debug para confirmar
            // Debug.Log("UI Vida: Player encontrado, barra inicializada.");
        }
        else
        {
            // --- FALLO: Player no encontrado aún ---
            // Debug.LogWarning("UI Vida: No encuentro al Player, reintentando en 0.5s...");

            // 3. Esperamos medio segundo
            yield return new WaitForSeconds(0.5f);

            // 4. Nos volvemos a llamar a nosotros mismos para reintentar
            StartCoroutine(Start());
        }
    }

    void Update()
    {
        if (player != null && maxLife > 0)
        {
            fillLife.fillAmount = (float)player.life / maxLife;
        }
    }
}