using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourAgent : NavigationAgent
{
    public State currentState;
    private State oldState;
    public GameObject target;
    public bool stunned;
    public float stunTime;

    public void Start()
    {
        currentNodeIndex = findClosestWayPoint(gameObject);
        
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Roam:
                Roam();
                break;
            case State.Attack:
                Attack();
                break;
        }

        if(currentState != oldState)
        {
            currentPath.Clear();
            greedyPaintList.Clear();
            currentPathIndex = 0;

            oldState = currentState;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            List<int> integers = new List<int>
        {
            10
        };
            GreedySearch(10, 3, integers);
        }
    }

    public virtual void Roam()
    {

    }

    public virtual void Attack()
    {
        Debug.Log("Bad Attack");
    }
   
    public int findClosestWayPoint(GameObject target)
    {
        float distance = 1000.0f;
        int closestWaypoint = 0;
        //Find the waypoint closest to this position
        for (int i = 0; i < graphNodes.graphNodes.Length; i++)
        {
            if (Vector3.Distance(target.transform.position, graphNodes.graphNodes[i].transform.position) <= distance)
            {
                distance = Vector3.Distance(target.transform.position, graphNodes.graphNodes[i].transform.position);
                closestWaypoint = i;
            }
        }

        return closestWaypoint;
    }

    public virtual IEnumerator Stunned()
    {
        stunned = true;
        yield return new WaitForSeconds(stunTime);
        stunned = false;
    }
}
