using UnityEngine;

public class FlameTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered the flame: " + other.name);

        if (other.CompareTag("Player"))
        {
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
