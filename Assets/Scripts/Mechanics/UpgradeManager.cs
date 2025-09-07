using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    [System.Serializable]
    public class Upgrade
    {
        public string upgradeName;
        public string effectDescription;
        public Sprite icon;
        public string id;
    }


    public GameObject upgradePanel;

    public Button[] upgradeButtons;                 
    public TMP_Text[] upgradeNameTexts;             
    public TMP_Text[] upgradeEffectTexts;
    public Image[] UpgradeIcon;
    [SerializeField] Animator anim;
    [SerializeField] ParticleSystem richesRain;

    public List<Upgrade> allUpgrades = new List<Upgrade>();

    private List<Upgrade> availableUpgrades = new List<Upgrade>();
    private Upgrade[] currentOptions = new Upgrade[3];
    [SerializeField] private AudioSource upgradeMusicSource;
   

    void Start()
    {
        progressSlider.maxValue = cashGoal;

        SetupPlayer();
        //allUpgrades = new List<Upgrade>()
        //{
        //    new Upgrade { upgradeName = "Sharpen Blade", 
        //        effectDescription = "Increase Melee Damage", 
        //        icon = Resources.Load<Sprite>("Sprite/Sharpen Blade"), id = "MeleeDamage"},
        //
        //    new Upgrade { upgradeName = "Polish Blade", 
        //        effectDescription = "Increase Melee Speed", 
        //        icon = Resources.Load<Sprite>("Sprite/Polish Blade"), id = "MeleeSpeed" },
        //
        //    new Upgrade { upgradeName = "Reinforced Shield",
        //        effectDescription = "Increase Health", 
        //        icon = Resources.Load<Sprite>("Sprite/Reinforced Shield"),id = "MaxHealth" },
        //
        //    new Upgrade { upgradeName = "Streamline Casings", 
        //        effectDescription = "Increase Projectile Speed / Range", 
        //        icon = Resources.Load<Sprite>("Sprite/Streamline Casings"), id = "BulletSpeed" },
        //
        //    new Upgrade { upgradeName = "Sharpen Casings", 
        //        effectDescription = "Increase Projectile Damage", 
        //        icon = Resources.Load<Sprite>("Sprite/Sharpen Casings"), id = "BulletDamage" },
        //
        //    new Upgrade { upgradeName = "Strengthen Arm", 
        //        effectDescription = "Increase Knockback", 
        //        icon = Resources.Load<Sprite>("Sprite/Strengthen Arm"), id = "Knockback" },
        //
        //    new Upgrade { upgradeName = "Oil Limbs", 
        //        effectDescription = "Increase Move Speed", 
        //        icon = Resources.Load<Sprite>("Sprite/Sharpen Blade"), id = "MoveSpeed" },
        //
        //                new Upgrade { upgradeName = "Improve Heat Sink",
        //        effectDescription = "Reduces gun's heat buildup",
        //        icon = Resources.Load<Sprite>("Sprite/IncreaseAmmo"), id = "Heat" },
        //
        //};

        availableUpgrades = new List<Upgrade>(allUpgrades);

        upgradePanel.SetActive(false);

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int index = i;
            upgradeButtons[i].onClick.AddListener(() => SelectUpgrade(index));
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ShowUpgradeOptions();
        }
    }

    internal void ShowUpgradeOptions()
    {
        richesRain.Play();
        //if (availableUpgrades.Count < 3)
        //{
        //    Debug.LogWarning("Not enough upgrades left!");
        //    return;
        //}

        // Pick 3 random unique upgrades
        List<Upgrade> tempList = new List<Upgrade>(availableUpgrades);
        for (int i = 0; i < 3; i++)
        {
            int rand = Random.Range(0, tempList.Count);
            currentOptions[i] = tempList[rand];
            tempList.RemoveAt(rand);

            upgradeNameTexts[i].text = currentOptions[i].upgradeName;
            upgradeEffectTexts[i].text = currentOptions[i].effectDescription;


            if (UpgradeIcon[i] != null && currentOptions[i].icon != null)
            {
                UpgradeIcon[i].sprite = currentOptions[i].icon;
                UpgradeIcon[i].enabled = true;
            }

            if (UpgradeIcon[i] == null)
                Debug.LogWarning("Missing Image reference at index " + i);
            if (currentOptions[i].icon == null)
                Debug.LogWarning("Missing sprite for upgrade: " + currentOptions[i].upgradeName);

            AudioManager.PauseMusic();
            AudioManager.PlayEffect(SoundType.UPGRADE_MUSIC, 0.35f);
            upgradeMusicSource.Play();

        }


        upgradePanel.SetActive(true);
        Time.timeScale = 0f;
        anim.SetTrigger("Enter");
    }

    public void SelectUpgrade(int index)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Entered"))
        {
            Upgrade chosen = currentOptions[index];
            Debug.Log($"Selected: {chosen.upgradeName}");


            //availableUpgrades.RemoveAll(u => u.upgradeName == chosen.upgradeName);

            playerLevel++;
            levelText.text = playerLevel.ToString();

            DoUpgrade(chosen.id);

            upgradePanel.SetActive(false);
            richesRain.Stop();
            Time.timeScale = 1f;
            upgradeMusicSource.Stop();
            AudioManager.resumeMusic();
        }
    }

    [Header("Variables")]
    [SerializeField] internal float cashGoal;
    [SerializeField] int playerLevel = 1;

    [Header("Stat Increases")]
    [SerializeField] int healthIncrease = 10;
    [SerializeField] float attackSpeedIncrease = 0.1f;
    [SerializeField] int damageIncrease = 3;
    [SerializeField] float bulletSpeedIncrease = 1;
    [SerializeField] float bulletCoolIncrease = 1;
    [SerializeField] int bulletDamageIncrease = 1;
    [SerializeField] int bulletPierceIncrease = 1;
    [SerializeField] float moveSpeedIncrease = 0.5f;
    [SerializeField] float knockbackIncrease = 1;
    [SerializeField] float parryMultIncrease = 0.1f;
    [SerializeField] int heatIncrease = 1;
    [SerializeField] float rangedSizeIncrease;
    [SerializeField] float meleeSizeIncrease;


    [Header("Components")]
    [SerializeField] PlayerMovement movement;
    [SerializeField] PlayerMeleeAttack melee;
    [SerializeField] PlayerRangedAttack ranged;
    [SerializeField] PlayerHealth health;
    [SerializeField] PlayerHealthUI healthUI;
    [SerializeField] TMP_Text levelText;
    [SerializeField] Slider progressSlider;

    void SetupPlayer()
    {
        if(movement == null)
        {
            GameObject player = GameObject.Find("Player");
            movement = player.GetComponent<PlayerMovement>();
            melee = player.GetComponent<PlayerMeleeAttack>();
            ranged = player.GetComponent<PlayerRangedAttack>();
            health = player.GetComponent<PlayerHealth>();
        }
    }

    public void DoUpgrade(string stat)
    {
        if (stat == "Damage")
        {
            melee.IncreaseDamage(damageIncrease);
            melee.IncreaseKnockback(knockbackIncrease);
        }
        else if (stat == "Speed")
        {
            melee.IncreaseSpeed(attackSpeedIncrease);
            ranged.IncreaseSpeed(bulletSpeedIncrease, bulletCoolIncrease);
        }
        else if (stat == "MaxHealth")
        {
            health.IncreaseMax(healthIncrease);
            healthUI.UpdateMax();
        }
        else if (stat == "Heat")
        {
            ranged.IncreaseHeat(heatIncrease);
        }
        else if (stat == "BulletDamage")
        {
            ranged.IncreaseDamage(bulletDamageIncrease, bulletPierceIncrease);
        }
        else if (stat == "Move")
        {
            movement.IncreaseSpeed(moveSpeedIncrease);
        }
        //else if (stat == "Parry")
        //{
        //    GameManager.instance.IncreaseParryMult(parryMultIncrease);
        //}
        else if (stat == "Heat")
        {
            ranged.IncreaseHeat(heatIncrease);
        }
        else if (stat == "Size")
        {
            ranged.IncreaseSize(rangedSizeIncrease);
            melee.IncreaseSize(meleeSizeIncrease);
        }
        else
        {
            Debug.LogError("Error: Invalid Upgrade");
        }

        increaseGoal();
    }

    void increaseGoal()
    {
        cashGoal *= 1.5f;
        progressSlider.maxValue = cashGoal;
        progressSlider.value = 0;
        GameManager.instance._BoltCount = 0;
    }
}
