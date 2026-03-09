using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject tutorialPanel;

    [Header("Pause Button")]
    public GameObject pauseButton;        // The on-screen pause button

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
        tutorialPanel.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void OnPauseButtonClicked()
    {
        Pause();
    }

    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);   // Hide pause button while paused
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        tutorialPanel.SetActive(false);
        pauseButton.SetActive(true);    // Show pause button again
        Time.timeScale = 1f;
    }

    public void OpenTutorial()
    {
        pausePanel.SetActive(false);
        tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}