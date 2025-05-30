using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform aimCursor;
    [SerializeField] float bulletSpeed;
    [SerializeField] int bulletDamage;
    [SerializeField] float bulletKnockback;
    [SerializeField] Animator anim;
    [SerializeField] GameObject shoulder;


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
        anim.SetTrigger("Shoot");
        shoulder.SetActive(true);
        shoulder.transform.rotation = Quaternion.FromToRotation(Vector3.up, -dir);

        if (!bulletPrefab || !firePoint) return;
        if (GameManager.instance.DecreaseAmmo(1))
        {
            Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.Init(dir);
            bullet.playerOwned = true;
            bullet.damage = bulletDamage;
            bullet.speed = bulletSpeed;
            bullet.knockback = bulletKnockback;
        }
    }

    internal void IncreaseDamage(int amount)
    {
        bulletDamage += amount;
    }
    internal void IncreaseSpeed(float move, float cool)
    {
        bulletSpeed += move;
        rangedCooldown -= cool;
    }
    internal void IncreaseKnockback(float amount)
    {
        bulletKnockback += amount;
    }
}
