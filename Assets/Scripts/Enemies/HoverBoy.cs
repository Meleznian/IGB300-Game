using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverBoy : FlyingEnemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float targetY;
    Vector3 target;

    //[SerializeField] float minSpeed;

    public override void Move()
    {
        Transform player = player = GameManager.instance.Player.transform;

        target = new Vector3(player.position.x, targetY, player.position.z);

        _moveDirection = target - transform.position;

        _moveDirection = new Vector3(_moveDirection.x, _moveDirection.y, _moveDirection.z);

        AdjustSpeed();

        transform.position += _moveDirection * actingMoveSpeed;

        print(_moveDirection * actingMoveSpeed);
    }

    void AdjustSpeed()
    {
        actingMoveSpeed = Vector3.Distance(transform.position, target);
        actingMoveSpeed = Mathf.Clamp(actingMoveSpeed, 0.01f, moveSpeed/20);
    }
}
