using UnityEngine;
using static UnityEngine.UI.Image;

public class SteamKingAttacks : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] SteamKing steamKingScript;
    PlayerHealth playerHealth;
    [SerializeField] GameObject slashEffect;
    [SerializeField] LayerMask ignore;
    
    

    [Header("Attack Variables")]
    [SerializeField] int SlashDamage;

    [Header("Attack Transforms")]
    [SerializeField] Transform slashPoint;
    [SerializeField] Vector2 slashSize;

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
        Instantiate(slashEffect, slashPoint.position, slashPoint.rotation);
        var hit = Physics2D.OverlapBox(slashPoint.position, slashSize, 0f, ~ignore);
        if (hit != null)
        {
            print(hit.gameObject);
            if (hit.gameObject == playerHealth.gameObject)
            {
                print("Player Hit");
                playerHealth.TakeDamage(SlashDamage);
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
    }
}
