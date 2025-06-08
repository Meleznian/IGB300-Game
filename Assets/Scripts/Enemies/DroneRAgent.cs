using System.Collections;
using UnityEngine;

public class DroneRAgent : BehaviourAgent
{
    [Header("Universal")]
    public float moveSpeed;
    public float minDistance;

    [Header("Attack differentiation")]
    //public float chargeDistance;
    public float yLevelError;
    public bool attacking;

    /*[Header("Charge Variables")]
    public float chargeSpeed;
    public int chargeDamage;
    public Knockback chargeKnockback;
    public int chargeCooldown;
    public bool chargeParriable;
    public bool chargeAvailable = true;
    public float chargeTravel;


    [Header("Contact Variables")]
    public int damageCooldown;
    public int contactDamage;
    public Knockback damageKnockback;*/

    /// <summary>
    /// Roam State of DFA Agent
    /// </summary>
    public override void Roam()
    {
        //Debug.Log("Roaming");
        //Will Require Navigation Agent 
        if (currentPath.Count <= 0)
        {
            currentPath = GreedySearch(currentNodeIndex, Random.Range(0, graphNodes.graphNodes.Count), currentPath);
            currentPath.Reverse();
            currentPath.RemoveAt(currentPath.Count - 1);
            return;
        }
        if (Vector2.Distance(transform.position, graphNodes.graphNodes[currentPath[currentPathIndex]].transform.position) <= minDistance)
        {
            //Debug.Log(currentPath.Count);
            if (currentPathIndex < currentPath.Count - 1) currentPathIndex++;
            else
            {
                int randomNode = Random.Range(0, graphNodes.graphNodes.Count);
                //Debug.Log(graphNodes.graphNodes.Length);
                //Debug.Log("Start");
                //Debug.Log(currentNodeIndex);
                //Debug.Log("New goal");
                //Debug.Log(randomNode);
                currentPath.Clear();
                greedyPaintList.Clear();
                currentPathIndex = 0;
                currentPath.Add(currentNodeIndex);

                currentPath = GreedySearch(currentPath[currentPathIndex], randomNode, currentPath);

                currentPath.Reverse();
                currentPath.RemoveAt(currentPath.Count - 1);
            }
        }
        Move();
    }

    /*
    /// <summary>
    /// Attack state of DFA Agent
    /// </summary>
    public override void Attack()
    {
        if (attacking) return;
        //Debug.Log("Correct Attack");
        //Check Charge eligibility
        if (Vector2.Distance(transform.position, target.transform.position) > chargeDistance && chargeAvailable)
        {
            StartCoroutine(Charge());
            currentPath.Clear();

        }
        else
        {
            //Move
            //Debug.Log("Moving");
            if (currentPath.Count > 0)
            {
                if (Vector2.Distance(transform.position, graphNodes.graphNodes[currentPath[currentPathIndex]].transform.position) <= minDistance)
                {
                    if (currentPathIndex < currentPath.Count - 1) currentPathIndex++;
                    else { currentPath = AStarSearch(currentNodeIndex, findClosestWayPoint(target)); currentPathIndex = 0; }
                    //else { currentPath.Clear(); currentPathIndex = 0; }
                }

                Move();
            }
            else { currentPath = AStarSearch(currentNodeIndex, findClosestWayPoint(target)); }
        }
    }

    */

    public override void Flee()
    {

    }

    /// <summary>
    /// Moves the NPC to the next node in its path
    /// </summary>
    public void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, graphNodes.graphNodes[currentPath[currentPathIndex]].transform.position, moveSpeed * Time.deltaTime);
        currentNodeIndex = graphNodes.graphNodes[currentPath[currentPathIndex]].GetComponent<LinkedNodes>().index;

    }

    public void Shoot()
    {

    }


}
