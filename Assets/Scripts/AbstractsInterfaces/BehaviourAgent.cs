using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourAgent : NavigationAgent, IDamageable
{
    public State currentState;
    private State _oldState;
    public GameObject target;
    public bool stunned;
    public float stunTime;

    public float health;
    public float iFrames;
    public bool hasIFrames;

    public bool colliding;
    public List<GameObject> collisions;
    public GameObject playerCollision;

    public void Start()
    {
        //currentNodeIndex = findClosestWayPoint(gameObject);
        
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
            case State.Pursue:
                Pursue();
                break;
            case State.Flee:
                Flee();
                break;
        }

        if(currentState != _oldState)
        {
            currentPath.Clear();
            greedyPaintList.Clear();
            currentPathIndex = 0;

            _oldState = currentState;
        }
        /*if(Input.GetKeyDown(KeyCode.Space))
        {
            List<int> integers = new List<int>
        {
            10
        };
            GreedySearch(10, 3, integers);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            AStarSearch(10, 6);
        }*/
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
        for (int i = 0; i < graphNodes.graphNodes.Count; i++)
        {
            if (Vector3.Distance(target.transform.position, graphNodes.graphNodes[i].transform.position) <= distance)
            {
                distance = Vector3.Distance(target.transform.position, graphNodes.graphNodes[i].transform.position);
                closestWaypoint = i;
            }
            Debug.Log(closestWaypoint);
        }

        return closestWaypoint;
    }

    public virtual void Pursue()
    {

    }
    public virtual void Flee()
    {

    }

    public virtual IEnumerator Stunned()
    {
        stunned = true;
        yield return new WaitForSeconds(stunTime);
        stunned = false;
    }

    public void TakeDamage(int damage)
    {
        if (hasIFrames) return;
        StartCoroutine(Invincible());
        health -= damage;
        if (health <= 0) Death();
    }

    private void Death()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator Invincible()
    {
        hasIFrames = true;
        yield return new WaitForSeconds(iFrames);
        hasIFrames = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collisions.Contains(collision.gameObject)) return;
        collisions.Add(collision.gameObject);
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCollision = collision.gameObject;
            colliding = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collisions.Contains(collision.gameObject)) return;
        collisions.Remove(collision.gameObject);
        if (collision.gameObject.CompareTag("Player")) colliding = false;
    }
}
