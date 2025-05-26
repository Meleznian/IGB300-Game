using UnityEngine;

public class SteamKing : EnemyBase
{
    [Header("Component")]
    //[SerializeField] Transform leftWall;
    //[SerializeField] Transform rightWall;
    //[SerializeField] Transform Centre;
    [SerializeField] internal Transform Player;
    [SerializeField] Animator anim;
    [SerializeField] Transform[] arenaPoints;
    [SerializeField] Transform currentLocation;

    [Header("King Variables")]
    [SerializeField] float meleeRange;
   

    [Header("Debug Stuff")]
    [SerializeField] int timesParried;
    [SerializeField] bool damagedRecently;

    enum KingStates
    {
        Idle,
        Dashing,
        Attacking
    }

    KingStates state;


    private void Start()
    {
        Player = GameObject.Find("Player").transform;
        transform.position = arenaPoints[2].position;
        currentLocation = arenaPoints[2];  
    }

    private void Update()
    {
        if (state == KingStates.Dashing)
        {
            Dash();
        }
    }


    public void GetNextAction()
    {
        //float playerDist = Vector3.Distance(transform.position, Player.position);
        //int index = System.Array.IndexOf(arenaPoints, currentLocation);
        
        //if (playerDist < meleeRange)
        {
            GetMeleeAttack();
        }
        //else if(index == 0 || index == 4)
        //{
        //    GetEdgeAttack();
        //}
        //else
        //{
        //    GetCentreAttack();
        //}
    }


    void GetMeleeAttack()
    {
        int nextAction = UnityEngine.Random.Range(0, 3);

        if (nextAction == 0)
        {
            StartDash();
            return;
        }
        if (damagedRecently)
        {
            if(nextAction == 1)
            {
                //Kick
            }
            else
            {
               //DodgeShoot            
            }
        }
        else
        {
            if (nextAction == 1)
            {
                anim.SetTrigger("Slash");
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
        int index = System.Array.IndexOf(arenaPoints, currentLocation);

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
}
