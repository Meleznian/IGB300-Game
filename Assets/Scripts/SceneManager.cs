using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class SettingsManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject TutorialPanel;

    public GameObject popupPanel;
    public float popupDuration = 3f;
    public TMP_Text popupText;
    public void Start()
    {
        popupPanel.SetActive(false);
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
    public void ShowLoadPanel()
    {
        ShowPopup("Save/Save game feature coming in a future update — Thank you for your patience!");
    }
    private void ShowPopup(string message)
    {
        StopAllCoroutines();
        popupText.text = message;
        popupPanel.SetActive(true);
        StartCoroutine(HidePopupAfterDelay());
    }
    private IEnumerator HidePopupAfterDelay()
    {
        yield return new WaitForSeconds(popupDuration);
        popupPanel.SetActive(false);
    }
    public void GoBackToMainMenu()
    {
        popupPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
