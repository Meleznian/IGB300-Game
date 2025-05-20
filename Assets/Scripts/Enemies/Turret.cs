using UnityEngine;

public class Turret : EnemyBase
{
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform player;
    [SerializeField] float sightRange = 10f;
    [SerializeField] LayerMask obstacleMask; // Use walls and other objects to block views.
    [SerializeField] Animator anim;
    LineRenderer laserSight;

    float fireCooldown = 0f;

    void Update()
    {
        if (PlayerInSight())
        {
            laserSight.enabled = true;
            laserSight.SetPosition(0, firePoint.position);
            laserSight.SetPosition(1, player.position);


            if (fireCooldown <= 0)
            {
                anim.SetTrigger("Shoot");
                fireCooldown = attackSpeed;
            }
        }
        else
        {
            laserSight.enabled = false;
        }

        fireCooldown -= Time.deltaTime;
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
    }

    public override void ExtraSetup()
    {
        player = GameObject.Find("Player").transform;
        laserSight = GetComponent<LineRenderer>();
    }
}
