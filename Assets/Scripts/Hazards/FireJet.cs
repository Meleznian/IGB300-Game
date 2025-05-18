using System.Collections;
using UnityEngine;

public class FireJet : MonoBehaviour
{
    [Header("Timing Settings")]
    [SerializeField] private float activeDuration = 1.5f;
    [SerializeField] private float inactiveDuration = 2.0f;

    [Header("Flame Settings")]
    [SerializeField] private ParticleSystem flameParticles;
    [SerializeField] private Collider2D damageTrigger;

    private void Start()
    {
        StartCoroutine(FireCycle());
    }

    private IEnumerator FireCycle()
    {
        while (true)
        {
            // ON
            EnableFlame(true);
            yield return new WaitForSeconds(activeDuration);

            // OFF
            EnableFlame(false);
            yield return new WaitForSeconds(inactiveDuration);
        }
    }

    private void EnableFlame(bool isOn)
    {
        if (flameParticles != null)
        {
            if (isOn) flameParticles.Play();
            else flameParticles.Stop();
        }

        if (damageTrigger != null)
        {
            damageTrigger.enabled = isOn;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered the flame: " + other.name);

        if (!damageTrigger.enabled) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered flame!");
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);
                Debug.Log("Damage applied to player.");
            }
            else
            {
                Debug.LogWarning("PlayerHealth not found on player!");
            }
        }
    }

}
