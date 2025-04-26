using UnityEngine;
using UnityEngine.UI;

public class SkillTabController : MonoBehaviour
{
    public GameObject MeleeWeaponPanel, RangedWeaponPanel;


    public Button meleeTab, rangedTab;

    void Start()
    {
        ShowMeleePanel();
    }

    public void ShowMeleePanel()
    {
        ShowPanel(MeleeWeaponPanel);
    }

    public void ShowRangedPanel()
    {
        ShowPanel(RangedWeaponPanel);
    }

    void ShowPanel(GameObject activePanel)
    {
        MeleeWeaponPanel.SetActive(activePanel == MeleeWeaponPanel);
        RangedWeaponPanel.SetActive(activePanel == RangedWeaponPanel);

    }
}
