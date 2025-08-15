using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField] Transform up, down, left, right;
    [SerializeField] float range = 1f;
    [SerializeField] int damage = 1;
    [SerializeField] float knockback = 5f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] GameObject flashFX;
    [SerializeField] Animator anim;
    Vector2 knockDirection;

    [SerializeField] float meleeCooldown = 0.5f;
    float meleeCooldownTimer = 0f;

    void Update()
    {
        if (!GameManager.instance.playerDead)
        { 

            meleeCooldownTimer -= Time.deltaTime;

            if (meleeCooldownTimer < -0.7f)
            {
                anim.SetInteger("Slashes", 0);
            }

            // Gamepad input: Left stick + R1 button
            if (Input.GetKeyDown(KeyCode.JoystickButton5) && meleeCooldownTimer <= 0f)
            {
                Vector2 stick = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                if (stick.magnitude > 0.5f)
                {
                    TryAttack(stick.normalized);
                    meleeCooldownTimer = meleeCooldown;
                }
            }

            // Mouse input: left click
            if (Input.GetMouseButtonDown(0) && meleeCooldownTimer <= 0f)
            {
                Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 dir = (mouse - (Vector2)transform.position).normalized;
                TryAttack(dir);
                meleeCooldownTimer = meleeCooldown;
            }
        }
    }

    void TryAttack(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;

        Transform point = right;
        knockDirection = Vector2.right;
        Quaternion rot = Quaternion.identity;

        if (angle >= 45f && angle < 135f)
        {
            point = up;
            knockDirection = Vector2.up;
            rot = Quaternion.Euler(0, 0, 90);
        }
        else if (angle >= 135f && angle < 225f)
        {
            point = left;
            knockDirection = Vector2.left;
            rot = Quaternion.Euler(0, 0, 180);
        }
        else if (angle >= 225f && angle < 315f)
        {
            point = down;
            knockDirection = Vector2.down;
            rot = Quaternion.Euler(0, 0, 270);
        }

        SpawnEffect(point.position, rot);
        DealDamage(point.position);
    }

    void DealDamage(Vector2 origin)
    {
        var hits = Physics2D.OverlapCircleAll(origin, range, enemyLayer);
        

        foreach (var h in hits)
        {
            var enemy = h.GetComponent<IDamageable>();
            if (enemy != null)
            {
                print(h.gameObject.name);
                enemy.TakeDamage(damage);
                AudioManager.PlayEffect(SoundType.ENEMY_DAMAGE);
                if (h.GetComponent<Rigidbody2D>() != null)
                {
                    h.GetComponent<Rigidbody2D>().AddForce(knockDirection*knockback, ForceMode2D.Impulse);
                }
                GameManager.instance.IncreaseGauge();
            }
        }
    }

    void SpawnEffect(Vector2 pos, Quaternion rot)
    {
        if (flashFX)
        {
            var effect = Instantiate(flashFX, pos, rot);
            effect.transform.localScale = new Vector3(range,range,range);
        }

        AudioManager.PlayEffect(SoundType.SLASH);

        anim.SetTrigger("Slash");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        if (up) Gizmos.DrawWireSphere(up.position, range);
        if (down) Gizmos.DrawWireSphere(down.position, range);
        if (left) Gizmos.DrawWireSphere(left.position, range);
        if (right) Gizmos.DrawWireSphere(right.position, range);
    }

    internal void IncreaseDamage(int amount)
    {
        damage += amount;
    }
    internal void IncreaseSpeed(float amount)
    {
        meleeCooldown -= amount;
    }
    internal void IncreaseKnockback(float amount)
    {
        knockback += amount;
    }
    public void IncreaseSize(float amount)
    {
        range += amount;
    }

}
