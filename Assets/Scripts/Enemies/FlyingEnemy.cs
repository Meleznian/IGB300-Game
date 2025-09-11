
using UnityEngine;
using System.Collections;

public class FlyingEnemy : EnemyBase
{
    internal Vector3 _moveDirection = Vector3.left;
    public float A, B, C;
    [SerializeField] internal float knockback = 2f;
    [SerializeField] float offsetRange;
    [SerializeField] float heightRandomness;

    float offset = 1;
    public override void Move()
    {
        transform.position += _moveDirection * actingMoveSpeed;
        transform.position = new Vector3(transform.position.x, A * Mathf.Sin(transform.position.x - B) + C, transform.position.z);
        transform.localScale = new Vector3(_moveDirection.x, 1, 1);

        if (transform.position.x < GameManager.instance.Player.transform.position.x + offset) _moveDirection = Vector3.right;
        else _moveDirection = Vector3.left;
        SetTarget();
    }

    internal bool canDamage;
    internal float timer;

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
        if (canDamage)
        {
            var player = other.GetComponent<PlayerHealth>();

            if (player != null)
            {
                Attack(player);
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

    void SetTarget()
    {
        if(_moveDirection == Vector3.right)
        {
            offset = offsetRange;
        }
        else
        {
            offset = -offsetRange;
        }       
    }

    public override void ExtraSetup()
    {
        C += UnityEngine.Random.Range(-heightRandomness, heightRandomness);
    }

    public override void Attack(PlayerHealth player)
    {
        var knockbackApply = player.GetComponent<PlayerMovement>();
        print("Triggered: " + player.name);
        player.TakeDamage(defaultDamage);
        knockbackApply.ApplyKnockbackFrom(transform.position + new Vector3(1f, 0, 0), knockback);
        Vector2 direction = ((-_moveDirection + Vector3.up)) * 4;
        rb.AddForce(direction, ForceMode2D.Impulse);
        canDamage = false;
        timer = 1;
    }
}
