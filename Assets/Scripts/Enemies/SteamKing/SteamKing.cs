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
    [SerializeField] BoxCollider2D ChargeCollider;

    [Header("King Variables")]
    [SerializeField] float meleeRange;
    float phaseTransition;
    [SerializeField] bool phase2;
   

    [Header("Debug Stuff")]
    [SerializeField] int timesParried;
    [SerializeField] bool damagedRecently;
    [SerializeField] bool logConsoleMessages;

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
        phaseTransition = health / 2;
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

    public void Phase2GetNextAction()
    {
        if (phase2)
        {
            GetNextAction();
        }
    }


    public void GetNextAction()
    {
        float playerDist = Vector3.Distance(transform.position, player.position);
        //print(playerDist);
        locationIndex = System.Array.IndexOf(arenaPoints, currentLocation);
        
        if (playerDist < meleeRange)
        {
            if (logConsoleMessages)
                print("Melee Attack Selected");
            GetMeleeAttack();
        }
        else if(locationIndex == 0 || locationIndex == 4)
        {
            damagedRecently = false;
            if (logConsoleMessages)
                print("Edge Attack Selected");
            GetEdgeAttack();
        }
        else
        {
            damagedRecently = false;
            if (logConsoleMessages)
                print("Centre Attack Selected");
            GetCentreAttack();
        }
    }



    void GetMeleeAttack()
    {
        int nextAction = UnityEngine.Random.Range(0, 3);

        if (nextAction == 0)
        {
            if (logConsoleMessages)
                print("Dashing");
            StartDash();
            return;
        }
        else if (damagedRecently)
        {
            damagedRecently = false;
            if(nextAction == 1)
            {
                if (logConsoleMessages)
                    print("Kicking");
                anim.SetTrigger("Kick");
            }
            else
            {
                if (logConsoleMessages)
                    print("Dodge Shooting");
                anim.SetTrigger("DodgeShoot");  
                StartDodge();
            }
        }
        else
        {
            if (nextAction == 1)
            {
                if (logConsoleMessages)
                    print("Thrusting");
                anim.SetTrigger("Thrust");
            }
            else
            {
                if (logConsoleMessages)
                    print("Slashing");
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
                if (logConsoleMessages)
                    print("Charging");
                ReadyCharge();
                return;
            case 1:
                if (logConsoleMessages)
                    print("Diving");
                //Dive Slam
                return;
            case 2:
                if (logConsoleMessages)
                    print("Aiming");
                anim.SetTrigger("AimedShot");
                return;
            case 3:
                if (logConsoleMessages)
                    print("Whipping");
                anim.SetTrigger("ChainWhip");
                return;
        }
    }

    void GetCentreAttack()
    {
        int nextAction = UnityEngine.Random.Range(0, 4);
        switch (nextAction)
        {
            case 0:
                if (logConsoleMessages)
                    print("Dashing");
                StartDash();
                return;
            case 1:
                if (logConsoleMessages)
                    print("Leaping");
                //Leap
                return;
            case 2:
                if (logConsoleMessages)
                    print("Aiming");
                anim.SetTrigger("AimedShot");
                return;
            case 3:
                if (logConsoleMessages)
                    print("Whipping");
                anim.SetTrigger("ChainWhip");
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

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("KingCharge"))
            {
                EndCharge();
            }
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

        if(health <= phaseTransition && !phase2)
        {
            phase2 = true;  
        }

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

    void ReadyCharge()
    {
        anim.SetBool("Charging", true);

        if (locationIndex == 0)
        {
            nextLocation = arenaPoints[4];
        }
        else if(locationIndex == 4)
        {
            nextLocation = arenaPoints[0];
        }
    }

    internal void StartCharge()
    {
        anim.SetTrigger("Charge");
        state = KingStates.Dashing;
        ChargeCollider.enabled = true;
    }


    void EndCharge()
    {
        ChargeCollider.enabled = false;
        anim.SetBool("Charging", false);
    }
}
