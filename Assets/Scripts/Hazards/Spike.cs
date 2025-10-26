using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] internal bool canDamage;
    float timer;
    [SerializeField] float knockback;

    private void Update()
    {
        DoUpdate();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (canDamage)
        {
            var player = other.GetComponent<PlayerHealth>();
            var knockbackApply = other.GetComponent<PlayerMovement>();

            if (player != null)
            {
                print("Triggered: " + other.name);

                knockbackApply.ApplyKnockbackFrom(transform.position - new Vector3(0, 1, 0), knockback);
                player.TakeDamage(damage);
                canDamage = false;
                timer = 1;
            }
        }
    }

    internal void Cooldown()
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

    public virtual void DoUpdate()
    {
        if (!canDamage)
        {
            Cooldown();
        }
    }
}
