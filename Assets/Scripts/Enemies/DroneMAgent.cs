using System.Collections;
using UnityEngine;

public class DroneMAgent : BehaviourAgent
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
    public float chargeTravel;


    [Header("Contact Variables")]
    public int damageCooldown;
    public int contactDamage;
    public Knockback damageKnockback;

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
                /*transform.position = Vector2.MoveTowards(transform.position, graphNodes.graphNodes[currentPath[currentPathIndex]].transform.position, moveSpeed * Time.deltaTime);

                if(Vector2.Distance(transform.position, graphNodes.graphNodes[currentPath[currentPathIndex]].transform.position) <= minDistance)
                {
                    if (currentPathIndex < currentPath.Count - 1) currentPathIndex++;
                    else { currentPath = AStarSearch(currentNodeIndex, findClosestWayPoint(target)); currentPathIndex = 0; }
                }
                currentNodeIndex = graphNodes.graphNodes[currentPath[currentPathIndex]].GetComponent<LinkedNodes>().index;*/
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

    /// <summary>
    /// Moves the NPC to the next node in its path
    /// </summary>
    public void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, graphNodes.graphNodes[currentPath[currentPathIndex]].transform.position, moveSpeed * Time.deltaTime);
        currentNodeIndex = graphNodes.graphNodes[currentPath[currentPathIndex]].GetComponent<LinkedNodes>().index;

    }


    /// <summary>
    /// Enemy Charge attack
    /// </summary>
    /// <returns>N/A</returns>
    public IEnumerator Charge()
    {
        chargeAvailable = false;
        attacking = true;
        //Perform action
        Debug.Log("Charging");
        parriable = true;
        Vector2 targetPos = target.transform.position - transform.position;
        targetPos /= targetPos.magnitude;
        targetPos *= chargeTravel;
        bool cancelled = false;
        //move towards targetXPos
        while (Vector2.Distance(transform.position, targetPos) >= minDistance && !cancelled)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, chargeSpeed * Time.deltaTime);
            yield return null;
        }
        Debug.Log("Charge End");
        parriable = false;
        attacking = false;
        yield return new WaitForSeconds(chargeCooldown);
        chargeAvailable = true;
    }



}
