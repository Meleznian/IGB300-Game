using System.Collections;
using UnityEngine;

public class RunningEnemy : EnemyBase
{
    private Vector3 _moveDirection = Vector3.left;
    public override void Move()
    {
        transform.position += _moveDirection * actingMoveSpeed;
        transform.localScale = new Vector3(_moveDirection.x, 1, 1);

        if (transform.position.x < GameManager.instance.Player.transform.position.x) _moveDirection = Vector3.right;
        else _moveDirection = Vector3.left;
    }

    bool canDamage;
    float timer;

    private void Update()
    {
        if (!canDamage)
        {
            Cooldown();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (canDamage)
        {
            var player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                print("Triggered: " + other.name);
                player.TakeDamage(defaultDamage);
                Vector2 direction = ((-_moveDirection + Vector3.up)) * 4;
                rb.AddForce(direction, ForceMode2D.Impulse);
                canDamage = false;
                timer = 1;
            }
        }
    }

    void Cooldown()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            canDamage = true;
        }
    }
}
