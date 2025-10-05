using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradesUI : MonoBehaviour
{
    [SerializeField] private Transform upgradesPanel;
    [SerializeField] private GameObject upgradeIconPrefab;

    void Start()
    {
        upgradesPanel.gameObject.SetActive(false);
    }

    public void UpdateUpgradeUI(UpgradeManager.Upgrade upgrade)
    {
        if (!upgradesPanel.gameObject.activeSelf)
            upgradesPanel.gameObject.SetActive(true);

        // Look for existing icon
        foreach (Transform child in upgradesPanel)
        {
            UpgradeIconUI icon = child.GetComponent<UpgradeIconUI>();
            if (icon != null && icon.UpgradeID == upgrade.id)
            {

                icon.UpdateStack(upgrade.timesChosen);
                return; // Don't spawn a new icon
            }
        }

        // Make a new one if no icon exists
        GameObject newIcon = Instantiate(upgradeIconPrefab, upgradesPanel);
        UpgradeIconUI newIconUI = newIcon.GetComponent<UpgradeIconUI>();
        newIconUI.Setup(upgrade);

        // Shifts others to the right
        newIcon.transform.SetSiblingIndex(0);
    }
}
