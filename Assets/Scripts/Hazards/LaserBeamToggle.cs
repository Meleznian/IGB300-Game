using System.Collections;
using UnityEngine;

public class LaserBeamToggle : MonoBehaviour
{
    [Header("Toggle Settings")]
    [SerializeField] private float onDuration = 2f;
    [SerializeField] private float offDuration = 1.5f;

    [Header("Components")]
    [SerializeField] private SpriteRenderer laserVisual;
    [SerializeField] private Collider2D laserCollider;

    private void Start()
    {
        StartCoroutine(LaserCycle());
    }

    private IEnumerator LaserCycle()
    {
        while (true)
        {
            // ON
            SetLaserActive(true);
            yield return new WaitForSeconds(onDuration);

            // OFF
            SetLaserActive(false);
            yield return new WaitForSeconds(offDuration);
        }
    }

    private void SetLaserActive(bool isActive)
    {
        if (laserVisual != null)
            laserVisual.enabled = isActive;

        if (laserCollider != null)
            laserCollider.enabled = isActive;
    }
}
