using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUIManager : MonoBehaviour
{
    [System.Serializable]
    public class WeaponData
    {
        public string name;
        public int attack;
        public float attackSpeed;
        public int cost;
    }


    public GameObject weaponInfoPanel;
    public TMP_Text nameText;
    public TMP_Text attackText;
    public TMP_Text speedText;
    public TMP_Text costText;
    public TMP_Text boltText;
    public Button buyButton;

    public WeaponData[] weapons;


    public int playerCurrency = 500;

    private int selectedWeaponIndex = -1;

    void Start()
    {
        UpdateCoinUI();
    }

    public void ShowWeaponInfo(int index)
    {
        if (index < 0 || index >= weapons.Length) return;

        selectedWeaponIndex = index;

        WeaponData weapon = weapons[index];
        nameText.text = weapon.name;
        attackText.text = "Attack: +" + weapon.attack + "%";
        speedText.text = "Attack Speed: +" + weapon.attackSpeed.ToString("F1");
        costText.text = "Cost: " + weapon.cost;

        weaponInfoPanel.SetActive(true);
        Debug.Log("Selected weapon: " + weapons[index].name);

    }

    public void HideWeaponInfo()
    {
        weaponInfoPanel.SetActive(false);
        selectedWeaponIndex = -1;
    }

    public void BuySelectedWeapon()
    {
        Debug.Log("Button Clicked!");

        if (selectedWeaponIndex == -1) return;

        WeaponData weapon = weapons[selectedWeaponIndex];

        if (playerCurrency >= weapon.cost)
        {
            playerCurrency -= weapon.cost;
            UpdateCoinUI();
            Debug.Log($"Bought {weapon.name}");
        }
        else
        {
            Debug.Log("Not enough Bolts!");
        }
    }


    void UpdateCoinUI()
    {
        boltText.text = ": " + playerCurrency;
    }
}