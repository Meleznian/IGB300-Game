//using System;
using System.Collections;
using UnityEngine;

public class KamikazeAgent : BehaviourAgent
{
    [Header("Universal")]
    public float moveSpeed;
    public float minDistance;

    [Header("Attack differentiation")]
    public float chargeDistance;
    public float chargeAdditionalTravel;
    public float yLevelError;
    public bool attacking;

    [Header("Charge Variables")]
    public float chargeSpeed;
    public int chargeDamage;
    public float chargeKnockback;
    public int chargeCooldown;
    public bool chargeParriable;
    public bool chargeAvailable = true;


    [Header("Explosion Variables")]
    public int explodeDamage;
    public float explodeKnockback;
    public float explodeRadius;
    public float explodingSpeed;
    bool exploding;
    public LayerMask layersToHit;

    [SerializeField] GameObject explosionEffect;


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
        if (transform.position.y >= target.transform.position.y - yLevelError && transform.position.y <= target.transform.position.y + yLevelError && Vector3.Distance(target.transform.position, transform.position) > chargeDistance && chargeAvailable)
        {
            StartCoroutine(Charge());
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
            else { currentPath = AStarSearch(currentNodeIndex, findClosestWayPoint(target));}
        }
    }

    /// <summary>
    /// Moves the NPC to the next node in its path
    /// </summary>
    public void Move()
    {
        if (!exploding)
        {
            transform.position = Vector2.MoveTowards(transform.position, graphNodes.graphNodes[currentPath[currentPathIndex]].transform.position, moveSpeed * Time.deltaTime);
            currentNodeIndex = graphNodes.graphNodes[currentPath[currentPathIndex]].GetComponent<LinkedNodes>().index;
        }
        else
        {
            if (GameManager.instance.Player.transform.position.x >= transform.position.x)
            {
                transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);

            }
            else
            {
                transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
            }
        }
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
        //Debug.Log("Charging");
        anim.SetBool("Charging",true);

        float targetXPos = target.transform.position.x;

        //if (target.transform.position.x > transform.position.x)
        //{
        //    targetXPos += chargeAdditionalTravel;
        //}
        //else
        //{
        //    targetXPos -= chargeAdditionalTravel;
        //}

        bool cancelled = false;
        //move towards targetXPos
        while (Vector2.Distance(transform.position, new Vector2(targetXPos, transform.position.y)) >= minDistance && !cancelled)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetXPos, transform.position.y), chargeSpeed * Time.deltaTime);
            yield return null;
        }
        //Debug.Log("Charge End");

        moveSpeed = explodingSpeed;
        exploding = true;
        anim.SetTrigger("Explode");

        attacking = false;
        yield return new WaitForSeconds(chargeCooldown);
        chargeAvailable = true;
    }

    /// <summary>
    /// Enemy Bash attack
    /// </summary>
    /// <returns>N/A</returns>
    public void Explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        var hits = Physics2D.OverlapCircleAll(transform.position, explodeRadius, layersToHit); 
        foreach (var h in hits)
        {
            print(h.gameObject.name);
            var player = h.GetComponent<IDamageable>();

            if (player != null)
            {
                //print("Player Exploded");
                player.TakeDamage(explodeDamage);

                Vector2 knockDirection = transform.position - h.transform.position;
                knockDirection = knockDirection.normalized;

                if (h.GetComponent<Rigidbody2D>() != null)
                {
                    h.GetComponent<Rigidbody2D>().AddForce(knockDirection * explodeKnockback, ForceMode2D.Impulse);
                }
            }
        }


        TakeDamage(health);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explodeRadius);
    }
}
