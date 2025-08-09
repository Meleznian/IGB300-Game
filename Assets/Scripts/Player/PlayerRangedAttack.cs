using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRangedAttack : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float bulletSpeed;
    [SerializeField] int bulletDamage;
    [SerializeField] float bulletKnockback;
    [SerializeField] float heatPerShot;
    [SerializeField] float maxHeat;
    [SerializeField] float coolModifier = 1;
    [SerializeField] float rangedCooldown = 0.3f;
    [SerializeField] float heat;

    [Header("Components")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform aimCursor;
    [SerializeField] Animator anim;
    [SerializeField] GameObject shoulder;
    [SerializeField] Slider heatGauge;
    [SerializeField] Image heatGaugeFill;
    Color heatColor;

    bool disabled;

    


    float rangedCooldownTimer = 0f;

    private void Start()
    {
        print("Gun Setup");
        heatColor = heatGaugeFill.color;
        heatGauge.maxValue = maxHeat;
        print(heatGauge.maxValue);
    }

    void Update()
    {
        if (!GameManager.instance.playerDead)
        {
            rangedCooldownTimer -= Time.deltaTime;



            if (!disabled)
            {
                // Right click with mouse
                if (Input.GetKeyDown(KeyCode.LeftShift) && rangedCooldownTimer <= 0f)
                {
                    Vector2 dir = aimCursor.position - firePoint.position;
                    FireBullet(dir.normalized);
                    rangedCooldownTimer = rangedCooldown;
                    heat += heatPerShot;
                    print("updating heat value to: " + heat);

                }

                // Controller: press R2
                //if (Input.GetKeyDown(KeyCode.JoystickButton7) && rangedCooldownTimer <= 0f)
                //{
                //    Vector2 dir = aimCursor.position - firePoint.position;
                //    FireBullet(dir.normalized);
                //    rangedCooldownTimer = rangedCooldown;
                //}
            }

            UpdateHeat();
        }

    }

    void FireBullet(Vector2 dir)
    {
        anim.SetTrigger("Shoot");
        shoulder.transform.rotation = Quaternion.FromToRotation(Vector3.up, -dir);

        if (!bulletPrefab || !firePoint) return;

        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();         
        bullet.playerOwned = true;
        bullet.damage = bulletDamage;
        bullet.speed = bulletSpeed;
        bullet.knockback = bulletKnockback;
        bullet.Init(dir);
        AudioManager.PlayEffect(SoundType.PLAYER_SHOOT);

    }

    void UpdateHeat()
    {
        heatGauge.value = heat;

        if (heat > 0)
        {
            heat -= (Time.deltaTime*coolModifier);

            heatGauge.gameObject.SetActive(true);
        }
        else
        {
            heatGauge.gameObject.SetActive(false);
            if (disabled)
            {
                disabled = false;
                heatGaugeFill.color = heatColor;
            }
        }

        if (disabled && heat <= 0)
        {
            disabled = false;
            heatGaugeFill.color = heatColor;
        }

        if (heat >= maxHeat)
        {
            disabled = true;
            heatGaugeFill.color = Color.red;
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
    public void IncreaseHeat(int amount)
    {
        maxHeat += amount;
        heatGauge.maxValue = maxHeat;
    }
}
