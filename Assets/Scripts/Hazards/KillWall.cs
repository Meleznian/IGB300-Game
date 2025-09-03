using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KillWall : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] Transform player;
    [SerializeField] string playerTag = "Player";
    [SerializeField] string enemyTag = "Enemy";

    [Header("Speed Mapping")]
    [SerializeField] float minSpeed = 1f;    // Near speed
    [SerializeField] float maxSpeed = 8f;    // Speed at Far
    [SerializeField] float nearDist = 10f;   // Below this��min
    [SerializeField] float farDist = 18f;   // No more��max

    [Header("Smoothing")]
    [SerializeField] float accel = 10f;      // Speed following target speed

    float currentSpeed;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void Awake()
    {
        if (!player)
        {
            var tagged = GameObject.FindGameObjectWithTag(playerTag);
            if (tagged) player = tagged.transform;
        }
        currentSpeed = minSpeed;
    }

    void Update()
    {
        // Distance (wall �� player's X difference). Forward is positive.
        float dx = player ? (player.position.x - transform.position.x) : 0f;

        // Linear map: Near��0, Far��1 (automatically clamps values outside range)
        float t = Mathf.InverseLerp(nearDist, farDist, dx);

        // Interpolate speed between min..max 
        float targetSpeed = Mathf.Lerp(minSpeed, maxSpeed, t);

        // Smoothing
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accel * Time.deltaTime);

        // Horizontal movement
        transform.position += Vector3.right * (currentSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            var hp = other.GetComponent<PlayerHealth>() ?? other.GetComponentInParent<PlayerHealth>();
            if (hp) hp.Kill();
            return;
        }
        if (other.CompareTag(enemyTag))
        {
            var enemy = other.GetComponent<EnemyBase>();
            if (enemy) enemy.Die(false);
        }
    }
}
