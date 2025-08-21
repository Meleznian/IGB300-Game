
using UnityEngine;

public class FlyingEnemy : EnemyBase
{
    private Vector3 _moveDirection = Vector3.left;
    public float A, B, C;
    public override void Move()
    {
        transform.position += _moveDirection * actingMoveSpeed;
        transform.position = new Vector3(transform.position.x, A * Mathf.Sin(transform.position.x - B) + C, transform.position.z);
        transform.localScale = new Vector3(_moveDirection.x, 1, 1);

        if (transform.position.x < GameManager.instance.Player.transform.position.x) _moveDirection = Vector3.right;
        else _moveDirection = Vector3.left;
    }
}
