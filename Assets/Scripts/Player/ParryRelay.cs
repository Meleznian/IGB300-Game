using UnityEngine;

public class ParryRelay : MonoBehaviour
{
    [SerializeField] PlayerHealth health;
    [SerializeField] Parry parry;
    [SerializeField] GameObject shoulder;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

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
    public void EnableShoulder()
    {
        shoulder.SetActive(true);
    }

    public void IncrementSlash()
    {
        anim.SetInteger("Slashes", anim.GetInteger("Slashes") + 1);

        if (anim.GetInteger("Slashes") == 3)
        {
            anim.SetInteger("Slashes", 0);
        }
    }
    public void Die()
    {
        Debug.Log("Player died... Activating Death Panel");
        GameManager.instance.EndGame();
    }
}
