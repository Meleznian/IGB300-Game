using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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


    public TextMeshProUGUI botText;
    private int _BotCount = 0;

    bool gameHasEnded = false;
    bool gameHasWon = false;
    public float restartDelay = 0.5f;
    private int _killCount = 0;

    public int KillTarget = 10;

    public int crowdHype;
    public float cashMult;
    public int steamGauge;
    public int ammo;
    public int maxAmmo;
    public float parryMult = 1.2f;

    [SerializeField] Slider steamSlider;
    [SerializeField] Slider hypeSlider;
    [SerializeField] Slider ammoDisplay;
    [SerializeField] GameObject floorBullet;

    private void Start()
    {
        ammoDisplay.value = ammo;
    }

    void Update()
    {
        killCountText.text = "Kills: " + DisplayKillCount();
        //botText.text = _BotCount.ToString();

        DisplayBotCount();//why was this remove?

        if (_killCount >= KillTarget & gameHasWon == false)
        {
            gameHasWon = true;
            CompleteLevel();
        }


    }

    public void KillCount()
    {
        _killCount = _killCount + 1;
        killCountText.text = "Kills: " + DisplayKillCount();
        Debug.Log("killCount " + _killCount);

    }

    public void BotCount()
    {
        //Debug.Log("_BotCount " + _BotCount);
        _BotCount += 100;
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

    public int DisplayBotCount()
    {
        Debug.Log("_BotCount " + _BotCount);
        return _BotCount;
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
        crowdHypeText.text = crowdHype.ToString();
        hypeSlider.value = crowdHype;

        CalcMult();
    }

    public void DecreaseHype()
    {
        crowdHype /= 2;
        crowdHypeText.text = crowdHype.ToString();
        hypeSlider.value = crowdHype;

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

    public void IncreaseGauge()
    {
        steamGauge++;
        steamGauge = Mathf.Clamp(steamGauge, 0, 10);
        steamSlider.value = steamGauge;
    }

    public bool DecreaseGauge(int amount)
    {
        if (steamGauge - amount >= 0)
        {
            steamGauge -= amount;
            steamSlider.value = steamGauge;
            return true;
        }
        else
        {
            return false;
        }  
    }

    public void IncreaseAmmo(int amount)
    {
        ammo += amount;
        ammo = Mathf.Clamp(ammo, 0, maxAmmo);
        ammoDisplay.value = ammo;
    }

    public bool DecreaseAmmo(int amount)
    {
        if (ammo - amount >= 0)
        {
            ammo -= amount;
            ammoDisplay.value = ammo;
            return true;
        }
        else
        {
            return false;
        }
    }

    internal void SpawnBullets(int b, Vector3 position)
    {
        while (b > 0)
        {
            var bullet = Instantiate(floorBullet, position, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector3(UnityEngine.Random.Range(-2.0f,3.0f), 0, 0), ForceMode2D.Impulse);
            b--;
        }
    }

    internal void IncreaseParryMult(float amount)
    {
        parryMult += amount;
    }

    public void IncreaseMaxAmmo(int amount)
    {
        maxAmmo += amount;
        IncreaseAmmo(maxAmmo - ammo);
    }
}
