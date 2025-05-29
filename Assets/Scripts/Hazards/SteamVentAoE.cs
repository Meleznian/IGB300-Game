using UnityEngine;
using System.Collections.Generic;

public class SteamVentAoE : MonoBehaviour
{
    [Header("Steam AoE Settings")]
    public float steamActiveDuration = 1.5f;    
    public float steamInactiveDuration = 3f;    
    public ParticleSystem steamEffect;
    public Collider2D damageArea;
    public int damageAmount = 10;

    private float timer = 0f;
    private bool isActive = false;
    private HashSet<GameObject> damagedThisCycle = new HashSet<GameObject>();

    void Update()
    {
        timer += Time.deltaTime;

        if (!isActive && timer >= steamInactiveDuration)
        {
            ActivateSteam();
        }
        else if (isActive && timer >= steamActiveDuration)
        {
            DeactivateSteam();
        }

        if (isActive)
        {
            CheckDamage();
        }
    }

    void ActivateSteam()
    {
        isActive = true;
        timer = 0f;
        damagedThisCycle.Clear();

        if (steamEffect != null)
            steamEffect.Play();

        if (damageArea != null)
            damageArea.enabled = true;
    }

    void DeactivateSteam()
    {
        isActive = false;
        timer = 0f;

        if (steamEffect != null)
            steamEffect.Stop();

        if (damageArea != null)
            damageArea.enabled = false;
    }

    void CheckDamage()
    {
        if (damageArea == null) return;

        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        Collider2D[] results = new Collider2D[10];
        int hitCount = Physics2D.OverlapCollider(damageArea, filter, results);

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D col = results[i];
            if (col != null && col.CompareTag("Player") && !damagedThisCycle.Contains(col.gameObject))
            {
                PlayerHealth player = col.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    player.TakeDamage(damageAmount);
                    damagedThisCycle.Add(col.gameObject);
                }
            }
        }
    }
}
