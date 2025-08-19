using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KillWall : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 2f;

    [Header("Filter")]
    [SerializeField] string playerTag = "Player";

    void Reset()
    {
        // Convert the 2D colliders attached to KillWall to Triggers.
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void Update()
    {
        // Move at a constant speed to the right
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[KillWall] Trigger with {other.name}");

        // Player judgment (filter by tag)
        if (!other.CompareTag(playerTag)) return;

        // Search for PlayerHealth directly or from parent
        var playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth == null)
            playerHealth = other.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.Kill(); // Set HP to 0 and kill
            Debug.Log("[KillWall] Player killed.");
        }
        else
        {
            Debug.LogWarning($"[KillWall] PlayerHealth not found on {other.name} or its parents.");
        }
    }
}
