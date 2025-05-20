using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP setting")]
    public int maxHealth = 100;          // Maximum HP
    public int currentHealth;            // Current HP

    [Header("Recovery Settings")]
    public int healAmountPerTick = 5;    // Amount of recovery (per tick)
    public float healTickDelay = 0.5f;   // Tick interval (seconds)
    public int totalHealTicks = 5;       // Recovery times
    public int healCharge = 1;           // Charge used for heels

    public bool isHealing = false;       // Currently recovering?

    void Start()
    {
        currentHealth = maxHealth;       // Maximize HP at the start of the game
        anim = GetComponent<Animator>();
    }

    private Animator anim;

    void Update()
    {
        // Q key to recover (when charged and not in recovery)
        if (Input.GetKeyDown(KeyCode.Q) && healCharge > 0 && !isHealing)
        {
            StartCoroutine(HealOverTime());
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        GameManager.instance.DecreaseHype();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            anim.SetTrigger("Killed");

        }
    }

    IEnumerator HealOverTime()
    {
        isHealing = true;
        healCharge--;

        for (int i = 0; i < totalHealTicks; i++)
        {
            currentHealth += healAmountPerTick;

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            yield return new WaitForSeconds(healTickDelay);
        }

        isHealing = false;
    }

    void Die()
    {
        Debug.Log("Player died. Reset to Wave 1...");
        FindFirstObjectByType<GameManager>().EndGame();
    }
}
