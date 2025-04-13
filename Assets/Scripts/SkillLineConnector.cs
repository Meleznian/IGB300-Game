using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillLineConnector : MonoBehaviour
{
    public Image lineImage;
    public float fillDuration = 1f;
    public RectTransform shine; // The shining effect
    public bool playGlow = true;

    private float lineLength;

    void Start()
    {
        if (shine != null)
        {
            shine.gameObject.SetActive(false);
        }
    }

    public void AnimateLine()
    {
        StartCoroutine(FillLine());
    }

    IEnumerator FillLine()
    {
        float elapsed = 0f;
        if (lineImage != null) lineImage.fillAmount = 0f;

        if (shine != null)
        {
            shine.gameObject.SetActive(true);
        }

        while (elapsed < fillDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fillDuration);
            if (lineImage != null) lineImage.fillAmount = t;

            if (shine != null)
            {
                // Move shine across the line as it fills
                shine.anchorMin = new Vector2(t, shine.anchorMin.y);
                shine.anchorMax = new Vector2(t, shine.anchorMax.y);
            }

            yield return null;
        }

        if (shine != null)
        {
            shine.gameObject.SetActive(false); // Hide the shine after filling
        }
    }
}
