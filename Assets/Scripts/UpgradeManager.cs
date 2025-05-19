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
    }


    public GameObject upgradePanel;

    public Button[] upgradeButtons;                 
    public TMP_Text[] upgradeNameTexts;             
    public TMP_Text[] upgradeEffectTexts;
    public Image[] UpgradeIcon;

    private List<Upgrade> allUpgrades = new List<Upgrade>();
    private List<Upgrade> availableUpgrades = new List<Upgrade>();
    private Upgrade[] currentOptions = new Upgrade[3];

    void Start()
    {
        allUpgrades = new List<Upgrade>()
        {
            new Upgrade { upgradeName = "Sharpen Blade", 
                effectDescription = "Increase Melee Damage", 
                icon = Resources.Load<Sprite>("Sprite/Sharpen Blade")},

            new Upgrade { upgradeName = "Polish Blade", 
                effectDescription = "Increase Melee Speed", 
                icon = Resources.Load<Sprite>("Sprite/Polish Blade") },

            new Upgrade { upgradeName = "Reinforced Shield",
                effectDescription = "Increase Health", 
                icon = Resources.Load<Sprite>("Sprite/Reinforced Shield") },

            new Upgrade { upgradeName = "Streamline Casings", 
                effectDescription = "Increase Projectile Speed / Range", 
                icon = Resources.Load<Sprite>("Sprite/Streamline Casings") },

            new Upgrade { upgradeName = "Sharpen Casings", 
                effectDescription = "Increase Projectile Damage", 
                icon = Resources.Load<Sprite>("Sprite/Sharpen Casings") },

            new Upgrade { upgradeName = "Strengthen Arm", 
                effectDescription = "Increase Melee Knockback", 
                icon = Resources.Load<Sprite>("Sprite/Strengthen Arm") },

            new Upgrade { upgradeName = "Oil Limbs", 
                effectDescription = "Increase Move Speed", 
                icon = Resources.Load<Sprite>("Sprite/Sharpen Blade") },

            new Upgrade { upgradeName = "Analyse Opponents", 
                effectDescription = "Increase Parry Multiplier", 
                icon = Resources.Load<Sprite>("Sprite/Sharpen Blade") },

            new Upgrade { upgradeName = "Enhanced Charging", 
                effectDescription = "Reduce Special Cooldown", 
                icon = Resources.Load<Sprite>("Sprite/Sharpen Blade") }
        };

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

    void ShowUpgradeOptions()
    {
        if (availableUpgrades.Count < 3)
        {
            Debug.LogWarning("Not enough upgrades left!");
            return;
        }

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

        }


        upgradePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void SelectUpgrade(int index)
    {
        Upgrade chosen = currentOptions[index];
        Debug.Log($"Selected: {chosen.upgradeName}");


        availableUpgrades.RemoveAll(u => u.upgradeName == chosen.upgradeName);

        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
