using UnityEngine;
using TMPro; 

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText; 

    private int score = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
        else
            Debug.LogWarning("Score Text is not assigned!");
    }
}