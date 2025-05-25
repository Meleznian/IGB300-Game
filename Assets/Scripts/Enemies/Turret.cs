using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Turret : EnemyBase
{
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject Gun;
    [SerializeField] Transform player;
    [SerializeField] float sightRange = 10f;
    [SerializeField] LayerMask obstacleMask; // Use walls and other objects to block views.
    [SerializeField] Animator anim;
    internal LineRenderer laserSight;

    float fireCooldown = 0f;
    bool aiming;

    void Update()
    {
        if (PlayerInSight())
        {
            LaserOn();
            RotateGun();

            if (fireCooldown <= 0)
            {
                anim.SetTrigger("Shoot");
                //Fire();
                fireCooldown = attackSpeed;
            }

            fireCooldown -= Time.deltaTime;
        }
        else
        {
            LaserOff();
        }

    }

    bool PlayerInSight()
    {
        if (player == null) return false;

        Vector2 dir = player.position - transform.position;
        float dist = dir.magnitude;

        if (dist > sightRange) return false;

        int mask = ~(1 << LayerMask.NameToLayer("Turret")); // Exclude turret layer
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir.normalized, dist, mask);

        if (hit.collider != null)
        {
            if (hit.collider.transform == player)
            {
                //Debug.Log("Player in sight!");
                return true;
            }
        }

        return false;
    }

    internal void Fire()
    {
        print("Shooting");
        Debug.Log("Turret fired!");

        Vector2 dir = (player.position - firePoint.position).normalized;
        var bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(dir);
        bullet.GetComponent<Bullet>().originallyEnemy = true;

    }

    public override void ExtraSetup()
    {
        player = GameObject.Find("Player").transform;
        laserSight = GetComponent<LineRenderer>();
    }

    void RotateGun()
    {
        laserSight.SetPosition(0, firePoint.position);
        laserSight.SetPosition(1, player.position);
        Vector2 direction = player.position - Gun.transform.position;
        Gun.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        if(player.position.x < transform.position.x)
        {
            Gun.transform.localScale = new Vector3(-1,1,1);
        }
        else
        {
            Gun.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void LaserOn()
    {
        if (!aiming)
        {
            Gun.SetActive(true);
            anim.SetBool("Aiming", true);
            laserSight.enabled = true;
            aiming = true;
        }
    }

    void LaserOff()
    {
        if (aiming)
        {
            Gun.SetActive(false);
            anim.SetBool("Aiming", false);
            laserSight.startColor = Color.red;
            laserSight.endColor = Color.red;
            laserSight.enabled = false;
            fireCooldown = attackSpeed;
            aiming = false;
        }
    }
}
