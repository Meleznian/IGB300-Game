using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrailerFadeOut: MonoBehaviour
{
    public float fadeDuration = 2f; // how long the fade takes
    private SpriteRenderer spriteRenderer;
    private Renderer meshRenderer;
    private Image uiImage;
    private Color startColor;
    private bool isFading = false;

    void Start()
    {
        // Try to find a compatible renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        meshRenderer = GetComponent<Renderer>();
        uiImage = GetComponent<Image>();

        if (spriteRenderer != null)
            startColor = spriteRenderer.color;
        else if (meshRenderer != null && meshRenderer.material.HasProperty("_Color"))
            startColor = meshRenderer.material.color;
        else if (uiImage != null)
            startColor = uiImage.color;
        else
            Debug.LogWarning("No compatible renderer found on " + gameObject.name);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isFading)
        {
            StartCoroutine(FadeOutRoutine());
        }
    }

    IEnumerator FadeOutRoutine()
    {
        isFading = true;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);

            if (spriteRenderer != null)
                spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            else if (meshRenderer != null && meshRenderer.material.HasProperty("_Color"))
                meshRenderer.material.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            else if (uiImage != null)
                uiImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            yield return null;
        }

        isFading = false;
    }
}