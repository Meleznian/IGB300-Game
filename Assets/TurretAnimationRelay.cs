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
}
