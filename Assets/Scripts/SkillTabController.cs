using UnityEngine;
using UnityEngine.UI;

public class SkillTabController : MonoBehaviour
{
    public GameObject attackPanel, defensePanel, healthPanel;

    // New: add reference to Basic Attack skill group
    public GameObject basicAttackGroup;

    public Button attackTab, defenseTab, healthTab;
    public Color selectedColor = Color.yellow;
    public Color defaultColor = Color.white;

    void Start()
    {
        ShowAttackPanel();
    }

    public void ShowAttackPanel()
    {
        ShowPanel(attackPanel);
        basicAttackGroup.SetActive(true);
        UpdateTabColors(attackTab);
        Debug.Log("Attack tab clicked!");

    }

    public void ShowDefensePanel()
    {
        ShowPanel(defensePanel);
        basicAttackGroup.SetActive(false);
        UpdateTabColors(defenseTab);
        Debug.Log("Defense tab clicked!");
    }

    public void ShowHealthPanel()
    {
        ShowPanel(healthPanel);
        basicAttackGroup.SetActive(false);
        UpdateTabColors(healthTab);
        Debug.Log("Health tab clicked!");
    }

    void ShowPanel(GameObject activePanel)
    {
        attackPanel.SetActive(activePanel == attackPanel);
        defensePanel.SetActive(activePanel == defensePanel);
        healthPanel.SetActive(activePanel == healthPanel);
    }

    void UpdateTabColors(Button activeTab)
    {
        attackTab.image.color = (activeTab == attackTab) ? selectedColor : defaultColor;
        defenseTab.image.color = (activeTab == defenseTab) ? selectedColor : defaultColor;
        healthTab.image.color = (activeTab == healthTab) ? selectedColor : defaultColor;
    }
}
