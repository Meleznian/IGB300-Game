using UnityEngine;
using UnityEngine.UI;

public class SkillTabController : MonoBehaviour
{
    public GameObject attackPanel, defensePanel, healthPanel;

    // New: add reference to Basic Attack skill group
    public GameObject basicAttackGroup;

    public Button attackTab, defenseTab, healthTab;

    void Start()
    {
        ShowAttackPanel();
    }

    public void ShowAttackPanel()
    {
        ShowPanel(attackPanel);
        basicAttackGroup.SetActive(true);
        Debug.Log("Attack tab clicked!");

    }

    public void ShowDefensePanel()
    {
        ShowPanel(defensePanel);
        basicAttackGroup.SetActive(false);
        Debug.Log("Defense tab clicked!");
    }

    public void ShowHealthPanel()
    {
        ShowPanel(healthPanel);
        basicAttackGroup.SetActive(false);
        Debug.Log("Health tab clicked!");
    }

    void ShowPanel(GameObject activePanel)
    {
        attackPanel.SetActive(activePanel == attackPanel);
        defensePanel.SetActive(activePanel == defensePanel);
        healthPanel.SetActive(activePanel == healthPanel);
    }
}
