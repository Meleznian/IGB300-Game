using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform aimCursor;

    [SerializeField] float rangedCooldown = 0.3f;
    float rangedCooldownTimer = 0f;

    void Update()
    {
        rangedCooldownTimer -= Time.deltaTime;

        // Right click with mouse
        if (Input.GetKeyDown(KeyCode.LeftShift) && rangedCooldownTimer <= 0f)
        {
            Vector2 dir = aimCursor.position - firePoint.position;
            FireBullet(dir.normalized);
            rangedCooldownTimer = rangedCooldown;
        }

        // Controller: press R2
        if (Input.GetKeyDown(KeyCode.JoystickButton7) && rangedCooldownTimer <= 0f)
        {
            Vector2 dir = aimCursor.position - firePoint.position;
            FireBullet(dir.normalized);
            rangedCooldownTimer = rangedCooldown;
        }
    }

    void FireBullet(Vector2 dir)
    {
        if (!bulletPrefab || !firePoint) return;

        var bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(dir);
    }
}
