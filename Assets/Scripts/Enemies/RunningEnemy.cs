using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunningEnemy : EnemyBase
{
    private Vector3 _moveDirection = Vector3.left;
    [SerializeField] bool canJump;
    [SerializeField] float jumpForce;
    [SerializeField] float knockback = 2f;
    public override void Move()
    {
        transform.position += _moveDirection * actingMoveSpeed;
        transform.localScale = new Vector3(_moveDirection.x, 1, 1);

        if (transform.position.x < GameManager.instance.Player.transform.position.x) _moveDirection = Vector3.right;
        else _moveDirection = Vector3.left;

        Jump();
    }

    bool canDamage;
    float timer;

    private void Update()
    {
        if (!canDamage)
        {
            Cooldown();
        }

        CheckWall();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerHealth>();
        var knockbackApply = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            if (canDamage)
            {
                print("Triggered: " + other.name);
                knockbackApply.ApplyKnockbackFrom(transform.position, knockback);
                player.TakeDamage(defaultDamage);
                Vector2 direction = ((-_moveDirection + Vector3.up)) * 6;
                rb.AddForce(direction, ForceMode2D.Impulse);
                canDamage = false;
                timer = 1;
            }
            rb.AddForce(-_moveDirection/4, ForceMode2D.Impulse);

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

    float jumpTimer;
    void Jump()
    {
        if (canJump)
        {
            if(jumpTimer <= 0)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jumpTimer = UnityEngine.Random.Range(1.0f,2.0f);
            }

            jumpTimer -= Time.deltaTime;
        }
    }
}
