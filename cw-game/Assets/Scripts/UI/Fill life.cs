using UnityEngine;
using UnityEngine.UI;

public class Filllife : MonoBehaviour
{
    public Image fillLife;

    private GameObject snowy;

    private float maxLife;

    private Player player;

    void Start()
    {
        player = FindFirstObjectByType<Player>();

        if (player != null)
        {
            maxLife = player.life;
        }
        else
        {
            enabled = false;
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