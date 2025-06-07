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


    void Start()
    {
        currentHealth = maxHealth;       // Maximize HP at the start of the game
        anim = sprite.GetComponent<Animator>();
    }

    private Animator anim;

    void Update()
    {
        // Q key to recover (when charged and not in recovery)
        if (Input.GetKeyDown(KeyCode.Q) && GameManager.instance.DecreaseGauge(10) && !isHealing)
        {
            AudioManager.PlayEffect(SoundType.PLAYER_HEAL);
            healStart.Play();
            healing.Play();
            StartCoroutine(HealOverTime());
        }
    }

    public void TakeDamage(int amount)
    {
        if (!iframing && !dead)
        {
            currentHealth -= amount;
            StartCoroutine(ChangeColour());

            GameManager.instance.DecreaseHype();
            AudioManager.PlayEffect(SoundType.TAKE_DAMAGE, 0.7f);

            if (currentHealth <= 0)
            {
                AudioManager.PlayEffect(SoundType.PLAYER_DEATH);
                currentHealth = 0;
                anim.SetTrigger("Killed");
                GetComponent<PlayerMovement>().Die();
                dead = true;
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

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            yield return new WaitForSeconds(healTickDelay);
        }

        isHealing = false;
        healing.Stop();
    }

    public void IncreaseMax(int amount)
    {
        maxHealth +=+ amount;
    }

    internal void StartParry()
    {
        parrying = true;
    }
    internal void EndParry()
    {
        parrying = false;
    }

    IEnumerator ChangeColour()
    {
        //print("Colour Changed Started");
        bool done = false;
        bool reverse = false;
        float t = 0;
        iframing = true;

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

        iframing = false;
        //print("Colour Changed Finished");
    }
}
