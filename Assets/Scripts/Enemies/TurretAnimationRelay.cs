using UnityEngine;

public class TurretAnimationRelay : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Turret turretScript;

    public void BeginFire()
    {
        print("Calling Fire Function");
        turretScript.Fire();
    }



    public void SetYellow()
    {
        turretScript.laserSight.startColor = Color.yellow;
        turretScript.laserSight.endColor = Color.yellow;

    }
    public void SetRed()
    {
        turretScript.laserSight.startColor = Color.red;
        turretScript.laserSight.endColor = Color.red;
    }
}
