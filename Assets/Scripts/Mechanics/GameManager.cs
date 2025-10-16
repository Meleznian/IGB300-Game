using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static EnemyManager;

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
    internal float distanceTravelled;


    internal Transform killWall;
    [SerializeField] UpgradeManager upgrader;

    [SerializeField] private DetailPanel detailPanel;
    [SerializeField] internal bool tutorial;


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
    public int _killCount = 0;

    public int KillTarget = 10;

    //public int crowdHype;
    public int steamGauge;
    public int maxSteam;
    public int ammo;
    public int maxAmmo;
    //public float parryMult = 1.2f;

    [SerializeField] Slider steamSlider;
    //[SerializeField] Slider hypeSlider;
    [SerializeField] Slider ammoDisplay;
    [SerializeField] Slider levelProgress;
    [SerializeField] GameObject floorBullet;


    [Serializable]
    public class Pickup
    {
        public GameObject prefab;
        public float weight;
        public string spawnPercentage;
    }

    [Header("Pickups")]
    [SerializeField] public Pickup[] pickups;
    float pickupsTotalWeight;

    private void Start()
    {
        ammoDisplay.value = ammo;
        AudioManager.PlayMusic(SoundType.MAIN_MUSIC, 0.3f);

        //Player
        Player = GameObject.Find("Player");
        playerHealth = Player.GetComponent<PlayerHealth>();
        playerMovement = Player.GetComponent<PlayerMovement>();
        cameraScript = Camera.main.GetComponent<CameraFollow>();
        // Store starting position
        startPosition = Player.transform.position;

        if (!tutorial)
        {
            killWall = GameObject.Find("KillWall").transform;
        }

        //steamSlider.maxValue = maxSteam;

        foreach (Pickup p in pickups)
        {
            pickupsTotalWeight += p.weight;
            print("Total Weight is: " + pickupsTotalWeight);
        }
        foreach (Pickup p in pickups)
        {
            GetSpawnPercentage(p);
        }

        if (!tutorial)
        {
            SetupCutscene();
        }
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

        //if (detailPanel != null)
        //{
        //    detailPanel.UpdateDistance(distanceTravelled);
        //}


    }

    public void KillCount()
    {
        _killCount++;
        killCountText.text = "Kills: " + DisplayKillCount();

        //if (detailPanel != null)
        //{
        //    detailPanel.UpdateKills(_killCount);
        //}

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
        //if (detailPanel != null)
        //{
        //    detailPanel.UpdateBoltCount(0, steelBolts);
        //    detailPanel.UpdateBoltCount(1, brassBolts);
        //    detailPanel.UpdateBoltCount(2, silverBolts);
        //    detailPanel.UpdateBoltCount(3, goldBolts);
        //}

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
        if (gameHasEnded == false)
        {
            print("Ending Game");
            gameHasEnded = true;

            if (tutorial)
            {
                SceneManager.LoadScene("Start Scene");
            }
            else
            {
                ScoreManager.instance.StopScoring();
                MenuManager.instance.PlayerDead();

                //UICanvas.SetActive(false);

                //Debug.Log("GAME OVER!!!");
                //YouDiedtxt.gameObject.SetActive(true);//for gameObject
                ////YouDiedtxt.enabled = true;//for component only 
                //Invoke("Restart", restartDelay);
                ////Restart();
                ///
            }

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

    //public void IncreaseGauge()
    //{
    //    steamGauge++;
    //    steamGauge = Mathf.Clamp(steamGauge, 0, maxSteam);
    //    steamSlider.value = steamGauge;
    //
    //    if(steamGauge == maxSteam && !playerHealth.isHealing)
    //    {
    //        playerHealth.StartHealing();
    //        steamGauge = 0;
    //    }
    //}

    //public bool DecreaseGauge(int amount)
    //{
    //    if (steamGauge - amount >= 0)
    //    {
    //        steamGauge -= amount;
    //        steamSlider.value = steamGauge;
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }  
    //}


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
    [SerializeField] float valueMultiplier;
    [SerializeField] float valueIncrease;
    [SerializeField] int boltClamp;


    public GameObject[] GenerateMoney(float amount)
    {
        amount = Mathf.Round(amount * valueMultiplier);
        amount = Mathf.Clamp(amount, 0, boltClamp);
        print("Cash After Mult: " + amount);
        List<GameObject> cashList = new();

        while (amount > 0)
        {
            if (amount <= 20 && amount > 5)
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

    internal GameObject GetPickup()
    {
        float roll = UnityEngine.Random.Range(0, pickupsTotalWeight);

        //print("Total Weight is: " + pickupsTotalWeight);
        //print("Roll was: " + roll);

        int i = 0;

        foreach (Pickup p in pickups)
        {
            roll -= p.weight;

            if (roll < 0)
            {
                return p.prefab;
            }
            i++;
        }

        return pickups[0].prefab;
    }

    void GetSpawnPercentage(Pickup p)
    {
        float s = p.weight / pickupsTotalWeight;
        s *= 100;
        p.spawnPercentage = s + "%";
    }

    internal GameObject GetScaledBolt()
    {
        int level = ScoreManager.instance.currentScore;

        if (level > 20000)
        {
            return bolt20;
        }
        else if(level > 10000)
        {
            return bolt10;
        }
        else if (level > 5000)
        {
            return bolt5;
        }
        else
        {
            return bolt1;
        }

    }

    internal void IncreaseBoltMultiplier()
    {
        valueMultiplier += valueIncrease;
    }

    PlayerMovement playerMovement;
    CameraFollow cameraScript;

    public void StartGame()
    {
        EnemyManager.instance.gameObject.SetActive(true);
        playerMovement.enabled = true;
        killWall.GetComponent<KillWall>().BeginWalking();
        cameraScript.enabled = true;
        ScoreManager.instance.gameObject.SetActive(true);
    }

    public void SetupCutscene()
    {
        EnemyManager.instance.gameObject.SetActive(false);
        playerMovement.enabled = false;
        cameraScript.enabled = false;
        ScoreManager.instance.gameObject.SetActive(false);
    }
}


