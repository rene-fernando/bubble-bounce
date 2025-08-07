using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject optionsMenuPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("BubbleBounce");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void OpenOptions()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (optionsMenuPanel != null) optionsMenuPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsMenuPanel != null) optionsMenuPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayGame();
        }
    }
}