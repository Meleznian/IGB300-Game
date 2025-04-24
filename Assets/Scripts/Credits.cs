using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject MainCanvas;
    public Button closeButton;
    public Button creditButton;

    void Start()
    {
        creditsPanel.SetActive(false);
        closeButton.onClick.AddListener(CloseCredits);
        creditButton.onClick.AddListener(OpenCredits);
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        MainCanvas.SetActive(false);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
        MainCanvas.SetActive(true);
    }
}
