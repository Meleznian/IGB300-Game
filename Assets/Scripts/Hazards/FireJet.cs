using System.Collections;
using UnityEngine;

public class FireJet : MonoBehaviour
{
    [Header("Timing Settings")]
    [SerializeField] int damage = 10;
    [SerializeField] private float activeDuration = 1.5f;
    [SerializeField] private float inactiveDuration = 2.0f;
    [SerializeField] private float damageInterval = 0.3f;

    [Header("Flame Settings")]
    [SerializeField] private ParticleSystem flameParticles;
    [SerializeField] private Collider2D damageTrigger; // Å© Even if not set, it will be automatically acquired in Awake.

    // internal state
    private bool canDamage;
    private float timer;

    private void Awake()
    {
        if (!damageTrigger) damageTrigger = GetComponent<Collider2D>();
        if (damageTrigger) damageTrigger.isTrigger = true;
    }

    private void Start()
    {
        StartCoroutine(FireCycle());
    }

    private void Update()
    {
        if (timer > 0f) timer -= Time.deltaTime;
    }

    private IEnumerator FireCycle()
    {
        while (true)
        {
            // Fire ON
            EnableFlame(true);
            yield return new WaitForSeconds(activeDuration);

            // Fire OFF
            EnableFlame(false);
            yield return new WaitForSeconds(inactiveDuration);
        }
    }

    private void EnableFlame(bool isOn)
    {
        // ParticleSystem
        if (flameParticles)
        {
            var emission = flameParticles.emission;
            emission.enabled = isOn;
            if (isOn && !flameParticles.isPlaying) flameParticles.Play();
            if (!isOn && flameParticles.isPlaying) flameParticles.Stop();
        }

        // Synchronize collision detection and damage flags
        if (damageTrigger) damageTrigger.enabled = isOn;
        canDamage = isOn;

        // Prevent multi-hit damage immediately after activation
        timer = 0f;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!canDamage) return;
        if (!other.CompareTag("Player")) return;

        if (timer > 0f) return;

        var hp = other.GetComponent<PlayerHealth>() ?? other.GetComponentInParent<PlayerHealth>();
        if (hp)
        {
            hp.TakeDamage(damage);
            timer = damageInterval; // Cool Down
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Prevent the next one from hitting immediately after it comes out
            timer = 0f;
        }
    }
}
