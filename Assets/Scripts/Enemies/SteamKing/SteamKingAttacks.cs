using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SteamKingAttacks : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] SteamKing steamKingScript;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] GameObject slashEffect;
    [SerializeField] LayerMask ignore;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject slamWave;



    [Header("Attack Damages")]
    [SerializeField] int slashDamage;
    [SerializeField] int thrustDamage;
    [SerializeField] int kickDamage;
    [SerializeField] int bulletDamage;
    [SerializeField] int whipDamage;
    [SerializeField] int chargeDamage;
    [SerializeField] int slamDamage;


    [Header("Attack Knockbacks")]
    [SerializeField] float slashKnockback;
    [SerializeField] float thrustKnockback;
    [SerializeField] float kickKnockback;
    [SerializeField] float bulletKnockback;
    [SerializeField] float whipKnockback;
    [SerializeField] float chargeKnockback;
    [SerializeField] float slamKnockback;



    [Header("Bullet Speeds")]
    [SerializeField] float dodgeBulletSpeed;
    [SerializeField] float aimedBulletSpeed;
    [SerializeField] float slamSpeed;

    [Header("Attack Transforms")]
    [SerializeField] Transform slashPoint;
    [SerializeField] Vector2 slashSize;
    [SerializeField] Transform thrustPoint;
    [SerializeField] Vector2 thrustSize;
    [SerializeField] Transform kickPoint;
    [SerializeField] Vector2 kickSize;
    [SerializeField] Transform whipPoint;
    [SerializeField] Vector2 whipSize;
    [SerializeField] Transform dodgeFirePoint;
    [SerializeField] Transform aimedFirePoint;
    [SerializeField] Transform slamPoint;

    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        playerRb = playerHealth.GetComponent<Rigidbody2D>();  

    }

    public void GetNextAction()
    {
        steamKingScript.GetNextAction();
    }

    public void Phase2Get()
    {
        steamKingScript.Phase2GetNextAction();
    }

    public void StartCharge()
    {
        steamKingScript.StartCharge();
    }

    public void Slash()
    {
        var effect = Instantiate(slashEffect, slashPoint.position, slashPoint.rotation);
        effect.transform.localScale = slashSize;

        var hit = Physics2D.OverlapBox(slashPoint.position, slashSize, 0f, ~ignore);
        if (hit != null)
        {
            print(hit.gameObject);
            if (hit.gameObject == playerHealth.gameObject)
            {
                print("Player Hit");
                Vector2 dir = GetDirection();
                //playerRb.AddForce(dir * slashKnockback, ForceMode2D.Impulse);
                playerHealth.TakeDamage(slashDamage);

            }
        }
        else
        {
            print("Nothing Hit");
        }
    }

    public void Thrust()
    {
        var effect = Instantiate(slashEffect, thrustPoint.position, thrustPoint.rotation);
        effect.transform.localScale = thrustSize;


        var hit = Physics2D.OverlapBox(thrustPoint.position, thrustSize, 0f, ~ignore);
        if (hit != null)
        {
            print(hit.gameObject);
            if (hit.gameObject == playerHealth.gameObject)
            {
                print("Player Hit");
                Vector2 dir = GetDirection();
                playerRb.AddForce(dir * thrustKnockback, ForceMode2D.Impulse);
                playerHealth.TakeDamage(thrustDamage);

            }
        }
        else
        {
            print("Nothing Hit");
        }
    }

    public void Kick()
    {
        var effect = Instantiate(slashEffect, kickPoint.position, kickPoint.rotation);
        effect.transform.localScale = kickSize;


        var hit = Physics2D.OverlapBox(kickPoint.position, kickSize, 0f, ~ignore);
        if (hit != null)
        {
            print(hit.gameObject);
            if (hit.gameObject == playerHealth.gameObject)
            {
                print("Player Hit");
                Vector2 dir = GetDirection();
                playerRb.AddForce(dir * kickKnockback, ForceMode2D.Impulse);
                playerHealth.TakeDamage(kickDamage);
            }
        }
        else
        {
            print("Nothing Hit");
        }
    }

    int shots;
    public void DodgeShoot()
    {
        if (transform.rotation == Quaternion.Euler(0, 180, 0))
        {
            if (shots == 0)
            {
                FireBullet(new Vector2(-0.5f, 0.5f), dodgeFirePoint, dodgeBulletSpeed);
                shots++;
            }
            else if (shots == 1)
            {
                FireBullet(Vector2.left, dodgeFirePoint, dodgeBulletSpeed);
                shots++;
            }
            else if (shots == 2)
            {
                FireBullet(new Vector2(-0.5f, -0.5f), dodgeFirePoint, dodgeBulletSpeed);
                shots = 0;
            }
        }
        else
        {
            if (shots == 0)
            {
                FireBullet(new Vector2(0.5f, -0.5f), dodgeFirePoint, dodgeBulletSpeed);
                shots++;
            }
            else if (shots == 1)
            {
                FireBullet(Vector2.right, dodgeFirePoint, dodgeBulletSpeed);
                shots++;
            }
            else if (shots == 2)
            {
                FireBullet(new Vector2(0.5f, 0.5f), dodgeFirePoint, dodgeBulletSpeed);
                shots = 0;
            }
        }
    }


    void FireBullet(Vector2 direction, Transform firePoint, float speed)
    {
        Bullet newBullet = Instantiate(bullet, firePoint.position, Quaternion.identity).GetComponent<Bullet>();

        newBullet.Init(direction);
        newBullet.damage = bulletDamage;
        newBullet.originallyEnemy = true;
        newBullet.speed = speed;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        if (slashPoint) Gizmos.DrawWireCube(slashPoint.position, slashSize);
        if (thrustPoint) Gizmos.DrawWireCube(thrustPoint.position, thrustSize);
        if (kickPoint) Gizmos.DrawWireCube(kickPoint.position, kickSize);
        if (whipPoint) Gizmos.DrawWireCube(whipPoint.position, whipSize);

    }

    Vector2 GetDirection()
    {
        Vector2 direction = transform.position - playerHealth.transform.position;
        direction = direction.normalized;
        print(direction);
        return -direction;
    }

    public void ChainWhip()
    {
        var effect = Instantiate(slashEffect, whipPoint.position, whipPoint.rotation);
        effect.transform.localScale = thrustSize;


        var hit = Physics2D.OverlapBox(whipPoint.position, whipSize, 0f, ~ignore);
        if (hit != null)
        {
            print(hit.gameObject);
            if (hit.gameObject == playerHealth.gameObject)
            {
                print("Player Hit");
                Vector2 dir = GetDirection();
                playerRb.AddForce(dir * whipKnockback, ForceMode2D.Impulse);
                playerHealth.TakeDamage(whipDamage);

            }
        }
        else
        {
            print("Nothing Hit");
        }
    }

    public void AimedShot()
    {
        Vector2 dir = playerHealth.transform.position - transform.position;
        FireBullet(dir,aimedFirePoint,aimedBulletSpeed);
    }

    public void ChargeHit()
    {
        Vector2 dir = GetDirection();
        playerRb.AddForce(dir * chargeKnockback, ForceMode2D.Impulse);
        playerHealth.TakeDamage(chargeDamage);
    }

    public void DiveSlam()
    {
        Shockwave wave = Instantiate(slamWave, slamPoint.position, Quaternion.identity).GetComponent<Shockwave>();
        wave.Setup(slamDamage,slamSpeed,slamKnockback, Vector2.left);
        wave = Instantiate(slamWave, slamPoint.position, Quaternion.identity).GetComponent<Shockwave>();
        wave.Setup(slamDamage, slamSpeed, slamKnockback, Vector2.right);
    }


    public void TransitionToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
