using UnityEngine;
using UnityEngine.UI;

public class Filllife : MonoBehaviour
{
    public Image fillLife;
    private Player player;

    private float maxLife;

    void Start()
    {
        player = FindFirstObjectByType<Player>();

        if (player != null)
        {
            maxLife = player.life; 
        }
        else
        {
            Debug.LogError("No se encontró el componente 'Player' en la escena. Deshabilitando el script Filllife.");
            enabled = false;
            return;
        }

        fillLife.fillAmount = 1f;
    }

    void Update()
    {
        if (player != null && maxLife > 0)
        {
            fillLife.fillAmount = (float)player.life / maxLife;
        }
    }
}