using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float lifetime = 2f;
    [SerializeField] int damage = 1;

    Vector2 moveDir;

    public void Init(Vector2 direction)
    {
        moveDir = direction.normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        var enemy = other.GetComponent<IDamageable>();
        if (enemy != null)
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
