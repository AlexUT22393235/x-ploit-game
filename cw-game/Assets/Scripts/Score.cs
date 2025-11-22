using UnityEngine;

public class Score : MonoBehaviour
{
    public int currentScore = 0;

    public void Start()
    {
        
    }

    public void Update()
    {
        
    }

    public void AddPoints(int points)
    {
        currentScore += points;
    }
}