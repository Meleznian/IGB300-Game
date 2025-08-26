using UnityEngine;

public class Bolt : MonoBehaviour
{
    public float MagnetRange = 3f;
    public float MagnetSpeed = 4f;
    public int value;
    public Transform player;
    public bool inRange = false;
    //Transform parent;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        //parent = transform.parent;
    }
    void Update()
    {

        float distance = Vector2.Distance(transform.position, player.position);
        inRange = distance <= MagnetRange;

        if (inRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, MagnetSpeed * Time.deltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, MagnetRange);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Money Triggered");
        if (other.CompareTag("Wall"))
        {
            AudioManager.PlayEffect(SoundType.BOLTS, 0.2f);
        }
        if(other.CompareTag("Player"))
        {
            GameManager.instance.BoltCount(value);
            ScoreManager.instance.AddScore(50, transform.position);
            AudioManager.PlayEffect(SoundType.COLLECT_BOLT, 1f);
            Destroy(gameObject);
        }
    }
}
