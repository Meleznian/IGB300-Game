using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI YouDiedtxt;
    public TextMeshProUGUI killCountText;

    bool gameHasEnded = false;
    public float restartDelay = 0.5f;
    private int _killCount = 0;

    void Update()
    {
        killCountText.text = DisplayKillCount().ToString();
    }

    public void KillCount()
    {
        _killCount = _killCount + 1;
        Debug.Log("killCount " + _killCount);

    }

    public int DisplayKillCount()
    {
        return _killCount;
    }

    public void EndGame()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("GAME OVER!!!");
            YouDiedtxt.gameObject.SetActive(true);//for gameObject
            //YouDiedtxt.enabled = true;//for component only 
            Invoke("Restart", restartDelay);
            //Restart();
        }
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//return the name of the current scene
    }

}
