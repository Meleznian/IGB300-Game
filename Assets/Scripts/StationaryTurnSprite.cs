using UnityEngine;

public class StationaryTurnSprite : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Transform player;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        Turn();
    }

    void Turn()
    {
        if (player.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (player.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
