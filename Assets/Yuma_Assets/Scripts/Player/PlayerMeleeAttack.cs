using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField] Transform up, down, left, right;
    [SerializeField] float range = 1f;
    [SerializeField] int damage = 1;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] GameObject flashFX;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (mouse - (Vector2)transform.position).normalized;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            angle = (angle + 360) % 360;

            Transform point = right;
            Quaternion rot = Quaternion.identity;

            if (angle >= 45 && angle < 135)
            {
                point = up;
                rot = Quaternion.Euler(0, 0, 90);
            }
            else if (angle >= 135 && angle < 225)
            {
                point = left;
                rot = Quaternion.Euler(0, 0, 180);
            }
            else if (angle >= 225 && angle < 315)
            {
                point = down;
                rot = Quaternion.Euler(0, 0, 270);
            }

            SpawnEffect(point.position, rot);
            DealDamage(point.position);
        }
    }

    void DealDamage(Vector2 origin)
    {
        var hits = Physics2D.OverlapCircleAll(origin, range, enemyLayer);
        foreach (var h in hits)
        {
            var enemy = h.GetComponent<EnemyBase>();
            if (enemy != null)
                enemy.TakeDamage(damage);
        }
    }

    void SpawnEffect(Vector2 pos, Quaternion rot)
    {
        if (flashFX)
            Instantiate(flashFX, pos, rot);
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
