using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] internal float speed = 10f;
    //[SerializeField] float lifetime = 2f;
    [SerializeField] internal int damage = 1;
    [SerializeField] internal int pierce = 1;

    [SerializeField] internal float knockback;
    [SerializeField] internal bool playerOwned;
    [SerializeField] internal bool originallyEnemy;
    [SerializeField] internal Transform visual;
    [SerializeField] ParticleSystem dropParticle;

    public LayerMask playerLayer;
    public LayerMask enemyLayer;

    int pierced;

    Vector2 moveDir;

    public enum ProjectileType 
    {
        Bullet,
        Spear,
        Axe
    }

    public ProjectileType projectileType;

    public void Init(Vector2 direction)
    {
        Debug.Log("Bullet Setup");
        moveDir = direction.normalized;
        visual.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        //Destroy(gameObject, lifetime);

        SetIgnore();

        if(projectileType == ProjectileType.Axe)
        {
            GetComponent<Rigidbody2D>().AddForce(moveDir*(speed*1.7f), ForceMode2D.Impulse);
            GetComponent<Rigidbody2D>().AddTorque(-speed, ForceMode2D.Impulse);
        }
    }

    void Update()
    {
        if (projectileType != ProjectileType.Axe)
        {
            transform.Translate(moveDir * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var damageable = other.GetComponent<IDamageable>();
        var player = other.GetComponent<PlayerHealth>();

        if (playerOwned && damageable != null)
        {
            damageable.TakeDamage(damage);
            if(other.GetComponent<Rigidbody2D>() != null)
            {
                other.GetComponent<Rigidbody2D>().AddForce(moveDir*knockback, ForceMode2D.Impulse);
            }

            pierced++;

            if (pierced > pierce)
            {
                Destroy(gameObject);
                EnemyManager.instance.BulletHit(transform.position);
            }
        }
        //else if(!playerOwned && player != null)
        //{
        //    if (player.parrying)
        //    {
        //        GetParried();
        //    }
        //    else
        //    {
        //        player.TakeDamage(damage);
        //        Destroy(gameObject);
        //    }
        //}
        else if (!other.isTrigger)
        {
            //if (playerOwned && !originallyEnemy)
            //{
            //    GameManager.instance.SpawnBullets(1, transform.position);
            //}
            Instantiate(dropParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
            //EnemyManager.instance.BulletWall(transform.position);
        }
    }

    //internal void GetParried()
    //{
    //    playerOwned = true;
    //
    //    Vector2 newDirection = Input.mousePosition;
    //    newDirection = Camera.main.ScreenToWorldPoint(newDirection);
    //    newDirection = newDirection - new Vector2(transform.position.x,transform.position.y);
    //    moveDir = newDirection.normalized;
    //
    //    //if (playerOwned)
    //    //{
    //    SetIgnore();
    //
    //    //}
    //    //else
    //    //{
    //    //    gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
    //    //}
    //}

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
