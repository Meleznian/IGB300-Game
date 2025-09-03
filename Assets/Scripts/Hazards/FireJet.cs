using System.Collections;
using UnityEngine;

public class FireJet : MonoBehaviour
{

    [Header("Timing Settings")]
    [SerializeField] int damage;
    [SerializeField] private float activeDuration = 1.5f;
    [SerializeField] private float inactiveDuration = 2.0f;
    [SerializeField] float damageInterval;

    [Header("Flame Settings")]
    [SerializeField] private ParticleSystem flameParticles;
    [SerializeField] private Collider2D damageTrigger;

    bool canDamage;
    float timer;

    private void Start()
    {
        StartCoroutine(FireCycle());
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (timer <= 0 && !canDamage)
        {
            canDamage = true;
        }
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canDamage)
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(Mathf.RoundToInt(damage));
            timer = damageInterval; // Allow time between the first damage.
            canDamage = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timer = 0;
        }
    }

}
