using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeIconUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text stackText;

    public string UpgradeID { get; private set; }

    public void Setup(UpgradeManager.Upgrade upgrade)
    {
        UpgradeID = upgrade.id;
        iconImage.sprite = upgrade.icon;
        UpdateStack(upgrade.timesChosen);
    }

    public void UpdateStack(int count)
    {
        // Always show at least x1
        stackText.text = "x" + count.ToString();
    }
}
