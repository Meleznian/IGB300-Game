using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject deathPanel;
    public AudioClip openSound;
    public AudioClip closeSound;
    private AudioSource audioSource;

    private bool isOpen = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        menuPanel.SetActive(false);
        deathPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isOpen = !isOpen;
        menuPanel.SetActive(isOpen);

        if (isOpen && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }
        else if (!isOpen && closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }
    }
    public void OpenSettings()
    {
        SceneManager.LoadScene("Setting Scene", LoadSceneMode.Additive);
        Time.timeScale = 0f;
    }

    public void ReturnHome()
    {
        SceneManager.LoadScene("Start Scene");
    }
}
