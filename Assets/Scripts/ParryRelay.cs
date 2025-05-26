using UnityEngine;

public class ParryRelay : MonoBehaviour
{
    [SerializeField] PlayerHealth health;
    [SerializeField] Parry parry;

    public void StartParry()
    {
        health.StartParry();
        parry.StartParry();
    }
    public void EndParry()
    {
        health.EndParry();
        parry.EndParry();   
    }
}
