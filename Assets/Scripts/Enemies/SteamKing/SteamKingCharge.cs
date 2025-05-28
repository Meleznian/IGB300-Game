using UnityEngine;

public class SteamKingCharge : MonoBehaviour
{
    [SerializeField] SteamKingAttacks attack;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("Player hit by Charge");
            attack.ChargeHit();
        }
    }
}
