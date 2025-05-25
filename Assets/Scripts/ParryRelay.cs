using UnityEngine;

public class ParryRelay : MonoBehaviour
{
    [SerializeField] PlayerHealth health;

    public  void StartParry()
    {
        health.StartParry();
    }
    public void EndParry()
    {
        health.EndParry();
    }
}
