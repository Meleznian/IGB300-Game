using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight_Trigger : MonoBehaviour
{

    //Entry state
    public State enter;
    //Exit state
    public State exit;

    //Enemy to update
    public BehaviourAgent NPC;
    //Player to look for
    public GameObject player;

    [SerializeField] private float MaxDistance = Mathf.Infinity;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("Running");

        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position, MaxDistance);
        if(ray.collider != null)
        {
            if (ray.collider.CompareTag("Player"))
            {
                NPC.target = player;
                ChangeEnemyStates(enter);
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red); 
                ChangeEnemyStates(exit);
                NPC.target = null;
            }
        }

        
        //Casts a ray to see if player is in line of sight
        /*RaycastHit2D hit;
        Vector2 targetDirection = player.transform.position - transform.position;
        Debug.Log(player.transform.position);
        LayerMask mask = LayerMask.GetMask("Ignore Raycast");
        hit = Physics2D.Raycast(transform.position, targetDirection, 40.0f, mask);
        Debug.Log(hit == true);
        if (hit)
        {
            Debug.Log("Hitting");
            Debug.DrawRay(transform.position, targetDirection, Color.white);
            Debug.Log(hit.collider.gameObject.name);

            //Checks if player hit
            if (hit.collider.gameObject.tag == "Player")
            {
                //Sets enemy state upon entry into line of sight
                NPC.target = player;
                ChangeEnemyStates(enter);
            }
            else
            {
                //Sets enemy state upon exit from line of sight
                ChangeEnemyStates(exit);
                NPC.target = null;
            }
        } else
        {
            Debug.DrawRay(transform.position, targetDirection, Color.white);
            Debug.Log("Did not Hit");
        }*/
    }

    //Updates enemy state
    private void ChangeEnemyStates(State state)
    {
        NPC.currentState = state;
    }
}
