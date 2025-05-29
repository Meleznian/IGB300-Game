using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerHealth : MonoBehaviour
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
    [SerializeField] SpriteRenderer sprite;

    void Start()
    {
        currentHealth = maxHealth;       // Maximize HP at the start of the game
        anim = GetComponent<Animator>();
    }

    private Animator anim;

    void Update()
    {
        // Q key to recover (when charged and not in recovery)
        if (Input.GetKeyDown(KeyCode.LeftControl) && GameManager.instance.DecreaseGauge(10) && !isHealing)
        {
            StartCoroutine(HealOverTime());
        }
    }

    public void TakeDamage(int amount)
    {
        if (!iframing)
        {
            currentHealth -= amount;
            StartCoroutine(ChangeColour());

            GameManager.instance.DecreaseHype();

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
                anim.SetTrigger("Killed");
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
    }

    void Die()
    {
        Debug.Log("Player died. Reset to Wave 1...");
        FindFirstObjectByType<GameManager>().EndGame();
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
