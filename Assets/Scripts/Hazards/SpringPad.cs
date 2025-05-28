using UnityEngine;

public class SpringPad : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private Vector2 bounceDirection = Vector2.up;
    [SerializeField] private float bounceForce = 15f;

    [Header("Tag Settings")]
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; 
                rb.AddForce(bounceDirection.normalized * bounceForce, ForceMode2D.Impulse);

                Debug.Log("Spring activated: player bounced!");
            }
        }
    }
}
