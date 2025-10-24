using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public bool hoverer;

    Rigidbody2D rb;
    Collider2D c;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        StartCoroutine(CollectDelay());
        //parent = transform.parent;
        rb = GetComponent<Rigidbody2D>();
        c = GetComponent<Collider2D>();

        if (hoverer)
        {
            rb.gravityScale = 0.0f;
        }
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

        if (!tagged && hoverer)
        {
            Move();
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
                AudioManager.PlayEffect(SoundType.NUKE);
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
                AudioManager.PlayEffect(SoundType.MONEY);
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canBeCollected)
        {
            Collect();
        }
    }

    IEnumerator CollectDelay()
    {
        yield return new WaitForSeconds(collectionDelay);
        canBeCollected = true;
    }

    [Header("Float In")]
    internal Vector3 _moveDirection = Vector3.left;
    public float A, B, C;
    public float speed;

    public void Move()
    {
        transform.position += _moveDirection * (speed*Time.deltaTime);
        transform.position = new Vector3(transform.position.x, A * Mathf.Sin(transform.position.x - B) + C, transform.position.z);
    }
}
