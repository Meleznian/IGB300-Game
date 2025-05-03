using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public string startSceneName = "Start Scene";
    public void LoadStartScene()
    {
        SceneManager.LoadScene(startSceneName);
    }
}
