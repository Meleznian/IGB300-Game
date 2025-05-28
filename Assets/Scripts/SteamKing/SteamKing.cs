using Unity.VisualScripting;
using UnityEngine;

public class SteamKing : EnemyBase
{
    [Header("Component")]
    //[SerializeField] Transform leftWall;
    //[SerializeField] Transform rightWall;
    //[SerializeField] Transform Centre;
    [SerializeField] internal Transform player;
    [SerializeField] Animator anim;
    [SerializeField] Transform[] arenaPoints;
    [SerializeField] Transform currentLocation;

    [Header("King Variables")]
    [SerializeField] float meleeRange;
   

    [Header("Debug Stuff")]
    [SerializeField] int timesParried;
    [SerializeField] bool damagedRecently;
    int locationIndex;

    enum KingStates
    {
        Idle,
        Dashing,
        Attacking
    }

    KingStates state;


    private void Start()
    {
        player = GameObject.Find("Player").transform;
        transform.position = arenaPoints[2].position;
        currentLocation = arenaPoints[2];  
    }

    private void Update()
    {
        if (state == KingStates.Dashing)
        {
            Dash();
        }
        else
        {
            Turn();
        }
    }


    public void GetNextAction()
    {
        float playerDist = Vector3.Distance(transform.position, player.position);
        print(playerDist);
        locationIndex = System.Array.IndexOf(arenaPoints, currentLocation);
        
        if (playerDist < meleeRange)
        {
            GetMeleeAttack();
        }
        else if(locationIndex == 0 || locationIndex == 4)
        {
            damagedRecently = false;
            GetEdgeAttack();
        }
        else
        {
            damagedRecently = false;
            GetCentreAttack();
        }
    }


    void GetMeleeAttack()
    {
        int nextAction = UnityEngine.Random.Range(0, 3);

        if (nextAction == 0)
        {
            StartDash();
            return;
        }
        else if (damagedRecently)
        {
            damagedRecently = false;
            if(nextAction == 1)
            {
                anim.SetTrigger("Kick");
            }
            else
            {
                anim.SetTrigger("DodgeShoot");  
                StartDodge();
            }
        }
        else
        {
            if (nextAction == 1)
            {
                anim.SetTrigger("Thrust");
            }
            else
            {
                anim.SetTrigger("Slash");         
            }
        }
    }

    void GetEdgeAttack()
    {
        int nextAction = UnityEngine.Random.Range(0, 4);
        switch(nextAction)
        {
            case 0:
                //Shield Charge
                return;
            case 1:
                //Dive Slam
                return;
            case 2:
                //Aimed Shot
                return;
            case 3:
                //Chain Whip
                return;
        }
    }

    void GetCentreAttack()
    {
        int nextAction = UnityEngine.Random.Range(0, 4);
        switch (nextAction)
        {
            case 0:
                StartDash();
                return;
            case 1:
                //Leap
                return;
            case 2:
                //Aimed Shot
                return;
            case 3:
                //Chain Whip
                return;
        }
    }


    //Dash Variable
    Transform nextLocation;

    public void StartDash()
    {
        ChooseDashLocation();
        anim.SetBool("Dashing", true);
        state = KingStates.Dashing;
    }
    public void StartDodge()
    {
        ChooseDodgeLocation();
        state = KingStates.Dashing;
    }


    public void Dash()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextLocation.position, moveSpeed * Time.deltaTime);

        if(transform.position == nextLocation.position)
        {
            currentLocation = nextLocation;
            anim.SetBool("Dashing",false);
            state = KingStates.Idle;
        }
    }

    public void ChooseDashLocation()
    {
        int index = locationIndex;

        int next = UnityEngine.Random.Range(0, 2);

        if(index == 1 || index == 2 || index == 3)
        {
            if(next == 0)
            {
                nextLocation = arenaPoints[index + 1];
            }
            else
            {
                nextLocation = arenaPoints[index - 1];
            }
        }
        else if(index == 0)
        {
            nextLocation = arenaPoints[index + 1];
        }
        else if(index == 4)
        {
            nextLocation = arenaPoints[index - 1];
        }
    }

    public void ChooseDodgeLocation()
    {
        int index = locationIndex;

        if (player.position.x < transform.position.x)
        {
            if (index + 1 != 5)
            {
                nextLocation = arenaPoints[index + 1];
            }
            else
            {
                nextLocation = currentLocation;
            }
        }
        else if (player.position.x > transform.position.x)
        {
            if (index - 1 != -1)
            {
                nextLocation = arenaPoints[index - 1];
            }
            else
            {
                nextLocation = currentLocation;
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        if (EnemyManager.instance.LogEnemyDamage)
        {
            print(enemyName + " Has taken " + damage + " Damage");
        }

        health -= damage;
        damagedRecently = true;

        if (health <= 0)
        {
            Die();
        }
    }

    void Turn()
    {
        if (player.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (player.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
