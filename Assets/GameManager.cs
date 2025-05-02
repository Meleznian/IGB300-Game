using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI YouDiedtxt;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI killCountVictoryText;
    public GameObject completeLevelUI;

    bool gameHasEnded = false;
    bool gameHasWon = false;
    public float restartDelay = 0.5f;
    private int _killCount = 0;

    public int KillTarget = 3;


    void Update()
    {
        killCountText.text = DisplayKillCount().ToString();

        if (_killCount >= KillTarget & gameHasWon == false)
        {
            gameHasWon = true;
            CompleteLevel();
        }
    }

    public void KillCount()
    {
        _killCount = _killCount + 1;
        //Debug.Log("killCount " + _killCount);

    }

    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
        killCountVictoryText.text = DisplayKillCount().ToString();
        Debug.Log("LEVEL WON!");
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
