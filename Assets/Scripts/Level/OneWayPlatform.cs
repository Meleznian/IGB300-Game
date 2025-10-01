using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] BoxCollider2D c;
    public bool solid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        c = GetComponent<BoxCollider2D>();
        player = GameManager.instance.Player.transform;
        StartCheck();
    }

    // Update is called once per frame
    void Update()
    {

        if (player == null)
        {
            player = GameManager.instance.Player.transform;
            StartCheck();
        }

        CheckPlayerPosition();

    }

    void CheckPlayerPosition()
    {
        if (player.position.y - 0.8f > transform.position.y && !solid)
        {
            c.excludeLayers &= ~(1 << 3);
            solid = true;
        }
        else if(player.position.y - 0.8f <= transform.position.y && solid)
        {
            c.excludeLayers |= (1 << 3);

            solid = false;
        }
    }

    internal void StartCheck()
    {
        if (player.position.y - 0.9f > transform.position.y)
        {
            c.excludeLayers &= ~(1 << 3);
            solid = true;
        }
        else if (player.position.y - 0.9f <= transform.position.y)
        {
            c.excludeLayers |= (1 << 3);

            solid = false;
        }
    }

}
