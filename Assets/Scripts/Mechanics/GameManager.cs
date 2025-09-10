using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

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
    //public TMP_Text crowdHypeText;
    public GameObject completeLevelUI;
    public GameObject UICanvas;

    public GameObject Player;
    [SerializeField] PlayerHealth playerHealth;
    private Vector3 startPosition;
    private float distanceTravelled;


    internal Transform killWall;
    [SerializeField] UpgradeManager upgrader;

    [SerializeField] private DetailPanel detailPanel;


    [Header("Bolt Counters")]
    public int steelBolts;
    public int brassBolts;
    public int silverBolts;
    public int goldBolts;

    public TextMeshProUGUI boltText;
    [SerializeField] internal float _BoltCount = 0;


    bool gameHasEnded = false;
    bool gameHasWon = false;
    internal bool playerDead;
    public float restartDelay = 0.5f;
    private int _killCount = 0;

    public int KillTarget = 10;

    //public int crowdHype;
    public float cashMult;
    public int steamGauge;
    public int maxSteam;
    public int ammo;
    public int maxAmmo;
    public float parryMult = 1.2f;

    [SerializeField] Slider steamSlider;
    //[SerializeField] Slider hypeSlider;
    [SerializeField] Slider ammoDisplay;
    [SerializeField] Slider levelProgress;
    [SerializeField] GameObject floorBullet;

    private void Start()
    {
        ammoDisplay.value = ammo;
        AudioManager.PlayMusic(SoundType.MAIN_MUSIC,0.3f);

        //Player
        Player = GameObject.Find("Player");
        playerHealth = Player.GetComponent<PlayerHealth>();
        // Store starting position
        startPosition = Player.transform.position;

        killWall = GameObject.Find("KillWall").transform;
        steamSlider.maxValue = maxSteam;
    }

    void Update()
    {
        killCountText.text = "Kills: " + DisplayKillCount();
        //boltText.text = _BoltCount.ToString();

        //DisplayBoltCount();//why was this remove? //If you have questions put them in the chat. There are several reasons this was removed 

        //if (_killCount >= KillTarget & gameHasWon == false)
        //{
        //    gameHasWon = true;
        //    CompleteLevel();
        //}

        // Distance travelled
        distanceTravelled = Player.transform.position.x - startPosition.x;
        distanceTravelled = Mathf.Max(0, distanceTravelled); // Shouldn't count the negative value such as -0.1meter etc

        if (detailPanel != null)
        {
            detailPanel.UpdateDistance(distanceTravelled);
        }


    }

    public void KillCount()
    {
        _killCount++;
        killCountText.text = "Kills: " + DisplayKillCount();

        if (detailPanel != null)
        {
            detailPanel.UpdateKills(_killCount);
        }

        Debug.Log("killCount " + _killCount);
    }


    public void BoltCount(float amount)
    {
        playerHealth.getBolt.Play();
        _BoltCount += amount;
        levelProgress.value = _BoltCount;

        // Track the correct bolt type
        if (amount == 1)
            steelBolts++;
        else if (amount == 5)
            brassBolts++;
        else if (amount == 10)
            silverBolts++;
        else if (amount == 20)
            goldBolts++;

        // Update DetailPanel UI
        if (detailPanel != null)
        {
            detailPanel.UpdateBoltCount(0, steelBolts);
            detailPanel.UpdateBoltCount(1, brassBolts);
            detailPanel.UpdateBoltCount(2, silverBolts);
            detailPanel.UpdateBoltCount(3, goldBolts);
        }

        // Check for upgrade condition
        if (_BoltCount >= upgrader.cashGoal)
        {
            upgrader.ShowUpgradeOptions();
        }
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

    public float DisplayBoltCount()
    {
        Debug.Log("_BoltCount " + _BoltCount);
        return _BoltCount;
    }

    public void EndGame()
    {
        if(gameHasEnded == false)
        {
            print("Ending Game");
            gameHasEnded = true;

            UICanvas.SetActive(false);
            ScoreManager.instance.StopScoring();
            MenuManager.instance.PlayerDead();
            //Debug.Log("GAME OVER!!!");
            //YouDiedtxt.gameObject.SetActive(true);//for gameObject
            ////YouDiedtxt.enabled = true;//for component only 
            //Invoke("Restart", restartDelay);
            ////Restart();
       
        }
        
    }


    //public void IncreaseHype()
    //{
    //    crowdHype++;
    //    crowdHype = Mathf.Clamp(crowdHype, 0, 10);
    //    hypeSlider.value = crowdHype;
    //
    //    CalcMult();
    //}
    //
    //public void DecreaseHype()
    //{
    //    crowdHype /= 2;
    //    hypeSlider.value = crowdHype;
    //
    //    CalcMult();
    //}

    //void CalcMult()
    //{
    //    if(crowdHype == 10)
    //    {
    //        cashMult = 2;
    //    }
    //    else if(crowdHype >= 7)
    //    {
    //        cashMult = 1.7f;
    //    }
    //    else if (crowdHype >= 5)
    //    {
    //        cashMult = 1.5f;
    //    }
    //    else if (crowdHype >= 3)
    //    {
    //        cashMult = 1.2f;
    //    }
    //    else
    //    {
    //        cashMult = 1f;
    //    }
    //
    //    //crowdHypeText.text = "x"+ cashMult;
    //}

    public void IncreaseGauge()
    {
        steamGauge++;
        steamGauge = Mathf.Clamp(steamGauge, 0, maxSteam);
        steamSlider.value = steamGauge;

        if(steamGauge == maxSteam && !playerHealth.isHealing)
        {
            playerHealth.StartHealing();
            steamGauge = 0;
        }
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

    
    //public bool DecreaseHeat(int amount)
    //{
    //    if (ammo - amount >= 0)
    //    {
    //        ammo -= amount;
    //        ammoDisplay.value = ammo;
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //internal void SpawnBullets(int b, Vector3 position)
    //{
    //    while (b > 0)
    //    {
    //        var bullet = Instantiate(floorBullet, position, transform.rotation);
    //        bullet.GetComponent<Rigidbody2D>().AddForce(new Vector3(UnityEngine.Random.Range(-2.0f,3.0f), 0, 0), ForceMode2D.Impulse);
    //        b--;
    //    }
    //}

    //internal void IncreaseParryMult(float amount)
    //{
    //    parryMult += amount;
    //}
    //
    //public void IncreaseMaxAmmo(int amount)
    //{
    //    maxAmmo += amount;
    //    IncreaseAmmo(maxAmmo - ammo);
    //}

    [Header("Bolts")]
    [SerializeField] GameObject bolt1;
    [SerializeField] GameObject bolt5;
    [SerializeField] GameObject bolt10;
    [SerializeField] GameObject bolt20;
    
    public GameObject[] GenerateMoney(float amount)
    {
        amount = Mathf.Round(amount*cashMult);
        print("Cash After Mult: " + amount);
        List<GameObject> cashList = new();
    
        while(amount > 0)
        {
            if(amount >= 60 && amount > 40)
            {
                cashList.Add(bolt20);
                amount -= 20;
            }
            else if (amount <= 40 && amount > 20)
            {
                cashList.Add(bolt10);
                amount -= 10;
            }
            else if (amount <= 20 && amount > 5)
            {
                cashList.Add(bolt5);
                amount -= 5;
            }
            else if (amount <= 5)
            {
                cashList.Add(bolt1);
                amount -= 1;
            }
            else
            {
                print(amount);
                break;
            }
        }
        print(cashList.ToArray());
        return cashList.ToArray();
    }
}


