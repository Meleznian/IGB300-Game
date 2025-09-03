using UnityEngine;

public class SteamTrap : MonoBehaviour
{
    [SerializeField] private float damagePerTick = 5f;          // Amount of damage per time
    [SerializeField] private float damageInterval = 1f;         // Damage every how many seconds?
    private float timer = 0f;

    private bool canDamage = false;
    private PlayerHealth playerHealth;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canDamage)
        {
            playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(Mathf.RoundToInt(damagePerTick));
            timer = damageInterval; // Allow time between the first damage.
            canDamage = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth = null;
            timer = 0;
        }
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if(timer <= 0 && !canDamage)
        {
            canDamage = true;
        }
    }
}
