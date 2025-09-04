using UnityEngine;
using System.Collections;

public class Bolt : MonoBehaviour
{
    public float MagnetRange = 3f;
    public float MagnetSpeed = 4f;
    public int value;
    public Transform player;
    public bool inRange = false;
    [SerializeField] float collectionDelay;
    bool canBeCollected;
    bool tagged;
    bool floating;

    Rigidbody2D rb;
    CircleCollider2D c;
    //Transform parent;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        StartCoroutine(CollectDelay());
        //parent = transform.parent;
        rb = GetComponent<Rigidbody2D>();
        c = transform.GetChild(0).GetComponent<CircleCollider2D>();
    }
    void Update()
    {

        float distance = Vector2.Distance(transform.position, player.position);
        inRange = distance <= MagnetRange;

        if (inRange)
        {
            tagged = true;
        }

        if (canBeCollected)
        {

            if (tagged)
            {
                if (!floating)
                {
                    rb.gravityScale = 0;
                    c.excludeLayers |= (1 << 6);
                    floating = true;
                }

                transform.position = Vector2.MoveTowards(transform.position, player.position, MagnetSpeed * Time.deltaTime);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, MagnetRange);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //print("Money Triggered");
        if (other.CompareTag("Wall"))
        {
            AudioManager.PlayEffect(SoundType.BOLTS, 0.2f);
        }

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canBeCollected)
        {
            GameManager.instance.BoltCount(value);
            ScoreManager.instance.AddScore(50, transform.position);
            AudioManager.PlayEffect(SoundType.COLLECT_BOLT, 1f);
            Destroy(gameObject);
        }
    }

    IEnumerator CollectDelay()
    {
        yield return new WaitForSeconds(collectionDelay);
        canBeCollected = true;
    }
}
