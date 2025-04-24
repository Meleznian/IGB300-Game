using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform player;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float sightRange = 10f;
    [SerializeField] LayerMask obstacleMask; // Use walls and other objects to block views.

    float fireCooldown = 0f;

    void Update()
    {
        if (PlayerInSight())
        {
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                Fire();
                fireCooldown = 1f / fireRate;
            }
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
                Debug.Log("Player in sight!");
                return true;
            }
        }

        return false;
    }

    void Fire()
    {
        Debug.Log("Turret fired!");

        Vector2 dir = (player.position - firePoint.position).normalized;
        var bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(dir);
    }
}
