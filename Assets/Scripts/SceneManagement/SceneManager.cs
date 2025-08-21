using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject TutorialPanel;
    public GameObject ScoreboardPanel;

    private GameObject currentOpenPanel;

    public void Start()
    {
        TutorialPanel.SetActive(false);
        ScoreboardPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void Tutorial()
    {
        CloseCurrentPanel();
        TutorialPanel.SetActive(true);
        currentOpenPanel = TutorialPanel;
    }

    public void Scoreboard()
    {
        CloseCurrentPanel();
        ScoreboardPanel.SetActive(true);
        currentOpenPanel = ScoreboardPanel;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("RunnerDemo");
    }

    public void OpenSettings()
    {
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Setting Scene");
    }

    public void GoBackToMainMenu()
    {
        mainMenuPanel.SetActive(true);
    }

    public void CloseCurrentPanel()
    {
        if (currentOpenPanel != null)
        {
            currentOpenPanel.SetActive(false);
            currentOpenPanel = null;
        }
        mainMenuPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
