using UnityEngine;

public class SteamTrap : MonoBehaviour
{
    [SerializeField] private float damagePerTick = 5f;          // Amount of damage per time
    [SerializeField] private float damageInterval = 1f;         // Damage every how many seconds?
    private float timer = 0f;

    private bool isPlayerInside = false;
    private PlayerHealth playerHealth;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerHealth = other.GetComponent<PlayerHealth>();
            timer = damageInterval; // Allow time between the first damage.
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            playerHealth = null;
        }
    }

    private void Update()
    {
        if (isPlayerInside && playerHealth != null)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                playerHealth.TakeDamage(Mathf.RoundToInt(damagePerTick));
                timer = damageInterval;
            }
        }
    }
}
