using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject tutorialPanel;

    void Start()
    {
        mainMenuPanel.SetActive(true);
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f; // Make sure time is running!
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); // ← Match your game scene name!
    }

    public void OpenTutorial()
    {
        mainMenuPanel.SetActive(false);
        tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}