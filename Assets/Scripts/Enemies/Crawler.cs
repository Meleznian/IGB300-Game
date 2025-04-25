using UnityEngine;

public class Crawler : EnemyBase
{
    [Header("Crawler Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint; // Drag a child GameObject here in Inspector

    private Vector2 _moveDirection;
    private float _damageCooldown = 1f;
    private float _lastDamageTime;

    public override void Move()
    {
        // Physics-based movement
        rb.velocity = new Vector2(_moveDirection.x * actingMoveSpeed, rb.velocity.y);
        CheckFloor();
    }

    private void ChangeDirection()
    {
        _moveDirection *= -1;
        transform.localScale = new Vector3(-transform.localScale.x, 1, 1); // Flip sprite
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            ChangeDirection();
        }
        else if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            FindFirstObjectByType<GameManager>()?.KillCount(); // Null-safe
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time > _lastDamageTime + _damageCooldown)
        {
            if (other.TryGetComponent<PlayerHealth>(out var health))
            {
                health.TakeDamage(33);
                _lastDamageTime = Time.time;
            }
        }
    }

    void CheckFloor()
    {
        Vector2 rayOrigin = groundCheckPoint.position;
        bool hasGround = Physics2D.Raycast(rayOrigin, Vector2.down, 0.2f, groundLayer);

        Debug.DrawRay(rayOrigin, Vector2.down * 0.2f, hasGround ? Color.green : Color.red);

        if (!hasGround)
        {
            ChangeDirection();
        }
    }

    public override void ExtraSetup()
    {
        _moveDirection = Vector2.left;
    }
}