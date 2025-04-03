using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject videoPanel;
    public GameObject controlsPanel;
    public GameObject audioPanel;

    void Start()
    {
        ShowPanel(videoPanel);
    }

    public void ShowPanel(GameObject panel)
    {

        audioPanel.SetActive(false);
        videoPanel.SetActive(false);
        controlsPanel.SetActive(false);

 
        panel.SetActive(true);
    }
}
