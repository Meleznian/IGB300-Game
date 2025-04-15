using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform aimCursor;

    void Update()
    {
        // Right click with mouse
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 dir = aimCursor.position - firePoint.position;
            FireBullet(dir.normalized);
        }

        // Controller: press R2
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            Vector2 dir = aimCursor.position - firePoint.position;
            FireBullet(dir.normalized);
        }
    }

    void FireBullet(Vector2 dir)
    {
        if (!bulletPrefab || !firePoint) return;

        var bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(dir);
    }
}
