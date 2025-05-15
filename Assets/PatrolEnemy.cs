using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{

    public float moveSpeed = 2f;
    public Transform checkPoint;
    public float distance = 1f;
    public LayerMask layerMask;

    //public bool isGround = false;
    public bool facingLeft = true;

    public bool inRange = false;
    public Transform player;
    public float attackRange = 3f;
    public float retrieveDistance = 1f;
    public float chaseSpeed = 4f;
    public Animator animator;

    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer;

    public int damage = 34;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            inRange = true;
        }
        else
        {
            inRange = false; 
        }

        if (inRange)
        {
            if(player.position.x > transform.position.x && facingLeft == true)//player on the right side
            {//player is right side of the enemy and face left 
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;
            }
            else if (player.position.x < transform.position.x && facingLeft == false)//player on the left side
            {//player is left side of the enemy and face right 
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }

            if (Vector2.Distance(transform.position, player.position) > retrieveDistance)//within attack range
            {
                animator.SetBool("Attack1", false);//call animation. Animation is in Bool
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack1", true);

                //Debug.Log("Attack");
            }
            //Debug.Log("Player in Range");
        }
        else
        {//Patrol and walking
            transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

            //use Raycast
            RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);

            if (hit == false && facingLeft)
            {
                //isGround = true; 
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;
                //Debug.Log("Flip Enemy");
            }
            else if (hit == false && facingLeft == false)//flip
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }
        }
    }

    public void Attack()//call by animator in attack animation 
    {
        //make a circle hitbox 
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);//return bool. True/False

        if (collInfo)
        {
            //Debug.Log(collInfo.transform.name);

            PlayerHealth health = collInfo.gameObject.GetComponent<PlayerHealth>();//wot!? Play

            if(health != null)
            {
                health.TakeDamage(damage);
            }

        }
    }

    

    private void OnDrawGizmosSelected()//Raycast and Gizmos debugging. It really help!! 
    {
        if(checkPoint == null)//if not 
        {
            return;
        }
        Gizmos.color = Color.yellow; 
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);


        if (attackPoint == null) return;//check if attackPoint exist.. if not the rest of the code won't run
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }


}
