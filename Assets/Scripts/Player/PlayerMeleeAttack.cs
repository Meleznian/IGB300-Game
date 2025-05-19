using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField] Transform up, down, left, right;
    [SerializeField] float range = 1f;
    [SerializeField] int damage = 1;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] GameObject flashFX;
    [SerializeField] Animator anim;

    void Update()
    {
        // Gamepad input: Left stick + R1 button
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            Vector2 stick = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (stick.magnitude > 0.5f)
                TryAttack(stick.normalized);
        }

        // Mouse input: left click
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (mouse - (Vector2)transform.position).normalized;
            TryAttack(dir);
        }
    }

    void TryAttack(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;

        Transform point = right;
        Quaternion rot = Quaternion.identity;

        if (angle >= 45f && angle < 135f)
        {
            point = up;
            rot = Quaternion.Euler(0, 0, 90);
        }
        else if (angle >= 135f && angle < 225f)
        {
            point = left;
            rot = Quaternion.Euler(0, 0, 180);
        }
        else if (angle >= 225f && angle < 315f)
        {
            point = down;
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
                enemy.TakeDamage(damage);
        }
    }

    void SpawnEffect(Vector2 pos, Quaternion rot)
    {
        if (flashFX)
            anim.SetTrigger("Slash");
            Instantiate(flashFX, pos, rot);
        AudioManager.PlayEffect(SoundType.SLASH);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        if (up) Gizmos.DrawWireSphere(up.position, range);
        if (down) Gizmos.DrawWireSphere(down.position, range);
        if (left) Gizmos.DrawWireSphere(left.position, range);
        if (right) Gizmos.DrawWireSphere(right.position, range);
    }
}
