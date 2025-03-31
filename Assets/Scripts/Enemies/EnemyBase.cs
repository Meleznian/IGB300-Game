using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float health;
    public float moveSpeed;
    public float attackSpeed;
    public float damage;
    public float stunTime;


    public virtual void Move()
    {

    }

    public virtual void Attack()
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
