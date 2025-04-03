using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Main Game");
    }
    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
