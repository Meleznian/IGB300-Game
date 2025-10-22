using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

public class PlayerRangedAttack : MonoBehaviour
{
    [Header("Universal Variables")]
    [SerializeField] float sizeModifier = 1;
    [SerializeField] float speedModifier;
    [SerializeField] int damageModifier;
    [SerializeField] float knockbackModifier;
    [SerializeField] int pierceModifier;

    [Header("Bullet Variables")]
    [SerializeField] float bulletCooldown = 0.3f;
    [SerializeField] bool bulletUnlocked;
    [SerializeField] GameObject bulletPrefab;

    [Header("Spear Variables")]
    [SerializeField] float spearCooldown = 0.3f;
    [SerializeField] bool spearUnlocked;
    [SerializeField] GameObject spearPrefab;

    [Header("Axe Variables")]
    [SerializeField] float axeCooldown = 0.3f;
    [SerializeField] bool axeUnlocked;
    [SerializeField] GameObject axePrefab;

    float heatPerShot;
    float maxHeat;





    float heat;

    [Header("Components")]
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

    Coroutine bulletAttack;
    Coroutine spearAttack;
    Coroutine axeAttack;

    


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

        if (!autoing && autoAttack)
        {
            autoing = true;
            bulletAttack = StartCoroutine(BulletAttack());
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
                    FireBullet(dir.normalized, bulletPrefab);
                    rangedCooldownTimer = bulletCooldown;
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

    void FireBullet(Vector2 dir, GameObject projectilePrefab)
    {
        //anim.SetTrigger("Shoot");
        shoulder.transform.rotation = Quaternion.FromToRotation(Vector3.up, -dir);

        if (!bulletPrefab || !firePoint) return;

        Bullet projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity).GetComponent<Bullet>();

        if(projectile.projectileType == Bullet.ProjectileType.Spear)
        {
            projectile.transform.position += new Vector3(0, -0.55f, 0);
        }

        projectile.playerOwned = true;
        projectile.damage += damageModifier;
        projectile.speed += speedModifier;
        projectile.knockback += knockbackModifier;
        projectile.pierce += pierceModifier;
        projectile.transform.localScale = projectile.transform.localScale + new Vector3(sizeModifier, sizeModifier, sizeModifier);
        projectile.Init(dir);
        AudioManager.PlayEffect(SoundType.PLAYER_SHOOT);

    }

    void UpdateHeat()
    {
        heatGauge.value = heat;

        if (heat > 0)
        {
            //heat -= (Time.deltaTime*coolModifier);

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
        damageModifier += damageAmount;
        pierceModifier += pierceAmount;
    }
    internal void IncreaseSpeed(float move, float cool)
    {
        speedModifier += move;
        bulletCooldown -= cool;
    }
    internal void IncreaseKnockback(float amount)
    {
        knockbackModifier += amount;
    }
    public void IncreaseHeat(int amount)
    {
        maxHeat += amount;
        heatGauge.maxValue = maxHeat;
    }

    public void IncreaseSize(float amount)
    {
        sizeModifier += amount;
    }

    IEnumerator BulletAttack()
    {
        while (bulletUnlocked)
        {
            //Vector2 dir = aimCursor.position - firePoint.position;
            Vector3 closestEnemy = GetClosestEnemy();
            Vector2 dir = closestEnemy - firePoint.position;
            FireBullet(dir.normalized, bulletPrefab);
            Debug.Log("Grrrrr");
            yield return new WaitForSeconds(bulletCooldown);
        }
    }

    IEnumerator SpearAttack()
    {
        while (spearUnlocked)
        {
            FireBullet(Vector3.right, spearPrefab);
            yield return new WaitForSeconds(spearCooldown);
        }
    }
    IEnumerator AxeAttack()
    {
        while (axeUnlocked)
        {
            FireBullet(Vector3.right + Vector3.up, axePrefab);
            yield return new WaitForSeconds(axeCooldown);
        }
    }

    internal void Die()
    {
        if (bulletAttack != null)
        {
            StopCoroutine(bulletAttack);
        }
        if (spearAttack != null)
        {
            StopCoroutine(spearAttack);
        }
        aimCursor.gameObject.SetActive(false);
    }

    void ToggleAuto()
    {
        if (Input.GetKeyUp(KeyCode.O))
        {
            UnlockSpear();
            //autoAttack = !autoAttack;
            //if (autoing)
            //{
            //    autoing = false;
            //}
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            UnlockAxe();
            //autoAttack = !autoAttack;
            //if (autoing)
            //{
            //    autoing = false;
            //}
        }
    }

    internal void UnlockBullet()
    {
        bulletUnlocked = true;
        bulletAttack = StartCoroutine(BulletAttack());
    }

    internal void UnlockSpear()
    {
        spearUnlocked = true;
        spearAttack = StartCoroutine(SpearAttack());
    }
    internal void UnlockAxe()
    {
        axeUnlocked = true;
        axeAttack = StartCoroutine(AxeAttack());
    }

    private Vector3 GetClosestEnemy()
    {
        List<GameObject> enemies = EnemyManager.instance.livingEnemiesList;
        GameObject closestEnemy = enemies[0];
        float closestDistance = Vector3.Distance(closestEnemy.transform.position, gameObject.transform.position);
        for(int i = 1; i < enemies.Count - 1; i++)
        {
            if (Vector3.Distance(enemies[i].transform.position, gameObject.transform.position) < closestDistance)
            {
                closestEnemy = enemies[i];
                closestDistance = Vector3.Distance(enemies[i].transform.position, gameObject.transform.position);
            }
        }
        return closestEnemy.transform.position;
    }
}
