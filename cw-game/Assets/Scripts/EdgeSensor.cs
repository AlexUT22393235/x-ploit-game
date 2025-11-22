using UnityEngine;

public class EdgeSensor : MonoBehaviour
{
    public bool IsPlatformAhead { get; private set; } = true;
    
    private const string FloorTag = "Floor"; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(FloorTag))
        {
            IsPlatformAhead = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(FloorTag))
        {
            IsPlatformAhead = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(FloorTag))
        {
            IsPlatformAhead = true;
        }
    }
}