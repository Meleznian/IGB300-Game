using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KillWall : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    float defaultSpeed;
    [SerializeField] float rubberbandDistance;
    Transform player;

    [Header("Filter")]
    [SerializeField] string playerTag = "Player";
    [SerializeField] string enemyTag = "Enemy";

    void Reset()
    {
        // Convert the 2D colliders attached to KillWall to Triggers.
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    private void Start()
    {
        defaultSpeed = moveSpeed;
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        // Move at a constant speed to the right
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

        //Rubberband();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[KillWall] Trigger with {other.name}");

        // Player judgment (filter by tag)
        if (other.CompareTag(playerTag))
        {

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
        if (other.CompareTag(enemyTag))
        {
            var enemy = other.GetComponent<EnemyBase>();
            enemy.Die(false);
        }
    }

    //void Rubberband()
    //{
    //    if((player.position.x - transform.position.x) > rubberbandDistance)
    //    {
    //        moveSpeed = 5f;
    //        print("Run: " + moveSpeed);
    //    }
    //    else
    //    {
    //        moveSpeed = defaultSpeed;
    //        print("Walk: " + moveSpeed);
    //    }
    //}
}
