using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SettingsManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject loadGamePanel;
    public GameObject TutorialPanel;

    public void Start()
    {
        loadGamePanel.SetActive(false);
        TutorialPanel.SetActive(false);
    }

    public void Tutorial()
    {
        TutorialPanel.SetActive(true);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("DemoScene");
    }
    public void OpenSettings()
    {
        SceneManager.LoadScene("Setting Scene");
    }
    public void ShowLoadPanel()
    {
        mainMenuPanel.SetActive(false);
        loadGamePanel.SetActive(true);
    }
    public void LoadGame(int slot)
    {
        string key = $"SaveSlot{slot}";
        if (PlayerPrefs.HasKey(key))
        {
            string levelName = PlayerPrefs.GetString(key);
            Debug.Log("Loaded Save Slot " + slot + ": " + levelName);

            loadGamePanel.SetActive(false);
            SceneManager.LoadScene("Main Game");

        }
        else
        {
            Debug.LogWarning("No save found in slot " + slot);
        }
    }
    public void GoBackToMainMenu()
    {
        loadGamePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
