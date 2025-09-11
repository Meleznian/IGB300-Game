using UnityEngine;

public class HoverBoy : FlyingEnemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float targetY;
    [SerializeField] float maxDist;
    Vector2 target;

    //[SerializeField] float minSpeed;

    public override void Move()
    {
        Transform player = GameManager.instance.Player.transform;

        target = new Vector2(player.position.x, targetY);

        AdjustSpeed();

        transform.position = Vector2.MoveTowards(transform.position, target, actingMoveSpeed * Time.deltaTime);

        print("Enemy Position: " +transform.position +"\n Target Position: " + target + "\n Speed: " + actingMoveSpeed);
        print(rb.linearVelocity.magnitude);
    }

    void AdjustSpeed()
    {
        float dist = Vector2.Distance(transform.position, target);
        //print("Distance: " + dist);
        dist /= maxDist;
        actingMoveSpeed = (moveSpeed)*dist;
    }

    public override void Attack(PlayerHealth player)
    {
        print("Triggered: " + player.name);
        player.TakeDamage(defaultDamage);
        player.GetComponent<Rigidbody2D>().AddForce(Vector2.down*knockback, ForceMode2D.Impulse);
        Vector2 direction = ((Vector3.up)) * 4;
        rb.AddForce(direction, ForceMode2D.Impulse);
        canDamage = false;
        timer = 1;
    }
}
