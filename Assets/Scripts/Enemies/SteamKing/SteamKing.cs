using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.U2D;

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
    [SerializeField] SpriteRenderer sprite;

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
        Leaping,
        DeathsDoor,
        Entering
    }

    [SerializeField] KingStates state;


    private void Start()
    {
        player = GameObject.Find("Player").transform;
        currentLocation = arenaPoints[2].position;
        transform.position = arenaPoints[2].position;
        transform.localPosition = new Vector2(transform.localPosition.x, 7);
        nextLocation = arenaPoints[2].position;
        phaseTransition = health / 2;
        state = KingStates.Entering;
        anim.SetBool("Dashing", true);
        AudioManager.PlayMusic(SoundType.BOSS_MUSIC,0.6f);
        AudioManager.PlayEffect(SoundType.STEAM_KING_LAUGH);
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
        if (state == KingStates.Entering)
        {
            Entering();
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
        if (state == KingStates.Idle)
        {
            float playerDist = Vector3.Distance(transform.position, player.position);
            GetLocationIndex();

            if (playerDist < meleeRange)
            {
                if (logConsoleMessages)
                    print("Melee Attack Selected");

                GetMeleeAttack();
            }
            else if (locationIndex == 0 || locationIndex == 4)
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
    }



    void GetMeleeAttack()
    {
        int nextAction = UnityEngine.Random.Range(0, 3);

        if (nextAction == 0)
        {
            if (logConsoleMessages)
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
            case 5:
                if (logConsoleMessages)
                    print("Dashing");
                StartDash();
                return;
        }
    }

    void GetCentreAttack()
    {
        int nextAction = UnityEngine.Random.Range(0, 2);
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
        print("Dashing");
        state = KingStates.Dashing;
        AudioManager.PlayEffect(SoundType.STEAM_KING_DASH);
    }
    public void StartDodge()
    {
        ChooseDodgeLocation();
        AudioManager.PlayEffect(SoundType.STEAM_KING_DASH);
        state = KingStates.Dashing;
    }


    public void Dash()
    {
        if (state != KingStates.Diving)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextLocation, moveSpeed * Time.deltaTime);

            if (transform.position.x == nextLocation.x)
            {
                currentLocation = nextLocation;

                if (anim.GetBool("Dashing"))
                {
                    anim.SetBool("Dashing", false);
                }

                state = KingStates.Idle;

                if (logConsoleMessages)
                {
                    print("Dash Finished");
                }

                if (anim.GetCurrentAnimatorStateInfo(0).IsName("KingCharge"))
                {
                    EndCharge();
                }
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

    public override void TakeDamage(int damage)
    {
        if (state != KingStates.Entering && state != KingStates.Leaping)
        {
            if (EnemyManager.instance != null && LegacyEnemyManager.instance.LogEnemyDamage)
            {
                print(enemyName + " Has taken " + damage + " Damage");
            }

            health -= damage;
            damagedRecently = true;

            if (health <= phaseTransition && !phase2)
            {
                phase2 = true;
            }

            if (health <= 0 && state != KingStates.DeathsDoor)
            {
                DeathsDoor();
            }
            else if (state == KingStates.DeathsDoor)
            {
                dialogue.GetComponent<TMP_Text>().text = "Thank... \nYou...";
                anim.SetTrigger("Killed");
            }
            else
            {
                if (!changing)
                {
                    StartCoroutine(ChangeColour());
                }
            }
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
        AudioManager.PlayEffect(SoundType.STEAM_KING_DASH);
        state = KingStates.Dashing;
        ChargeCollider.enabled = true;
    }


    void EndCharge()
    {
        print("Charge Finished");
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
        AudioManager.PlayEffect(SoundType.STEAM_KING_JUMP);
        state = KingStates.Diving;
        nextLocation = new Vector2(currentLocation.x,currentLocation.y + 15);
    }

    void DiveUp()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextLocation, (moveSpeed*1.5f) * Time.deltaTime);

        if(transform.position.y == nextLocation.y)
        {
            print("Dive Height Reached");
            anim.SetTrigger("DiveDown");
            nextLocation = GetRandomLocation();
            print("Location Selected: " + nextLocation);
            transform.position = new Vector2 (nextLocation.x, transform.position.y);
            hovering = true;
            StartCoroutine(Hover());
        }
    }

    IEnumerator Hover()
    {
        yield return new WaitForSeconds(1);
        up = true;
        hovering = false;
        print("Starting Dive Down");
    }

    void DiveDown()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextLocation, (moveSpeed/1.3f) * Time.deltaTime);

        if (transform.position.y <= nextLocation.y)
        {
            print("Finished Dive");
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
        print("Getting dive Target, current location is " + currentLocation);
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

    void DeathsDoor()
    {
        state = KingStates.DeathsDoor;
        anim.SetTrigger("Dying");
        dialogue.SetActive(true);
        dialogue.GetComponent<TMP_Text>().text = "Finish Me... \nPlease...";
    }

    public void Entering()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextLocation, moveSpeed * Time.deltaTime);

        if (transform.position.y == nextLocation.y)
        {
            currentLocation = nextLocation;

            anim.SetBool("Dashing", false);
            state = KingStates.Leaping;
            StartCoroutine("Dialogue");
        }
    }

    [SerializeField] GameObject dialogue;
    IEnumerator Dialogue()
    {
        dialogue.SetActive(true);
        yield return new WaitForSeconds(3);
        dialogue.SetActive(false);
        state = KingStates.Idle;
    }


    bool changing;
    IEnumerator ChangeColour()
    {
        //print("Colour Changed Started");
        bool done = false;
        bool reverse = false;
        float t = 0;
        changing = true;

        while (!done)
        {
            if (!reverse && sprite.color != Color.red)
            {
                //print("Red");
                sprite.color = Color.Lerp(sprite.color, Color.red, t);
                t += Time.deltaTime;

                yield return null;
            }
            else if (!reverse && sprite.color == Color.red)
            {
                //print("Neither");
                reverse = true;
                t = 0;
                yield return null;
            }
            else if (reverse && sprite.color != Color.white)
            {
                //print("White");
                sprite.color = Color.Lerp(sprite.color, Color.white, t);
                t += Time.deltaTime;

                yield return null;
            }
            else
            {
                done = true;
            }
        }

        changing = false;
        //print("Colour Changed Finished");
    }
}

