using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public Image[] hearts; // Assign 3 heart images in Inspector
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
            Invoke("ResetLevel", 2f); // Wait 2 seconds before resetting
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentLives;
        }
    }

    void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}