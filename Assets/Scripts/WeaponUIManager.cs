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
    public GameObject[] tickUI;
    public TMP_Text[] purchasedTextUI;
    public Button[] weaponButtons;


    public int playerCurrency = 500;

    private int selectedWeaponIndex = -1;

    void Start()
    {
        UpdateCoinUI();
    }
    public void ShowWeaponInfo(int index)
    {
        if (index < 0 || index >= weapons.Length) return;

        for (int i = 0; i < weapons.Length; i++)
        {
            if (i != index)
            {
                if (weaponButtons.Length > i && weaponButtons[i])
                    weaponButtons[i].interactable = true;

                if (tickUI.Length > i && tickUI[i])
                    tickUI[i].SetActive(false);

                if (purchasedTextUI.Length > i && purchasedTextUI[i])
                    purchasedTextUI[i].gameObject.SetActive(false);
            }
        }

        selectedWeaponIndex = index;


        WeaponData weapon = weapons[index];
        nameText.text = weapon.name;
        attackText.text = "Attack: +" + weapon.attack + "%";
        speedText.text = "Attack Speed: +" + weapon.attackSpeed.ToString("F1");
        costText.text = "Cost: " + weapon.cost;

        weaponInfoPanel.SetActive(true);
        Debug.Log("Selected weapon: " + weapon.name);

        // Check if already purchased
        bool alreadyPurchased = tickUI.Length > index && tickUI[index] && tickUI[index].activeSelf;

        if (alreadyPurchased)
        {
            buyButton.interactable = false;
            buyButton.GetComponentInChildren<TMP_Text>().text = "Purchased";
        }
        else
        {
            buyButton.interactable = true;
            buyButton.GetComponentInChildren<TMP_Text>().text = "Buy";
        }
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

            // Show tick and purchased text
            if (tickUI.Length > selectedWeaponIndex && tickUI[selectedWeaponIndex])
                tickUI[selectedWeaponIndex].SetActive(true);

            if (purchasedTextUI.Length > selectedWeaponIndex && purchasedTextUI[selectedWeaponIndex])
                purchasedTextUI[selectedWeaponIndex].gameObject.SetActive(true);

            // Disable the button and turn it gray
            if (weaponButtons.Length > selectedWeaponIndex && weaponButtons[selectedWeaponIndex])
            {
                Button btn = weaponButtons[selectedWeaponIndex];
                btn.interactable = false;

                ColorBlock cb = btn.colors;
                cb.normalColor = Color.gray;
                cb.highlightedColor = Color.gray;
                cb.pressedColor = Color.gray;
                cb.selectedColor = Color.gray;
                cb.disabledColor = Color.gray;
                btn.colors = cb;
            }

            buyButton.interactable = false;
            buyButton.GetComponentInChildren<TMP_Text>().text = "Purchased";
            weaponInfoPanel.SetActive(false);
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