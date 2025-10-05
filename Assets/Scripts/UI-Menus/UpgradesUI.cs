using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradesUI : MonoBehaviour
{
    [SerializeField] private Transform upgradesPanel;   // Where icons will be added
    [SerializeField] private GameObject upgradeIconPrefab;

    /// <summary>
    /// Called when an upgrade is chosen.
    /// </summary>
    public void UpdateUpgradeUI(UpgradeManager.Upgrade upgrade)
    {
        // Look for existing icon
        foreach (Transform child in upgradesPanel)
        {
            UpgradeIconUI icon = child.GetComponent<UpgradeIconUI>();
            if (icon != null && icon.UpgradeID == upgrade.id)
            {
                icon.UpdateStack(upgrade.timesChosen);
                return;
            }
        }

        // If no icon exists, make a new one
        GameObject newIcon = Instantiate(upgradeIconPrefab, upgradesPanel);
        UpgradeIconUI newIconUI = newIcon.GetComponent<UpgradeIconUI>();
        newIconUI.Setup(upgrade);
    }
}
