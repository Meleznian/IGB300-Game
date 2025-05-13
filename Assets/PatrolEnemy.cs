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
            if(player.position.x > transform.position.x)
            {
                facingLeft = false;
            }

            if (Vector2.Distance(transform.position, player.position) > retrieveDistance)
            {
                animator.SetBool("Attack1", false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack1", true);

                Debug.Log("Attack");
            }
            //Debug.Log("Player in Range");
        }
        else
        {
            transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

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

    private void OnDrawGizmosSelected()
    {
        if(checkPoint == null)//if not 
        {
            return;
        }
        Gizmos.color = Color.yellow; 
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


}
