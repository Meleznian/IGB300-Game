using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Sprite lockedSprite;
    public Sprite unlockedSprite;
    private Image skillImage;
    private bool isUnlocked = false;

    public SkillLineConnector[] linesToNextSkills;

    void Start()
    {
        skillImage = GetComponent<Image>();
        UpdateVisual();
    }

    public void UnlockSkill()
    {
        isUnlocked = true;
        UpdateVisual();

        foreach (var line in linesToNextSkills)
        {
            line.AnimateLine(); // trigger the animated fill
        }
    }

    void UpdateVisual()
    {
        skillImage.sprite = isUnlocked ? unlockedSprite : lockedSprite;
    }
}
