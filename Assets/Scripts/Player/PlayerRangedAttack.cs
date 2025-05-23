using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform aimCursor;
    [SerializeField] float bulletSpeed;
    [SerializeField] int bulletDamage;

    void Update()
    {
        // Right click with mouse
        if (Input.GetKeyDown(KeyCode.LeftShift))
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

        if (GameManager.instance.DecreaseAmmo(1))
        {
            Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.Init(dir);
            bullet.playerOwned = true;
            bullet.damage = bulletDamage;
            bullet.speed = bulletSpeed;
        }
    }

    internal void IncreaseDamage(int amount)
    {
        bulletDamage += amount;
    }
    internal void IncreaseSpeed(float amount)
    {
        bulletSpeed += amount;
    }
}
