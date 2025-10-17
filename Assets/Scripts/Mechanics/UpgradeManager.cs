using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class UpgradeManager : MonoBehaviour
{
    [System.Serializable]
    public class Upgrade
    {
        public string upgradeName;
        public string effectDescription;
        public Sprite icon;
        public string id;
        public bool avaliableAtStart;
        public bool removeAfterSelection;
        public int timesChosen;
    }


    public GameObject upgradePanel;
    [SerializeField] private UpgradesUI upgradesUI;

    public Button[] upgradeButtons;                 
    public TMP_Text[] upgradeNameTexts;             
    public TMP_Text[] upgradeEffectTexts;
    public Image[] UpgradeIcon;
    public TMP_Text[] upgradeTextsIndicator;
    [SerializeField] Animator anim;
    [SerializeField] ParticleSystem richesRain;

    public List<Upgrade> allUpgrades = new List<Upgrade>();
    private List<Upgrade> availableUpgrades = new List<Upgrade>();


    private Upgrade[] currentOptions = new Upgrade[3];
    [SerializeField] private AudioSource upgradeMusicSource;

    private int currentSelection = 0;
    private bool isChoosingUpgrade = false;
    private GameObject lastSelectedButton;

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

        foreach(Upgrade upgrade in allUpgrades)
        {
            if (upgrade.avaliableAtStart)
            {
                availableUpgrades.Add(upgrade);
            }
        }
        upgradePanel.SetActive(false);

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int index = i;
            upgradeButtons[i].onClick.AddListener(() => SelectUpgrade(index));
        }
    }

    void Update()
    {
        // Activate upgrade panel with 9 key for debug
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ShowUpgradeOptions();
        }

        // Restore selection if lost after player has misclick during the gameplay
        if (upgradePanel.activeSelf && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(upgradeButtons[currentSelection].gameObject);
        }

        // Move between buttons with arrow keys
        if (upgradePanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                currentSelection = (currentSelection - 1 + upgradeButtons.Length) % upgradeButtons.Length;
                HighlightCurrentButton();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                currentSelection = (currentSelection + 1) % upgradeButtons.Length;
                HighlightCurrentButton();
            }

            // Select with Enter
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SelectUpgrade(currentSelection);
            }
        }
    }

    void HighlightCurrentButton()
    {
        // Reset all button colors to default (white)
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            var cb = upgradeButtons[i].colors;
            cb.highlightedColor = Color.white;
            cb.selectedColor = Color.white;
            upgradeButtons[i].colors = cb;
        }

        // Highlight only the currently selected one
        var selectedButton = upgradeButtons[currentSelection];
        var selectedCb = selectedButton.colors;
        selectedCb.highlightedColor = Color.grey;
        selectedCb.selectedColor = Color.grey;
        selectedButton.colors = selectedCb;

        // Ensure the EventSystem knows which button is selected
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectedButton.gameObject);
    }



    internal void ShowUpgradeOptions()
    {
        movement.enabled = false;
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

            if (upgradeTextsIndicator[i] != null)
            {
                if (currentOptions[i].timesChosen > 0)
                    upgradeTextsIndicator[i].text = "x" + currentOptions[i].timesChosen;
                else
                    upgradeTextsIndicator[i].text = ""; // empty if never chosen
            }

            AudioManager.PauseMusic();
            AudioManager.PlayEffect(SoundType.UPGRADE_MUSIC, 0.35f);
            upgradeMusicSource.Play();
            HighlightCurrentButton();
        }


        upgradePanel.SetActive(true);
        Time.timeScale = 0f;
        anim.SetTrigger("Enter");
        isChoosingUpgrade = true;
        currentSelection = 0;


    }

    public void SelectUpgrade(int index)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Entered"))
        {
            Upgrade chosen = currentOptions[index];
            Debug.Log($"Selected: {chosen.upgradeName}");

            playerLevel++;
            levelText.text = playerLevel.ToString();

            movement.enabled = true;
            DoUpgrade(chosen.id);
            chosen.timesChosen += 1;

            if (upgradesUI != null)
            {
                upgradesUI.UpdateUpgradeUI(chosen);
            }

            if (chosen.removeAfterSelection)
            {
                availableUpgrades.Remove(chosen);
            }

            upgradePanel.SetActive(false);
            richesRain.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            Time.timeScale = 1f;
            upgradeMusicSource.Stop();
            AudioManager.resumeMusic();

        }
    }

    [Header("Variables")]
    [SerializeField] internal float cashGoal;
    [SerializeField] float goalIncreaseMultiplier;
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

    public void DoUpgrade(string id)
    {
        if (id == "Damage")
        {
            melee.IncreaseDamage(damageIncrease);
            melee.IncreaseKnockback(knockbackIncrease);
        }
        else if (id == "Speed")
        {
            melee.IncreaseSpeed(attackSpeedIncrease);
            ranged.IncreaseSpeed(bulletSpeedIncrease, bulletCoolIncrease);
        }
        else if (id == "MaxHealth")
        {
            health.IncreaseMax(healthIncrease);
            healthUI.UpdateMax();
        }
        else if (id == "Heat")
        {
            ranged.IncreaseHeat(heatIncrease);
        }
        else if (id == "BulletDamage")
        {
            ranged.IncreaseDamage(bulletDamageIncrease, bulletPierceIncrease);
        }
        else if (id == "Move")
        {
            movement.IncreaseSpeed(moveSpeedIncrease);
        }
        //else if (id == "Parry")
        //{
        //    GameManager.instance.IncreaseParryMult(parryMultIncrease);
        //}
        //else if (id == "Heat")
        //{
        //    ranged.IncreaseHeat(heatIncrease);
        //}
        else if (id == "Size")
        {
            ranged.IncreaseSize(rangedSizeIncrease);
            melee.IncreaseSize(meleeSizeIncrease);
        }
        else if( id == "Spear")
        {
            ranged.UnlockSpear();
            EnableRangedUpgrades();
        }
        else if (id == "Gun")
        {
            ranged.UnlockBullet();
            EnableRangedUpgrades();
        }
        else if (id == "Axe")
        {
            ranged.UnlockAxe();
            EnableRangedUpgrades();
        }
        else
        {
            Debug.LogError("Error: Invalid Upgrade");
        }

        increaseGoal();
    }

    void increaseGoal()
    {
        cashGoal *= goalIncreaseMultiplier;
        progressSlider.maxValue = cashGoal;
        progressSlider.value = 0;
        GameManager.instance._BoltCount = 0;
    }

    bool rangedActive;
    void EnableRangedUpgrades()
    {
        if (!rangedActive)
        {
            AddUpgrade("BulletDamage");
            rangedActive = true;
        }
    }

    void AddUpgrade(string id)
    {
        foreach (Upgrade upgrade in allUpgrades)
        {
            if (upgrade.id == id)
            {
                availableUpgrades.Add(upgrade);
                break;
            }
        }
    }
}
