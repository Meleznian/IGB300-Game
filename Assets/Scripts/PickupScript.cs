using UnityEngine;
using System.Collections;

public class PickupScript : MonoBehaviour, ICollectable
{
    [SerializeField] ParticleSystem particleEffect;

    public float MagnetRange = 3f;
    public float MagnetSpeed = 4f;
    public Transform player;
    public bool inRange = false;
    [SerializeField] float collectionDelay;
    bool canBeCollected;
    bool tagged;
    bool floating;

    Rigidbody2D rb;
    CircleCollider2D c;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        StartCoroutine(CollectDelay());
        //parent = transform.parent;
        rb = GetComponent<Rigidbody2D>();
        c = GetComponent<CircleCollider2D>();
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

    public int pickupType = 0;
    public void Collect()
    {
        switch (pickupType)
        {
            case 1:
                //Heal
                PlayerHealth healScript = GameManager.instance.Player.GetComponent<PlayerHealth>();
                healScript.Heal((int)(healScript.maxHealth * 0.4));
                break;
            case 2:
                //Nuke
                Instantiate(particleEffect, GameManager.instance.Player.transform.position, Quaternion.identity);
                for (int i = 0; i < EnemyManager.instance.livingEnemiesList.Count; i++)
                {
                    EnemyManager.instance.livingEnemiesList[0].GetComponent<EnemyBase>().Die(true);
                }
                break;
            case 3:
                //Slow Kill Wall
                GameManager.instance.killWall.gameObject.GetComponent<KillWall>().SlowKillWallPickup();
                break;
            case 4:
                //Money
                Instantiate(particleEffect, GameManager.instance.Player.transform.position, Quaternion.identity);
                GameManager.instance.BoltCount((float)(GameManager.instance.gameObject.GetComponent<UpgradeManager>().cashGoal * 0.3));
                break;
            case 5:
                //SuperCharge
                GameManager.instance.Player.GetComponent<PlayerMeleeAttack>().RunSuperCharge();
                break;
            default:
                Debug.Log("Not Implemented Exception");
                break;
        }

        Destroy(gameObject);
    }

    IEnumerator CollectDelay()
    {
        yield return new WaitForSeconds(collectionDelay);
        canBeCollected = true;
    }
}
