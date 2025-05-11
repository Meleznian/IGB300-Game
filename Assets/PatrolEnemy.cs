using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{

    public float moveSpeed = 2f;
    public Transform checkPoint;
    public float distance = 1f;
    public LayerMask layerMask;

    //public bool isGround = false;
    public bool facingLeft = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
        else if (hit == false && facingLeft == false)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingLeft = true;
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
    }


}
