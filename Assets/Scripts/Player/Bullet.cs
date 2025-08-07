using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    [SerializeField] internal float speed = 10f;
    //[SerializeField] float lifetime = 2f;
    [SerializeField] internal int damage = 1;
    [SerializeField] internal float knockback;
    [SerializeField] internal bool playerOwned;
    [SerializeField] internal bool originallyEnemy;
    [SerializeField] internal Transform visual;

    public LayerMask playerLayer;
    public LayerMask enemyLayer;

    Vector2 moveDir;

    public void Init(Vector2 direction)
    {
        Debug.Log("Bullet Setup");
        moveDir = direction.normalized;
        visual.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        //Destroy(gameObject, lifetime);

        SetIgnore();
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
            if (!originallyEnemy)
            {
                other.GetComponent<BulletLodging>().LodgeBullet();
            }
            damageable.TakeDamage(damage);
            if(other.GetComponent<Rigidbody2D>() != null)
            {
                other.GetComponent<Rigidbody2D>().AddForce(moveDir*knockback, ForceMode2D.Impulse);
            }

            Destroy(gameObject);
        }
        else if(!playerOwned && player != null)
        {
            if (player.parrying)
            {
                GetParried();
            }
            else
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else if (!other.isTrigger)
        {
            //if (playerOwned && !originallyEnemy)
            //{
            //    GameManager.instance.SpawnBullets(1, transform.position);
            //}
            Destroy(gameObject);
        }
    }

    internal void GetParried()
    {
        playerOwned = true;

        Vector2 newDirection = Input.mousePosition;
        newDirection = Camera.main.ScreenToWorldPoint(newDirection);
        newDirection = newDirection - new Vector2(transform.position.x,transform.position.y);
        moveDir = newDirection.normalized;

        //if (playerOwned)
        //{
        SetIgnore();

        //}
        //else
        //{
        //    gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        //}
    }

    void SetIgnore()
    {
        if (playerOwned)
        {
            GetComponent<Collider2D>().excludeLayers = playerLayer;
            gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        }
        else
        {
            GetComponent<Collider2D>().excludeLayers = enemyLayer;
            gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        }
    }
}
