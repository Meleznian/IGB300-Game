using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourAgent : NavigationAgent, IDamageable
{
    public bool assignGraph;
    public bool grounded;
    public State currentState;
    private State _oldState;
    public GameObject target;
    public bool stunned;
    public float stunTime;

    public int health;
    public float iFrames;
    public bool hasIFrames;

    public bool parriable;

    public bool flipped;
    public float lastX;
    
    [Header("Animation")]
    public GameObject animation;
    public Animator anim;
    internal bool dead;


    public Transform attackPoint;
    [SerializeField] LayerMask playerLayer;


    [Header("Currency Drop")]
    [Tooltip("Prefab to spawn when the enemy dies")]
    public GameObject currencyPrefab;

    [Tooltip("Number of currency drops to spawn")]
    public int dropAmount = 1;


    public void Start()
    {
        currentNodeIndex = findClosestWayPoint(gameObject);

    }
    // Update is called once per frame
    void Update()
    {
        if (stunned) return;
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

        if(transform.position.x > lastX)
        {
            flipped = false;
            lastX = transform.position.x;
        } else
        {
            flipped = true;
            lastX = transform.position.x;
        }

        if (!dead)
        {
            animation.transform.localScale = flipped ? new Vector3(-0.1f, 0.1f, 0.1f) : new Vector3(0.1f, 0.1f, 0.1f);
        }

        if (currentState != _oldState)
        {
            currentPath.Clear();
            greedyPaintList.Clear();
            currentPathIndex = 0;

            _oldState = currentState;
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
        for (int i = 0; i < graphNodes.graphNodes.Count; i++)
        {
            if (Vector3.Distance(target.transform.position, graphNodes.graphNodes[i].transform.position) <= distance)
            {
                distance = Vector3.Distance(target.transform.position, graphNodes.graphNodes[i].transform.position);
                closestWaypoint = i;
            }
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
        Debug.Log("Stunned");
        anim.SetBool("Stunned", true);
        yield return new WaitForSeconds(stunTime);
        stunned = false;
        anim.SetBool("Stunned", false);

    }

    public void TakeDamage(int damage)
    {
        if (hasIFrames) return;
        health -= damage;
        if (health <= 0) Death();
    }

    private void Death()
    {
        if (!dead)
        {
            dead = true;
            anim.SetTrigger("Dead");
            SpawnCurrency();
        }
    }

    public IEnumerator Invincible()
    {
        hasIFrames = true;
        yield return new WaitForSeconds(iFrames);
        hasIFrames = false;
    }


    protected void DealDamage(Vector2 origin, int damage)
    {
        float range = 1f;



        var hits = Physics2D.OverlapCircleAll(origin, range, playerLayer);
        foreach (var h in hits)
        {
            var player = h.GetComponent<IDamageable>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }

    }


    void SpawnCurrency()
    {

        //if (currencyPrefab == null) return;
        //
        //Debug.Log("Drop bolt");
        //
        //for (int i = 0; i < dropAmount; i++)
        //{
        //    // Random offset to spread them a bit
        //    Vector2 spawnOffset = UnityEngine.Random.insideUnitCircle * 0.5f;
        //    Instantiate(currencyPrefab, transform.position + (Vector3)spawnOffset, Quaternion.identity);
        //}

        GameObject[] money = GameManager.instance.GenerateMoney(dropAmount);

        Debug.Log("Drop bolt");

        foreach (GameObject g in money)
        {
            // Random offset to spread them a bit
            Vector2 spawnOffset = UnityEngine.Random.insideUnitCircle * 0.5f;
            Instantiate(g, transform.position + (Vector3)spawnOffset, Quaternion.identity);
        }
    }

    public void Killed()
    {
        EnemyManager.instance.EnemyKilled(gameObject);
    }
}
