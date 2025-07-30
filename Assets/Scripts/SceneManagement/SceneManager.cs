using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class SettingsManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject TutorialPanel;

    public void Start()
    {

        TutorialPanel.SetActive(false);
    }
    public void Tutorial()
    {
        TutorialPanel.SetActive(true);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("MVPLevel");
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
    public void ExitGame()
    {
        Application.Quit();
    }
}
