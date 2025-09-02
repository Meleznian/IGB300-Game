using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] bool canDamage;
    [SerializeField] float timer;

    private void Update()
    {
        if (!canDamage)
        {
            Cooldown();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (canDamage)
        {
            var player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                print("Triggered: " + other.name);
                player.TakeDamage(damage);
                canDamage = false;
                timer = 1;
            }
        }
    }

    void Cooldown()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            canDamage = true;
        }
    }
}
