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
            case 2:
                //Nuke
                for(int i = 0; i < EnemyManager.instance.livingEnemiesList.Count; i++)
                {
                    EnemyManager.instance.livingEnemiesList[0].GetComponent<EnemyBase>().Die(true);
                }
                break;
            case 3:
                //Slow Kill Wall
                GameManager.instance.killWall.gameObject.GetComponent<KillWall>().SlowKillWallPickup();
                break;
            case 4:
                //Money
                GameManager.instance.BoltCount((float)(GameManager.instance.gameObject.GetComponent<UpgradeManager>().cashGoal * 0.3));
                break;
            case 5:
                //SuperCharge
                GameManager.instance.Player.GetComponent<PlayerMeleeAttack>().RunSuperCharge();
                break;
            default:
                Debug.Log("Not Implemented Exception");
                break;
        }

        Destroy(gameObject);
    }
}
