using UnityEngine;
using System;

public class EnemyBase : MonoBehaviour, IDamageable
{
    [Tooltip("The enemies name")]
    public string enemyName;

    [Header("Enemy Stats")]
    [Tooltip("How much health the enemy starts with")]
    public float health;
    [Tooltip("Enemy Damage when not otherwise specified")]
    public int defaultDamage;
    [Tooltip("How fast does the enemy move")]
    public float moveSpeed;
    [Tooltip("How long between each attack")]
    public float attackSpeed;
    [Tooltip("How long is it stunned after being parried")]
    public float stunTime;
    [Tooltip("How much resistance does the enemy have to being knocked back by the player")]
    public float knockbackResist;

    [Header("Attacks")]
    [Tooltip("List of attacks the enemy can do")]
    public Attack[] attacks;

    [Serializable]
    public class Attack
    {
        [Tooltip("Attack identifier")]
        public string ID;
        [Tooltip("How much damage will the attack do")]
        public float damage;
        [Tooltip("how much force will the enemy knock the player back with")]
        public float knockback;
    }

    internal Rigidbody2D rb;
    internal float actingMoveSpeed;

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

    public virtual void TakeDamage(float damage)
    {
        if (EnemyManager.instance.LogEnemyDamage)
        {
            print(enemyName + " Has taken " + damage + " Damage");
        }

        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
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
        actingMoveSpeed = moveSpeed /20;

        ExtraSetup();
    }

    public virtual void ExtraSetup()
    {

    }
}
