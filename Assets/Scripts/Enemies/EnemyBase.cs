using UnityEngine;
using System;

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

    [Tooltip("Number of currency drops to spawn")]
    public GameObject[] money;

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

    public virtual void DoAttack()
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
            ScoreManager.instance.AddScore(200, transform.position);
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


    void SetUp()
    {
        rb = GetComponent<Rigidbody2D>();

        moveSpeed += UnityEngine.Random.Range(-speedVariance, speedVariance);

        actingMoveSpeed = moveSpeed /20;

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
        
        Debug.Log("Drop bolt");
        
        foreach (GameObject g in money)
        {
            // Random offset to spread them a bit
            Vector2 spawnOffset = UnityEngine.Random.insideUnitCircle * 0.5f;
            Instantiate(g, transform.position + (Vector3)spawnOffset, Quaternion.identity);
        }
    }
}
