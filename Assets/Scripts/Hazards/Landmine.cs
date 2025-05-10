using UnityEngine;

public class Landmine : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private GameObject explosionEffect; // Effect
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeMagnitude = 0.3f;

    private bool hasExploded = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasExploded) return;

        if (other.CompareTag("Player"))
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(Mathf.RoundToInt(damage));
            }

            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            if (CameraShake.Instance != null)
            {
                StartCoroutine(CameraShake.Instance.Shake(0.3f, 0.4f));
            }
            hasExploded = true;
            Destroy(gameObject);
        }
    }

}
