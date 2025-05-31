using UnityEngine;
using TMPro;

public class DeathMessageManager : MonoBehaviour
{
    [Header("Death Messages")]
    [TextArea(2, 4)]
    public string[] deathMessages;

    public GameObject deathPanel;    
    public TMP_Text messageText;

    void Start()
    {
        deathPanel.SetActive(false);   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ShowDeathPanel();
        }
    }

    public void ShowDeathPanel()
    {
        deathPanel.SetActive(true);
        ShowRandomDeathMessage();
    }

    void ShowRandomDeathMessage()
    {
        if (deathMessages.Length == 0)
        {
            messageText.text = "You Died.";
            return;
        }

        int randomIndex = Random.Range(0, deathMessages.Length);
        messageText.text = deathMessages[randomIndex];
    }
}
