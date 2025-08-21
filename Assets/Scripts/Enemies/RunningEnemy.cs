using System.Security.Cryptography;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.gameObject.GetComponent<PlayerHealth>();
        if (player != null)
        {
            print("Triggered: " + other.gameObject.name);
            player.TakeDamage(defaultDamage);
            Vector2 direction = ((-_moveDirection + Vector3.up)*actingMoveSpeed)*10;
            rb.AddForce(direction, ForceMode2D.Impulse);
        }
    }
}
