using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform pointA;     // Starting point of movement
    [SerializeField] private Transform pointB;     // Movement end point
    [SerializeField] private float moveSpeed = 2f; // Movement speed

    private Transform targetPoint;

    private void Start()
    {
        targetPoint = pointB;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.05f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
        }
    }

    // Move players together by parent-child attachment.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }
}
