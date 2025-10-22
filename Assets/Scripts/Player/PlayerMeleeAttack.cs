using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField] Transform up, down, left, right;
    [SerializeField] float range = 1f;
    [SerializeField] float detectRadius;
    [SerializeField] int damage = 1;
    [SerializeField] float knockback = 5f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] GameObject flashFX;
    [SerializeField] Animator anim;
    [SerializeField] bool autoAttack;
    internal bool shooting;
    //public bool attacking;
    Vector2 knockDirection;
    [SerializeField] ParticleSystem superChargeEffect;
    [SerializeField] int horizontalMove;
    [SerializeField] int verticalMove;
    private enum Directions{
        Up, Down, Left, Right
    }
    private Directions Direction;
    InputAction moveAction;

    [SerializeField] private int _lastDirection = 1;

    [SerializeField] public float meleeCooldown = 0.5f;
    float meleeCooldownTimer = 0f;

    bool autoing;
    public bool superCharge = false;

    Coroutine attack;

    void Start()
    {

        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        if (autoAttack && !autoing)
        {
            autoing = true;
            attack = StartCoroutine(AutoAttack());
        }

        ToggleAuto();

        if (!GameManager.instance.playerDead)
        { 

            meleeCooldownTimer -= Time.deltaTime;

            if (meleeCooldownTimer < -0.7f)
            {
                anim.SetInteger("Slashes", 0);
            }

            horizontalMove = moveAction.ReadValue<Vector2>().x > 0 ? 1 : 0;
            horizontalMove = moveAction.ReadValue<Vector2>().x < 0 ? -1 : horizontalMove;
        
            verticalMove = moveAction.ReadValue<Vector2>().y > 0 ? 1 : 0;
            verticalMove = moveAction.ReadValue<Vector2>().y < 0 ? -1 : verticalMove;

            _lastDirection = Input.GetKeyDown(KeyCode.D) ? 1 : _lastDirection;
            _lastDirection = Input.GetKeyDown(KeyCode.A) ? -1 : _lastDirection;

            if(verticalMove == 0 && horizontalMove == 0)
            {
                switch(_lastDirection)
                {
                    case -1:
                        Direction = Directions.Left; break;
                    case 1:
                        Direction = Directions.Right; break;
                    default:
                        Direction = Directions.Right; break;
                }
            }
            else if (verticalMove != 0)
            {
                switch (verticalMove)
                {
                    case -1:
                        Direction = Directions.Down; break;
                    case 1:
                        Direction = Directions.Up; break;
                    default:
                        Direction = Directions.Right; break;
                }
            }
            else if(horizontalMove != 0)
            {
                switch (horizontalMove)
                {
                    case -1:
                        Direction = Directions.Left; break;
                    case 1:
                        Direction = Directions.Right; break;
                    default:
                        Direction = Directions.Right; break;
                }
            }

            //Debug.Log(Direction);
            if (!autoAttack)
            {
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

        if (Input.GetKeyDown(KeyCode.I))
        {
            IncreaseSize(0.5f);
        }
    }

    void TryAttack(Vector2 dir)
    {
        if (!shooting)
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

            Debug.Log(point);
            SpawnEffect(point.position, rot);
            DealDamage(point.position);
        }
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
                //GameManager.instance.IncreaseGauge();
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
        Gizmos.DrawWireSphere(transform.position, detectRadius);
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
        up.position += new Vector3(0, amount, 0);
        down.position -= new Vector3(0, amount, 0);
        right.position += new Vector3(amount, 0, 0);
        left.position -= new Vector3(amount, 0, 0);
        detectRadius += amount*2;
    }

    IEnumerator AutoAttack()
    {
        while (autoAttack)
        {
            if (CheckForEnemy() || superCharge)
            {
                //Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //Vector2 dir = (mouse - (Vector2)transform.position).normalized;
                Vector2 dir = right.position;
                switch (Direction)
                {
                    case Directions.Up:
                        dir = up.localPosition;
                        break;
                    case Directions.Down:
                        dir = down.localPosition;
                        break;
                    case Directions.Left:
                        dir = left.localPosition;
                        break;
                    case Directions.Right:
                        dir = right.localPosition;
                        break;
                    default:
                        dir = right.localPosition;
                        break;
                }
                Debug.Log(Direction);
                Debug.Log(dir);
                TryAttack(dir);
                yield return new WaitForSeconds(superCharge ? 0.1f: meleeCooldown);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    internal void Die()
    {
        StopCoroutine(attack);
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

    bool CheckForEnemy()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, detectRadius, enemyLayer);
        foreach (var h in hits)
        {
            if (h.CompareTag("Enemy"))
            {
                return true;
            }
        }
        //print("No Enemy");
        return false;
    }

    IEnumerator SuperCharge()
    {
        superCharge = true;
        superChargeEffect.Play();
        yield return new WaitForSeconds(5f);
        superCharge = false;
    }

    public void RunSuperCharge()
    {
        StartCoroutine(SuperCharge());
    }
}
