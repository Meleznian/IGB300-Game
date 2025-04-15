using System.Collections;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class ChargerAgent : BehaviourAgent
{
    [Header("Universal")]
    public float moveSpeed;
    public float minDistance;

    [Header("Attack differentiation")]
    public float chargeDistance;
    public float yLevelError;
    public bool attacking;

    [Header("Charge Variables")]
    public float chargeSpeed;
    public int chargeDamage;
    public Knockback chargeKnockback;
    public int chargeCooldown;
    public bool chargeParriable;
    public bool chargeAvailable = true;


    [Header("Bash Variables")]
    public int bashDamage;
    public Knockback bashKnockback;
    public int bashCooldown;
    public bool bashParriable;
    public bool bashAvailable = true;

   

    public override void Roam()
    {
       //Will Require Navigation Agent 
    }

    public override void Attack()
    {
        if (attacking) return;
        //Debug.Log("Correct Attack");
        //Check Charge eligibility
        if (transform.position.y >= target.transform.position.y - yLevelError && transform.position.y <= target.transform.position.y + yLevelError && Vector3.Distance(target.transform.position, transform.position) > chargeDistance && chargeAvailable)
        {
            StartCoroutine(Charge());

        }//Check Bash eligibility
        else if (Vector3.Distance(target.transform.position, transform.position) < chargeDistance && bashAvailable)
        {
            StartCoroutine(Bash());
        }
        else
        {
            //Move
            //Debug.Log("Moving");
            if(currentPath.Count > 0)
            {
               
                transform.position = Vector2.MoveTowards(transform.position, graphNodes.graphNodes[currentPath[currentPathIndex]].transform.position, moveSpeed * Time.deltaTime);

                if(Vector2.Distance(transform.position, graphNodes.graphNodes[currentPath[currentPathIndex]].transform.position) <= minDistance)
                {
                    if (currentPathIndex < currentPath.Count - 1) currentPathIndex++;
                    else { currentPath = AStarSearch(currentNodeIndex, findClosestWayPoint(target)); currentPathIndex = 0; }
                }
                currentNodeIndex = graphNodes.graphNodes[currentPath[currentPathIndex]].GetComponent<LinkedNodes>().index;
            }
            else { currentPath = AStarSearch(currentNodeIndex, findClosestWayPoint(target));}
        }
    }

    public IEnumerator Charge()
    {
        chargeAvailable = false;
        attacking = true;
        //Perform action
        Debug.Log("Charging");
        attacking = false;
        yield return new WaitForSeconds(chargeCooldown);
        chargeAvailable = true;
    }
    public IEnumerator Bash()
    {
        bashAvailable = false;
        attacking = true;
        //Perform action
        Debug.Log("Bashing");
        attacking = false;
        yield return new WaitForSeconds(bashCooldown);
        bashAvailable = true;
    }
}
