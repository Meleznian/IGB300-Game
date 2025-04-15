using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SettingsManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject loadGamePanel;
    public void StartGame()
    {
        SceneManager.LoadScene("Main Game");
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
            string levelName = PlayerPrefs.GetString(key); // e.g., "Level1"
            Debug.Log("Loaded Save Slot " + slot + ": " + levelName);

            // Activate gameplay UI or logic
            loadGamePanel.SetActive(false);
            SceneManager.LoadScene("Main Game");

            // You can also spawn player, set stats, etc.
            // Example: GameManager.Instance.LoadPlayerData(slot);
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
