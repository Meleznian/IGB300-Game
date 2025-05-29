using UnityEngine;

public class Shockwave : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float speed;
    [SerializeField] float knockBack;
    internal Vector3 direction;

    private void Update()
    {
        transform.position += (direction * speed)*Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("Player hit by Shockwave");
            other.GetComponent<Rigidbody2D>().AddForce(direction*knockBack, ForceMode2D.Impulse);
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    public void Setup(int Damage, float Speed, float Knockback, Vector3 Direction)
    {
        damage = Damage;
        speed = Speed;
        knockBack = Knockback;
        direction = Direction;
    }
}
