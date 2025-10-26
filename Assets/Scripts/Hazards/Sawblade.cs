using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Sawblade : Spike
{

    [SerializeField] float speed;
    [SerializeField] float horizontalRange;
    //[SerializeField] float maxDist;
 
    float actingSpeed;

    [SerializeField] bool right;
    Vector3 leftTarget;
    Vector3 rightTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftTarget = transform.position + new Vector3(horizontalRange, 0,0);
        rightTarget = transform.position - new Vector3(horizontalRange, 0, 0);
        actingSpeed = speed;
    }
 

    void Move()
    {
        if (right)
        {
            transform.position = Vector3.MoveTowards(transform.position, rightTarget, actingSpeed*Time.deltaTime);
            //AdjustSpeed();

            if (transform.position.x <= rightTarget.x)
            {
                //print(Vector3.Distance(transform.position, rightTarget));
                right = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, leftTarget, actingSpeed * Time.deltaTime);
            //AdjustSpeed();

            if (transform.position.x >= leftTarget.x)
            {
                //print(Vector3.Distance(transform.position, leftTarget));
                right = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x-horizontalRange, transform.position.y, 0) , 0.5f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x+horizontalRange, transform.position.y, 0), 0.5f);
    }

    //void AdjustSpeed()
    //{
    //    float dist; 
    //    float leftDist = Vector2.Distance(transform.position, leftTarget);
    //    float rightDist = Vector2.Distance(transform.position, rightTarget);
    //
    //    if (leftDist > rightDist)
    //    {
    //        dist = rightDist;
    //    }
    //    else
    //    {
    //        dist = leftDist;
    //    }
    //    //print("Distance: " + dist);
    //    dist /= maxDist;
    //    float actingSpeed = (speed) * dist;
    //}

    public override void DoUpdate()
    {
        if (!canDamage)
        {
            Cooldown();
        }
        Move();
    }
}