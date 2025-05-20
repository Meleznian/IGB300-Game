using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
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

    public TextMeshProUGUI YouDiedtxt;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI killCountVictoryText;
    public TMP_Text crowdHypeText;
    public GameObject completeLevelUI;

    bool gameHasEnded = false;
    bool gameHasWon = false;
    public float restartDelay = 0.5f;
    private int _killCount = 0;

    public int KillTarget = 10;

    public int crowdHype;
    public float cashMult;

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


    public void IncreaseHype()
    {
        crowdHype++;
        crowdHype = Mathf.Clamp(crowdHype, 0, 10);
        crowdHypeText.text = "Crowd Hype: " + crowdHype;

        CalcMult();
    }

    public void DecreaseHype()
    {
        crowdHype /= 2;
        crowdHypeText.text = "Crowd Hype: " + crowdHype;

        CalcMult();
    }

    void CalcMult()
    {
        if(crowdHype == 10)
        {
            cashMult = 2;
        }
        else if(crowdHype >= 7)
        {
            cashMult = 1.7f;
        }
        else if (crowdHype >= 5)
        {
            cashMult = 1.5f;
        }
        else if (crowdHype >= 3)
        {
            cashMult = 1.2f;
        }
        else
        {
            cashMult = 1f;
        }
    }
}
