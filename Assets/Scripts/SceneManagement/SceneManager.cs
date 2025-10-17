using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject menuPanel;
    public GameObject TutorialPanel;
    public GameObject tutorialPopup;

    private GameObject currentOpenPanel;

    public void Start()
    {
        TutorialPanel.SetActive(false);
        mainMenuCanvas.SetActive(true);
        tutorialPopup.SetActive(false);
    }

    public void Tutorial()
    {
        //CloseCurrentPanel();
        //TutorialPanel.SetActive(true);
        //currentOpenPanel = TutorialPanel;
        PlayerPrefs.SetInt("TutorialOpened", 1);
        SceneManager.LoadScene("Tutorial");

    }

    //private void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.Escape))
    //    {
    //        PlayerPrefs.SetInt("TutorialOpened", 0);
    //    }
    //}


    public void StartGame()
    {
        if (PlayerPrefs.GetInt("TutorialOpened") == 0)
        {

            tutorialPopup.SetActive(true);
            menuPanel.SetActive(false);
            PlayerPrefs.SetInt("TutorialOpened", 1);
        }
        else
        {
            SceneManager.LoadScene("RunnerDemo");
        }
    }

    public void OpenSettings()
    {
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Setting Scene");
    }

    public void GoBackToMainMenu()
    {
        mainMenuCanvas.SetActive(true);
    }

    public void CloseCurrentPanel()
    {
        if (currentOpenPanel != null)
        {
            currentOpenPanel.SetActive(false);
            currentOpenPanel = null;
        }
        mainMenuCanvas.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
