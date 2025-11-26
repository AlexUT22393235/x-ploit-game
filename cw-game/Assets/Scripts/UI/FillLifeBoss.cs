using UnityEngine;
using UnityEngine.UI;

public class FilllifeBoss : MonoBehaviour
{
    public Image fillLife;
    private Boss boss;

    private float maxLife;

    void Start()
    {
        boss = FindFirstObjectByType<Boss>();

        if (boss != null)
        {
            maxLife = boss.life; 
        }
        else
        {
            Debug.LogError("No se encontró el componente 'Boss' en la escena. Deshabilitando el script FilllifeBoss.");
            enabled = false;
            return;
        }

        fillLife.fillAmount = 1f;
    }

    void Update()
    {
        if (boss != null && maxLife > 0)
        {
            fillLife.fillAmount = (float)boss.life / maxLife;
        }
    }
}