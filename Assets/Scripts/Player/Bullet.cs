using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] internal float speed = 10f;
    //[SerializeField] float lifetime = 2f;
    [SerializeField] internal int damage = 1;
    [SerializeField] internal bool playerOwned;

    Vector2 moveDir;

    public void Init(Vector2 direction)
    {
        moveDir = direction.normalized;
        //Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var damageable = other.GetComponent<IDamageable>();
        var player = other.GetComponent<PlayerHealth>();

        if (playerOwned && damageable != null)
        {
            other.GetComponent<BulletLodging>().LodgeBullet();
            damageable.TakeDamage(damage);

            Destroy(gameObject);
        }
        else if(!playerOwned && player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            if (playerOwned)
            {
                GameManager.instance.SpawnBullets(1, transform.position);
            }
            Destroy(gameObject);
        }
    }

    internal void SwitchOwner()
    {
        playerOwned = !playerOwned;

        if (playerOwned)
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        }
    }
}
