using UnityEngine;

public class MagnetBolt : MonoBehaviour
{
    public float MagnetRange = 3f;
    public float MagnetSpeed = 4f;
    public int value;
    public Transform player;
    public bool inRange = false;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
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

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            AudioManager.PlayEffect(SoundType.BOLTS, 0.2f);
        }
    }
}
