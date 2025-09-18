using UnityEngine;

public class PickupScript : MonoBehaviour, ICollectable
{
    public int pickupType = 0;
    public void Collect()
    {
        switch (pickupType)
        {
            case 1:
                //Heal
                PlayerHealth healScript = GameManager.instance.Player.GetComponent<PlayerHealth>();
                healScript.Heal((int)(healScript.maxHealth * 0.4));
                break;
            default:
                break;
        }

        Destroy(gameObject);
    }
}
