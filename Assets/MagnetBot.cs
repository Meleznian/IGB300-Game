using UnityEngine;

public class MagnetBot : MonoBehaviour
{
    public float MagnetRange = 3f;
    public float MagnetSpeed = 4f;
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
}
