using UnityEngine;
using System;

public class EnemyBase : MonoBehaviour
{
    [ToolTip("The enemies name")]
    public string enemyName;

    [Header("Enemy Stats")]
    [ToolTip("How much health the enemy starts with")]
    public float health;
    [ToolTip("How fast does the enemy move")]
    public float moveSpeed;
    [ToolTip("How long between each attack")]
    public float attackSpeed;
    [ToolTip("How long is it stunned after being parried")]
    public float stunTime;
    [ToolTip("How much resistance does the enemy have to being knocked back by the player")]
    public float knockbackResist;

    [Header("Attacks")]
    [ToolTip("List of attacks the enemy can do")]
    public Attack[] attacks;

    [Serializable]
    public class Attack
    {
        [ToolTip("Attack identifier")]
        public string ID;
        [ToolTip("How much damage will the attack do")]
        public float damage;
        [ToolTip("how much force will the enemy knock the player back with")]
        public float knockback;
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

    public void TakeDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {

    }
}
