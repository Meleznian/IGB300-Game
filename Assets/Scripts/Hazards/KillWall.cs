using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KillWall : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] Transform player;
    [SerializeField] string playerTag = "Player";
    [SerializeField] string enemyTag = "Enemy";

    [Header("Speed Mapping")]
    [SerializeField] float minSpeed = 1f;    // Near speed
    [SerializeField] float slowMinSpeed = 0.01f; //Min Speed when Near
    [SerializeField] float maxSpeed = 8f;    // Speed at Far
    [SerializeField] float slowMaxSpeed = 2f; //Max Speed when far
    [SerializeField] float nearDist = 10f;   // Below thisÅ®min
    [SerializeField] float farDist = 18f;   // No moreÅ®max

    [Header("Smoothing")]
    [SerializeField] float accel = 10f;      // Speed following target speed

    [SerializeField] float currentSpeed;
    [SerializeField] private bool slowed = false;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Color slowColor;
    [SerializeField] ParticleSystem slowEffect;
    [SerializeField] ParticleSystem landEffect;

    bool playerDead = false;
    internal bool began;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    void Awake()
    {
        if (!player)
        {
            var tagged = GameObject.FindGameObjectWithTag(playerTag);
            if (tagged) player = tagged.transform;
        }
        currentSpeed = minSpeed;
    }

    private void Start()
    {
        if(GameManager.instance.tutorial == true)
        {
            anim.SetBool("Tutorial", true);
            began = true;
        }
    }

    void FixedUpdate()
    {
        if (began)
        {
            if (!playerDead)
            {
                // Distance (wall Å® player's X difference). Forward is positive.
                float dx = player ? (player.position.x - transform.position.x) : 0f;

                // Linear map: NearÅ®0, FarÅ®1 (automatically clamps values outside range)
                float t = Mathf.InverseLerp(nearDist, farDist, dx);

                // Interpolate speed between min..max 
                float targetSpeed = slowed ? Mathf.Lerp(slowMinSpeed, slowMaxSpeed, t) : Mathf.Lerp(minSpeed, maxSpeed, t);

                // Smoothing
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accel * Time.deltaTime);

                // Horizontal movement
                transform.position += Vector3.right * (currentSpeed * Time.deltaTime);

                anim.speed = currentSpeed / minSpeed;
            }
            else
            {
                transform.position -= Vector3.right * (currentSpeed * Time.deltaTime);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            var hp = other.GetComponent<PlayerHealth>() ?? other.GetComponentInParent<PlayerHealth>();
            anim.SetTrigger("Attack");
            if (hp) hp.Kill();
            playerDead = true;
            StartCoroutine(Leave());
            return;
        }
        if (other.CompareTag(enemyTag))
        {
            var enemy = other.GetComponent<EnemyBase>();
            if (enemy) enemy.Die(false);
        }
    }

    IEnumerator slowKillWallCooldown()
    {
        slowed = true;
        sprite.color = slowColor;
        slowEffect.Play();
        yield return new WaitForSeconds(10f);
        slowed = false;
        sprite.color = Color.white;
        slowEffect.Stop();
    }

    public void SlowKillWallPickup()
    {
        StartCoroutine(slowKillWallCooldown());
    }

    IEnumerator Leave()
    {
        anim.SetBool("PlayerDead", true);
        currentSpeed = 0;

        yield return new WaitForSeconds(1f);

        anim.SetTrigger("Leave");
        currentSpeed = 2;
        transform.rotation = Quaternion.Euler(0, 180, 0); 
    }

    public void Landed()
    {
        landEffect.Play();
    }

    public void BeginWalking()
    {
        began = true;
        anim.SetBool("Talked", true);
    }
}
