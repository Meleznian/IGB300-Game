using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SteamKing : EnemyBase
{
    [Header("Component")]
    //[SerializeField] Transform leftWall;
    //[SerializeField] Transform rightWall;
    //[SerializeField] Transform Centre;
    [SerializeField] internal Transform player;
    [SerializeField] internal SteamKingAttacks attackScript;
    [SerializeField] Animator anim;
    [SerializeField] Transform[] arenaPoints;
    [SerializeField] Vector2 currentLocation;
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
        Attacking,
        Diving,
        Leaping
    }

    KingStates state;


    private void Start()
    {
        player = GameObject.Find("Player").transform;
        transform.position = arenaPoints[2].position;
        currentLocation = arenaPoints[2].position;
        phaseTransition = health / 2;
    }

    private void Update()
    {
        if (state == KingStates.Dashing)
        {
            Dash();
        }
        else if (state == KingStates.Leaping)
        {
            
        }
        else if (state == KingStates.Diving)
        {
            DiveSlam();
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
        GetLocationIndex();

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
        int nextAction = UnityEngine.Random.Range(0, 5);
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
                StartDive();
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
            case 4:
                if (logConsoleMessages)
                    print("Dashing");
                StartDash();
                return;
        }
    }

    void GetCentreAttack()
    {
        int nextAction = UnityEngine.Random.Range(0, 3);
        switch (nextAction)
        {
            case 0:
                if (logConsoleMessages)
                    print("Dashing");
                StartDash();
                return;
            case 1:
                if (logConsoleMessages)
                    print("Whipping");
                anim.SetTrigger("ChainWhip");
                return;
            case 2:
                if (logConsoleMessages)
                    print("Aiming");
                anim.SetTrigger("AimedShot");
                return;
            case 3:
                if (logConsoleMessages)
                    print("Leaping");
                //Leap
                return;
        }
    }


    //Dash Variable
    Vector2 nextLocation;

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
        transform.position = Vector3.MoveTowards(transform.position, nextLocation, moveSpeed * Time.deltaTime);

        if(transform.position.x == nextLocation.x)
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
                nextLocation = arenaPoints[index + 1].position;
            }
            else
            {
                nextLocation = arenaPoints[index - 1].position;
            }
        }
        else if(index == 0)
        {
            nextLocation = arenaPoints[index + 1].position;
        }
        else if(index == 4)
        {
            nextLocation = arenaPoints[index - 1].position;
        }
    }

    public void ChooseDodgeLocation()
    {
        int index = locationIndex;

        if (player.position.x < transform.position.x)
        {
            if (index + 1 != 5)
            {
                nextLocation = arenaPoints[index + 1].position;
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
                nextLocation = arenaPoints[index - 1].position;
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
            nextLocation = arenaPoints[4].position;
        }
        else if(locationIndex == 4)
        {
            nextLocation = arenaPoints[0].position;
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

    bool up;
    bool hovering;

    void DiveSlam()
    {
        if(!up && !hovering)
        {
            DiveUp();
        }
        else if(up)
        {
            DiveDown();
        }
    }

    void StartDive()
    {
        anim.SetBool("Diving", true);
        state = KingStates.Diving;
        nextLocation = new Vector2(currentLocation.x,currentLocation.y + 10);
    }

    void DiveUp()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextLocation, (moveSpeed*1.5f) * Time.deltaTime);

        if(transform.position.y == nextLocation.y)
        {
            anim.SetTrigger("DiveDown");
            nextLocation = GetRandomLocation();
            transform.position = new Vector2 (nextLocation.x, transform.position.y);
            hovering = true;
            StartCoroutine(Hover());
        }
    }

    IEnumerator Hover()
    {
        yield return new WaitForSeconds(2);
        up = true;
        hovering = false;
    }

    void DiveDown()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextLocation, (moveSpeed/1.3f) * Time.deltaTime);

        if (transform.position.y <= nextLocation.y)
        {
            anim.SetBool("Diving", false);
            attackScript.DiveSlam();
            currentLocation = nextLocation;
            state = KingStates.Idle;
            up = false;
        }
    }

    void Leap()
    {

    }

    Vector2 GetRandomLocation()
    {
        return arenaPoints[UnityEngine.Random.Range(0,5)].position;
    }

    void GetLocationIndex()
    {
        foreach(Transform t in arenaPoints)
        {
            if(currentLocation.x == t.position.x)
            {
                locationIndex = System.Array.IndexOf(arenaPoints, t);
                break;
            }
        }
    }
}
