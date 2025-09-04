using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("HP setting")]
    public int maxHealth = 100;          // Maximum HP
    public int currentHealth;            // Current HP

    [Header("Recovery Settings")]
    public int healAmountPerTick = 5;    // Amount of recovery (per tick)
    public float healTickDelay = 0.5f;   // Tick interval (seconds)
    public int totalHealTicks = 5;       // Recovery times
    //public int healCharge = 1;           // Charge used for heels

    public bool isHealing = false;       // Currently recovering?

    internal bool parrying;
    bool iframing;
    bool dead;

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] ParticleSystem healStart;
    [SerializeField] ParticleSystem healing;
    [SerializeField] ParticleSystem damaged;
    [SerializeField] ParticleSystem lowhealth;
    [SerializeField] GameObject death;
    [SerializeField] internal ParticleSystem getBolt;



    void Start()
    {
        currentHealth = maxHealth;       // Maximize HP at the start of the game
        anim = sprite.GetComponent<Animator>();
    }

    private Animator anim;

    void Update()
    {
        // Q key to recover (when charged and not in recovery)
        if(dead)
        {
            DeathTimer();
        }

    }

    public void TakeDamage(int amount)
    {
        if (!iframing && !dead)
        {
            currentHealth -= amount;
            Instantiate(damaged,transform.position, Quaternion.identity);
            StartCoroutine(ChangeColour());

            if(currentHealth <= maxHealth * 0.4)
            {
                lowhealth.Play();
            }

            //GameManager.instance.DecreaseHype();
            AudioManager.PlayEffect(SoundType.TAKE_DAMAGE, 0.7f);

            if (currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    IEnumerator HealOverTime()
    {
        isHealing = true;
        //healCharge--;

        for (int i = 0; i < totalHealTicks; i++)
        {
            currentHealth += healAmountPerTick;

            if (currentHealth >= currentHealth * 0.3)
            {
                lowhealth.Stop();
            }

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            yield return new WaitForSeconds(healTickDelay);
        }

        isHealing = false;
        healing.Stop();
    }

    public void IncreaseMax(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
    }

    //internal void StartParry()
    //{
    //    parrying = true;
    //}
    //internal void EndParry()
    //{
    //    parrying = false;
    //}

    IEnumerator ChangeColour()
    {
        //print("Colour Changed Started");
        bool done = false;
        bool reverse = false;
        float t = 0;
        iframing = true;

        while (!done)
        {
            if (!reverse && sprite.color != new Color (0.5f, 0f, 0f))
            {
                //print("Red");
                sprite.color = Color.Lerp(sprite.color, new Color(0.5f, 0f, 0f), t);
                t += Time.deltaTime;

                yield return null;
            }
            else if (!reverse && sprite.color == new Color(0.5f, 0f, 0f))
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

        iframing = false;
        //print("Colour Changed Finished");
    }


    float deathTimer;
    void DeathTimer()
    {
        deathTimer += Time.deltaTime;
        if(deathTimer > 3)
        {
            GameManager.instance.EndGame();
        }
    }

    public void Kill()
    {
        if (!dead)
        {
            //currentHealth = 0;
            //anim.SetTrigger("Killed");
            //AudioManager.PlayEffect(SoundType.PLAYER_DEATH);
            //GetComponent<PlayerMovement>().Die();
            //dead = true;

            AudioManager.PlayEffect(SoundType.PLAYER_DEATH);
            currentHealth = 0;
            //anim.SetTrigger("Killed");
            sprite.enabled = false;
            GameManager.instance.playerDead = true;
            Instantiate(death, transform.position, transform.rotation);
            GetComponent<PlayerMovement>().Die();
            dead = true;
        }
    }

    internal void StartHealing()
    {
        AudioManager.PlayEffect(SoundType.PLAYER_HEAL);
        healStart.Play();
        healing.Play();
        StartCoroutine(HealOverTime());
    }
}
