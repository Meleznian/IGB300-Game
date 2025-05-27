using UnityEngine;
using static UnityEngine.UI.Image;

public class SteamKingAttacks : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] SteamKing steamKingScript;
    PlayerHealth playerHealth;
    [SerializeField] GameObject slashEffect;
    //[SerializeField] GameObject thrustEffect;
    [SerializeField] LayerMask ignore;
    
    

    [Header("Attack Variables")]
    [SerializeField] int slashDamage;
    [SerializeField] int thrustDamage;

    [Header("Attack Transforms")]
    [SerializeField] Transform slashPoint;
    [SerializeField] Vector2 slashSize;
    [SerializeField] Transform thrustPoint;
    [SerializeField] Vector2 thrustSize;

    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    public void GetNextAction()
    {
        steamKingScript.GetNextAction();
    }

    public void Slash()
    {
        var effect = Instantiate(slashEffect, slashPoint.position, slashPoint.rotation);
        effect.transform.localScale = slashSize;

        var hit = Physics2D.OverlapBox(slashPoint.position, slashSize, 0f, ~ignore);
        if (hit != null)
        {
            print(hit.gameObject);
            if (hit.gameObject == playerHealth.gameObject)
            {
                print("Player Hit");
                playerHealth.TakeDamage(slashDamage);
            }
        }
        else
        {
            print("Nothing Hit");
        }
    }

    public void Thrust()
    {
        var effect = Instantiate(slashEffect, thrustPoint.position, thrustPoint.rotation);
        effect.transform.localScale = thrustSize;


        var hit = Physics2D.OverlapBox(thrustPoint.position, thrustSize, 0f, ~ignore);
        if (hit != null)
        {
            print(hit.gameObject);
            if (hit.gameObject == playerHealth.gameObject)
            {
                print("Player Hit");
                playerHealth.TakeDamage(thrustDamage);
            }
        }
        else
        {
            print("Nothing Hit");
        }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        if (slashPoint) Gizmos.DrawWireCube(slashPoint.position, slashSize);
        if (thrustPoint) Gizmos.DrawWireCube(thrustPoint.position, thrustSize);

    }
}
