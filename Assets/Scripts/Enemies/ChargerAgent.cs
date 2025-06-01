//using System;
using System.Collections;
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


    private bool isBashing = false;

    /// <summary>
    /// Roam State of DFA Agent
    /// </summary>
    public override void Roam()
    {
        //Debug.Log("Roaming");
        //Will Require Navigation Agent 
        if (currentPath.Count <= 0) {

            currentPath.Clear();
            greedyPaintList.Clear();

            currentPathIndex = 0;
            currentPath.Add(currentNodeIndex); 

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
        if (transform.position.y >= target.transform.position.y - yLevelError && transform.position.y <= target.transform.position.y + yLevelError && Vector3.Distance(target.transform.position, transform.position) > chargeDistance && chargeAvailable)
        {
            StartCoroutine(Charge());
            currentPath.Clear();

        }//Check Bash eligibility
        else if (Vector3.Distance(target.transform.position, transform.position) < chargeDistance && bashAvailable)
        {
            StartCoroutine(Bash());
            currentPath.Clear();
        }
        else
        {
            //Move
            //Debug.Log("Moving");
            if(currentPath.Count > 0)
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
            else { currentPathIndex = 0; currentPath = AStarSearch(currentNodeIndex, findClosestWayPoint(target));}
        }
    }

    /// <summary>
    /// Moves the NPC to the next node in its path
    /// </summary>
    public void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, graphNodes.graphNodes[currentPath[currentPathIndex]].transform.position, moveSpeed * Time.deltaTime);
        currentNodeIndex = graphNodes.graphNodes[currentPath[currentPathIndex]].GetComponent<LinkedNodes>().index;

        if (isBashing) return;
        if (attacking) return;
        //AudioManager.PlayEffect(SoundType.CHARGER_WALK);
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
        anim.SetBool("Charging", true);
        //AudioManager.PlayEffect(SoundType.CHARGER_CHARGING);

        float targetXPos = target.transform.position.x;
        bool cancelled = false;
        //move towards targetXPos
        while (Vector2.Distance(transform.position, new Vector2(targetXPos, transform.position.y)) >= minDistance && !cancelled)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetXPos, transform.position.y), chargeSpeed * Time.deltaTime);
            yield return null;
        }
        Debug.Log("Charge End");
        DealDamage(attackPoint.position, chargeDamage);
        anim.SetBool("Charging", false);

        attacking = false;
        yield return new WaitForSeconds(chargeCooldown);
        chargeAvailable = true;
    }

    /// <summary>
    /// Enemy Bash attack
    /// </summary>
    /// <returns>N/A</returns>
    public IEnumerator Bash()
    {
        isBashing = true;

        bashAvailable = false;
        attacking = true;
        anim.SetTrigger("Bash");
        while (attacking)
        {
            yield return null;
        }
        yield return new WaitForSeconds(bashCooldown);
        bashAvailable = true;
        AudioManager.PlayEffect(SoundType.CHARGER_ATTACK);

        isBashing = false;
    }

}
