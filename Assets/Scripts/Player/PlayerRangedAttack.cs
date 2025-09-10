using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerRangedAttack : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float bulletSpeed;
    [SerializeField] int bulletDamage;
    [SerializeField] float bulletKnockback;
    [SerializeField] int bulletPierce;
    [SerializeField] float bulletSize = 1;
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
    [SerializeField] ParticleSystem coolEffect;
    [SerializeField] bool autoAttack;
    Color heatColor;

    bool disabled;

    Coroutine attack;

    


    float rangedCooldownTimer = 0f;

    private void Start()
    {
        print("Gun Setup");
        heatColor = heatGaugeFill.color;
        heatGauge.maxValue = maxHeat;
        print(heatGauge.maxValue);


    }

    bool autoing;

    void Update()
    {

        ToggleAuto();

        if (autoAttack && !autoing)
        {
            autoing = true;
            attack = StartCoroutine(AutoAttack());
        }

        if (!GameManager.instance.playerDead)
        {
            rangedCooldownTimer -= Time.deltaTime;



            if (!disabled && !autoAttack)
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

            if (!autoAttack)
            {
                UpdateHeat();
            }
        }

    }

    void FireBullet(Vector2 dir)
    {
        //anim.SetTrigger("Shoot");
        shoulder.transform.rotation = Quaternion.FromToRotation(Vector3.up, -dir);

        if (!bulletPrefab || !firePoint) return;

        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();         
        bullet.playerOwned = true;
        bullet.damage = bulletDamage;
        bullet.speed = bulletSpeed;
        bullet.knockback = bulletKnockback;
        bullet.pierce = bulletPierce;
        bullet.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
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
        }

        if (disabled && heat <= 0)
        {
            disabled = false;
            coolEffect.Stop();
            heatGaugeFill.color = heatColor;
        }

        if (heat >= maxHeat)
        {
            disabled = true;
            coolEffect.Play();
            heatGaugeFill.color = Color.red;
        }
    }



    internal void IncreaseDamage(int damageAmount, int pierceAmount)
    {
        bulletDamage += damageAmount;
        bulletPierce += pierceAmount;
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

    public void IncreaseSize(float amount)
    {
        bulletSize += amount;
    }

    IEnumerator AutoAttack()
    {
        while (autoAttack)
        {
            if (!disabled)
            {
                Vector2 dir = aimCursor.position - firePoint.position;
                FireBullet(dir.normalized);
                heat += heatPerShot;
            }
            yield return new WaitForSeconds(rangedCooldown);
        }
    }

    internal void Die()
    {
        StopCoroutine(attack);
        aimCursor.gameObject.SetActive(false);
    }

    void ToggleAuto()
    {
        if (Input.GetKeyUp(KeyCode.O))
        {
            autoAttack = !autoAttack;
            if (autoing)
            {
                autoing = false;
            }
        }
    }
}
