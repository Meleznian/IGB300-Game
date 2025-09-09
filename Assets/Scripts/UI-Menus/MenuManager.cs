using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public GameObject menuPanel;
    public GameObject deathPanel;
    public GameObject detailPanel;
    public GameObject UICanvas;
    public AudioClip openSound;
    public AudioClip closeSound;
    private AudioSource audioSource;

    private bool isOpen = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        menuPanel.SetActive(false);
        deathPanel.SetActive(false);
        detailPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
            Time.timeScale = 0f;
            UICanvas.SetActive(false);
            audioSource.PlayOneShot(openSound);
        }
        else if (!isOpen && closeSound != null)
        {
            Time.timeScale = 1.0f;
            UICanvas.SetActive(true);  
            audioSource.PlayOneShot(closeSound);
        }
    }
    public void ResumeGame()
    {
        if (isOpen)
        {
            ToggleMenu();
        }
    }
    public void OpenSettings()
    {
        SceneManager.LoadScene("Setting Scene", LoadSceneMode.Additive);
        Time.timeScale = 0f;
    }

    public void ReturnHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start Scene");
    }

    public void PlayerDead()
    {
        deathPanel.SetActive(true);
        detailPanel.SetActive(true);
        GetComponent<DeathMessageManager>().ShowDeathPanel();
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//return the name of the current scene
    }
}
