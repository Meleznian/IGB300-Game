using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public int damage;
    public float cooldown;
    float timer;
    bool canDamage;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            canDamage = true;
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (canDamage)
        {
            if (other.CompareTag("Player")) other.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
            timer = 0;
            canDamage = false;
        }
    }
}
