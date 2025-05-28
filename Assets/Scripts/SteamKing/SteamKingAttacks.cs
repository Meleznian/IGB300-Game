using NUnit.Framework.Constraints;
using UnityEngine;
using static UnityEngine.UI.Image;

public class SteamKingAttacks : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] SteamKing steamKingScript;
    PlayerHealth playerHealth;
    Rigidbody playerRb;
    [SerializeField] GameObject slashEffect;
    //[SerializeField] GameObject thrustEffect;
    [SerializeField] LayerMask ignore;
    [SerializeField] GameObject bullet;
    
    

    [Header("Attack Damages")]
    [SerializeField] int slashDamage;
    [SerializeField] int thrustDamage;
    [SerializeField] int kickDamage;
    [SerializeField] int bulletDamage;

    [Header("Attack Knockbacks")]
    [SerializeField] float slashknockBack;
    [SerializeField] float thrustknockBack;
    [SerializeField] float kickknockBack;
    [SerializeField] float bulletknockBack;

    [Header("Bullet Speeds")]
    [SerializeField] float dodgeBulletSpeed;
    [SerializeField] float aimedBulletSpeed;

    [Header("Attack Transforms")]
    [SerializeField] Transform slashPoint;
    [SerializeField] Vector2 slashSize;
    [SerializeField] Transform thrustPoint;
    [SerializeField] Vector2 thrustSize;
    [SerializeField] Transform kickPoint;
    [SerializeField] Vector2 kickSize;
    [SerializeField] Transform dodgeFirePoint;

    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        playerRb = playerHealth.GetComponent<Rigidbody>();  

    }

    public void GetNextAction()
    {
        steamKingScript.GetNextAction();
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
                //playerRb.AddForce(thrustknockBack);
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
                //playerRb.AddForce(thrustknockBack);
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

    }
}
