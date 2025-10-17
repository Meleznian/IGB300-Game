using UnityEngine;
using System;
using System.Data.SqlTypes;

public class EnemyBase : MonoBehaviour, IDamageable
{
    [Tooltip("The enemies name")]
    public string enemyName;

    [Header("Enemy Stats")]
    [Tooltip("How much health the enemy starts with")]
    public int health;
    [Tooltip("Enemy Damage when not otherwise specified")]
    public int defaultDamage;
    [Tooltip("How fast does the enemy move")]
    public float moveSpeed;
    public float speedVariance = 1;

    //[Tooltip("How long between each attack")]
    //public float attackSpeed;
    //[Tooltip("How long is it stunned after being parried")]
    //public float stunTime;
    //[Tooltip("How much resistance does the enemy have to being knocked back by the player")]
    //public float knockbackResist;


    [Header("Currency Drop")]
    //[Tooltip("Prefab to spawn when the enemy dies")]
    //public GameObject currencyPrefab;
    public float PickupDropPercent = 0.02f;

    [Tooltip("Number of currency drops to spawn")]
    [SerializeField] float value;
    //public GameObject[] money;

    //[Header("Attacks")]
    //[Tooltip("List of attacks the enemy can do")]
    //public Attack[] attacks;
    //
    //[Serializable]
    //public class Attack
    //{
    //    [Tooltip("Attack identifier")]
    //    public string ID;
    //    [Tooltip("How much damage will the attack do")]
    //    public float damage;
    //    [Tooltip("how much force will the enemy knock the player back with")]
    //    public float knockback;
    //}



    internal Rigidbody2D rb;
    internal float actingMoveSpeed;
    //[SerializeField] ParticleSystem damageEffect;



    private void Start()
    {
        SetUp();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public virtual void Move()
    {

    }

    public virtual void Attack(PlayerHealth player)
    {

    }

    public virtual void Parried()
    {

    }

    public virtual void TakeDamage(int damage)
    {
        //if (LegacyEnemyManager.instance != null && LegacyEnemyManager.instance.LogEnemyDamage)
        //{
        //    print(enemyName + " Has taken " + damage + " Damage");
        //}

        health -= damage;
        EnemyManager.instance.EnemyHurt(transform.position);

        if (health <= 0)
        {
            Die(true);
        }
    }

    public virtual void Die(bool spawn)
    {
        Debug.Log("Die here");

        if (spawn)
        {
            SpawnCurrency();
            if (UnityEngine.Random.Range(0f, 1f) < PickupDropPercent) SpawnPickup();
            if (GameManager.instance.tutorial == false)
            {
                ScoreManager.instance.AddScore(200, transform.position);
            }
            GameManager.instance.KillCount();
        }

        AudioManager.PlayEffect(SoundType.ENEMY_DEATH);

        if (EnemyManager.instance != null)
        {
            
            EnemyManager.instance.EnemyKilled(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    Transform killWall;
    void SetUp()
    {
        rb = GetComponent<Rigidbody2D>();

        moveSpeed += UnityEngine.Random.Range(-speedVariance, speedVariance);

        actingMoveSpeed = moveSpeed /20;

        if (GameManager.instance.tutorial == false)
        {
            killWall = GameManager.instance.killWall.transform.GetChild(0);
        }

        ExtraSetup();
    }

    public virtual void ExtraSetup()
    {

    }

    void SpawnCurrency()
    {

        //if (currencyPrefab == null) return;
        //
        //Debug.Log("Drop bolt");
        //
        //for (int i = 0; i < dropAmount; i++)
        //{
        //    // Random offset to spread them a bit
        //    Vector2 spawnOffset = UnityEngine.Random.insideUnitCircle * 0.5f;
        //    Instantiate(currencyPrefab, transform.position + (Vector3)spawnOffset, Quaternion.identity);
        //}

        GameObject[] money = GameManager.instance.GenerateMoney(value);
        
        Debug.Log("Drop bolt");
        
        foreach (GameObject g in money)
        {
            // Random offset to spread them a bit
            Vector2 spawnOffset = UnityEngine.Random.insideUnitCircle * 0.5f;
            Rigidbody2D rb = Instantiate(g, transform.position + (Vector3)spawnOffset, Quaternion.identity).GetComponent<Rigidbody2D>();

            Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f));
            rb.AddForce(direction*5, ForceMode2D.Impulse);
        }
    }

    internal void CheckWall()
    {
        if (killWall != null)
        {
            if (killWall.position.x > transform.position.x)
            {
                Die(false);
            }
        }
    }

    void SpawnPickup()
    {
        Vector2 spawnOffset = UnityEngine.Random.insideUnitCircle * 0.5f;
        GameObject pickup = GameManager.instance.GetPickup();
        Vector3 spawnPos;

        if(pickup.GetComponent<PickupScript>().hoverer == true)
        {
            spawnPos = EnemyManager.instance.transform.position;
            spawnPos += new Vector3(0, UnityEngine.Random.Range(-5, 5),0);
        }
        else
        {
            spawnPos = transform.position;
        }

        Rigidbody2D rb = Instantiate(pickup, spawnPos + (Vector3)spawnOffset, Quaternion.identity).GetComponent<Rigidbody2D>();

        Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        rb.AddForce(direction * 5, ForceMode2D.Impulse);
    }
}
