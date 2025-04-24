using UnityEngine;
using UnityEngine.UIElements;

public class Crawler : EnemyBase
{
    private Vector3 _moveDirection;
    private float _damageCooldown = 1f; // Time between damage ticks
    private float _lastDamageTime;

    [SerializeField] private LayerMask grounds;

    public override void Move()
    {
        transform.position += _moveDirection * actingMoveSpeed;

        CheckFloor();
        //rb.AddForce(moveDirection * moveSpeed,  ForceMode2D.Impulse);
    }

    void ChangeDirection()
    {
        if (_moveDirection == Vector3.left)
        {
            _moveDirection = Vector3.right;
        }
        else
        {
            _moveDirection = Vector3.left;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            ChangeDirection();
        }
        else if (other.CompareTag("Bullet"))
        {

            Destroy(gameObject); // Destroy the crawler when hit by bullet
            //Debug.Log("bullet hit Crawler");
            FindFirstObjectByType<GameManager>().KillCount();
        }

    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time > _lastDamageTime + _damageCooldown)
            {
                other.GetComponent<PlayerHealth>().TakeDamage(33);
                _lastDamageTime = Time.time;
                //Debug.Log("Continuous Damage!");
            }
        }

    }

    


    public override void ExtraSetup()
    {
        _moveDirection = Vector2.left;
    }


    void CheckFloor()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 1f, grounds);

        //Debug.DrawRay(transform.position, -transform.up, Color.red);

        if(hit.collider == null)
        {
            ChangeDirection();
        }
    }

}
