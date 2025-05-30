using UnityEngine;
using UnityEngine.UIElements;

public class Crawler : EnemyBase
{
    private Vector3 _moveDirection;
    private float _damageCooldown = 1f; // Time between damage ticks
    private float _lastDamageTime;

    [SerializeField] private LayerMask grounds;
    [SerializeField] Animator anim;

    public override void Move()
    {
        transform.position += _moveDirection * actingMoveSpeed;
        AudioManager.PlayEffect(SoundType.CRAWLER_WALK);
        CheckFloor();
        //rb.AddForce(moveDirection * moveSpeed,  ForceMode2D.Impulse);
    }

    internal void ChangeDirection()
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

    internal void CrawlerAttack(Collider2D other)
    {
        if (Time.time > _lastDamageTime + _damageCooldown)
        {
            other.GetComponent<PlayerHealth>().TakeDamage(defaultDamage);
            _lastDamageTime = Time.time;
            //Debug.Log("Continuous Damage!");
            AudioManager.PlayEffect(SoundType.CRAWLER_ATTACK);
        }
    }



    public override void ExtraSetup()
    {
        _moveDirection = Vector2.left;

        anim.SetBool("Moving", true);
    }


    void CheckFloor()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 1f, grounds);

        //Debug.DrawRay(transform.position, -transform.up, Color.red);

        if (hit.collider == null)
        {
            ChangeDirection();
        }
    }
}