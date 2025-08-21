using UnityEngine;

public class Spike : MonoBehaviour
{
    string playerTag = "Player";
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"[KillWall] Trigger with {other.gameObject.name}");

        // Player judgment (filter by tag)
        if (other.gameObject.CompareTag(playerTag))
        {

            // Search for PlayerHealth directly or from parent
            var playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth == null)
                playerHealth = other.gameObject.GetComponentInParent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Kill(); // Set HP to 0 and kill
                Debug.Log("[KillWall] Player killed.");
            }
            else
            {
                Debug.LogWarning($"[KillWall] PlayerHealth not found on {other.gameObject.name} or its parents.");
            }
        }
    }
}
