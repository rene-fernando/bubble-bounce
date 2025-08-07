using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public Image[] hearts;
    public Image[] emptyHearts; 
    public int maxLives = 3;
    private int currentLives;

    void Start()
    {
        currentLives = maxLives;
        UpdateHearts();
    }

    public void TakeDamage()
    {
        currentLives--;
        UpdateHearts();

        if (currentLives <= 0)
        {
            Debug.Log("Game Over");
            Invoke("ResetLevel", 2f); 
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            bool isAlive = i < currentLives;
            hearts[i].enabled = isAlive;
            emptyHearts[i].enabled = !isAlive;
        }
    }

    void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}