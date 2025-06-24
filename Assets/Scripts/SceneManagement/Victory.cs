using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    public void LoadNextLevel()
    {
        SceneManager.LoadScene("Credits");
        //UnityEngine.SceneManagement.SceneManager:LoadScene()
    }
}
