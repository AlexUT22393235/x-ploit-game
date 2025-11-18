using UnityEngine;
using UnityEngine.UI;

public class Filllife : MonoBehaviour
{
    public Image fillLife;

    private GameObject snowy;

    private float maxLife;

    private Snowy snowyScript;

    void Start()
    {
        snowy = GameObject.Find("Snowy");

        if (snowy != null)
        {
            snowyScript = snowy.GetComponent<Snowy>();
        }

        if (snowyScript != null)
        {
            maxLife = snowyScript.life;
        }
        else
        {
            enabled = false;
        }

        fillLife.fillAmount = 1f;
    }

    void Update()
    {
        if (snowyScript != null && maxLife > 0)
        {
            fillLife.fillAmount = (float)snowyScript.life / maxLife;
        }
    }
}