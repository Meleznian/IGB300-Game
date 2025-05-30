using UnityEngine;

public class ParryRelay : MonoBehaviour
{
    [SerializeField] PlayerHealth health;
    [SerializeField] Parry parry;
    [SerializeField] GameObject shoulder;

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

    public void DisableShoulder()
    {
        shoulder.SetActive(false);
    }
}
