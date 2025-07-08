using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TMP_Text scoreText;

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score updated: " + score);
        scoreText.text = "Ducks: " + score;
    }
}