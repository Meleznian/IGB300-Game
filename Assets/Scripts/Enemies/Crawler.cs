using UnityEngine;
using UnityEngine.UIElements;

public class Crawler : EnemyBase
{
    Vector3 moveDirection;

    void Update()
    {
        
    }

    public override void Move()
    {
        transform.position += moveDirection * actingMoveSpeed;
        //rb.AddForce(moveDirection * moveSpeed,  ForceMode2D.Impulse);
    }

    void ChangeDirection()
    {
        if (moveDirection == Vector3.left)
        {
            moveDirection = Vector3.right;
        }
        else
        {
            moveDirection = Vector3.left;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            ChangeDirection();
        }

        /*if (other.CompareTag("Player"))
        {
            ChangeDirection();

            //attack animation 

            Debug.Log("Player Hit!");
        }*/
    }

    

    public override void ExtraSetup()
    {
        moveDirection = Vector2.left;
    }
}
