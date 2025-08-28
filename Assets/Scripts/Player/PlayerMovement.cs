using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    //Required Components
    private Rigidbody2D rb;
    private TrailRenderer tr;
    [SerializeField] private Animator anim;

    //Input Actions
    InputAction moveAction;
    InputAction jumpAction;
    InputAction dashAction;

    //Variables that relate to multiple movement components
    [Header("Universal Variables")]
    [SerializeField] int horizontalMove;
    [SerializeField] int verticalMove;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private int _lastDirection = 1;

    //Variables that only relate to default movement (WASD)
    [Header("Movement Variables")]
    [SerializeField] private float _speed;
    private bool _velocityAdded;

    //Variables that only relate to sprinting/dashing (left shift)
    [Header("Dash/Sprint Variables")]
    [SerializeField] private bool _sprintAvailable;
    [SerializeField] private float _sprintSpeed;
    private bool _sprinting;
    [SerializeField] private float _sprintTime;
    [SerializeField] private float _friction;
    private bool _sprintReleased;

    //Variables that only relate to jumping (spacebar)
    [Header("Jump Variables")]
    //[SerializeField] bool shouldJump;
    [SerializeField] bool jumped;
    [SerializeField] private float _jumpSpeed;
    //[SerializeField] private int jumps;
    [SerializeField] private bool _airJump;
    private bool _jumpReleased;

    private float gravScale;

    // Components for synthesizing external forces (such as knockback)
    [Header("External Impulse (Knockback)")]
    [SerializeField] private float impulseDamping = 6f; // Rate of attenuation (the higher the value, the faster it disappears)
    private Vector2 externalImpulse;                    //ÅgSuperimposed velocityÅh component of the external force applied
    [SerializeField] bool autoRun;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();

        rb.linearDamping = _friction;

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashAction = InputSystem.actions.FindAction("Sprint");

        gravScale = rb.gravityScale;

        if (autoRun)
        {
            horizontalMove = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ToggleAutoRun();

        if (!autoRun)
        {
            horizontalMove = moveAction.ReadValue<Vector2>().x > 0 ? 1 : 0;
            horizontalMove = moveAction.ReadValue<Vector2>().x < 0 ? -1 : horizontalMove;
        }
        verticalMove = moveAction.ReadValue<Vector2>().y > 0 ? 1 : 0;
        verticalMove = moveAction.ReadValue<Vector2>().y < 0 ? -1 : verticalMove;

        anim.SetBool("Running", horizontalMove != 0);

        _lastDirection = Input.GetKeyDown(KeyCode.D) ? 1 : _lastDirection;
        _lastDirection = Input.GetKeyDown(KeyCode.A) ? -1 : _lastDirection;

        if (_isGrounded && !_sprinting) _sprintAvailable = true;

        if (jumpAction.WasPressedThisFrame() && (_isGrounded || !_isGrounded && !_airJump)) Jump();

        //Makes the player sprint if able to
        if (dashAction.WasPressedThisFrame() && _sprintAvailable)
            StartCoroutine(Sprint(horizontalMove == 0 && verticalMove == 0 ? _lastDirection : horizontalMove, verticalMove));
    }

    private void FixedUpdate()
    {
        if (!_sprinting) Move(horizontalMove, verticalMove);

        // Reflect vertical external forces here (horizontal forces are synthesized within Move).
        if (Mathf.Abs(externalImpulse.y) > 0.0001f)
            rb.linearVelocityY = rb.linearVelocityY + externalImpulse.y;

        // Decay of external forces (towards 0 over time)
        if (externalImpulse.sqrMagnitude > 0.0001f)
            externalImpulse = Vector2.MoveTowards(externalImpulse, Vector2.zero, impulseDamping * Time.fixedDeltaTime);
        else
            externalImpulse = Vector2.zero;
    }

    void Jump()
    {
        Debug.Log("Jumping");
        rb.linearVelocityY = 0;
        rb.AddForce(transform.up * _jumpSpeed, ForceMode2D.Impulse);

        if (!_isGrounded)
        {
            _airJump = true;
            AudioManager.PlayEffect(SoundType.DOUBLEJUMP, 0.8f);
        }
        else
        {
            AudioManager.PlayEffect(SoundType.JUMP);
        }
    }

    void Move(int x, int y)
    {
        // Lateral velocity of the foundation based on input
        float newVelocity = Mathf.Clamp(Mathf.Abs(rb.linearVelocityX), _speed, int.MaxValue);
        float baseX = newVelocity * x;

        // The lateral force is synthesized and converted into the final velocity.
        float finalX = baseX + externalImpulse.x;
        rb.linearVelocityX = finalX;
    }

    /// <summary>
    /// The function handling the player sprint mechanics
    /// </summary>
    /// <param name="x">The X direction to sprint in (-1 = left, 0 = none, 1 = right)</param>
    /// <param name="y">The Y direction to sprint in (-1 = down, 0 = none, 1 = up)</param>
    /// <returns>N/A (handled in coroutine)</returns>
    private IEnumerator Sprint(int x, int y)
    {
        _sprintAvailable = false;
        _sprinting = true;
        tr.emitting = true;

        // Dash sets the speed directly.
        rb.linearVelocity = new Vector2(x * _sprintSpeed, y * _sprintSpeed);

        rb.linearDamping = 0;
        rb.gravityScale = 0;

        AudioManager.PlayEffect(SoundType.DASH);

        yield return new WaitForSeconds(_sprintTime);

        _sprinting = false;
        tr.emitting = false;

        rb.linearDamping = _friction;
        rb.gravityScale = gravScale;
    }

    public void LandedOnGround()
    {
        anim.SetBool("Jumping", false);
        _isGrounded = true;
        _airJump = false;
        AudioManager.PlayEffect(SoundType.LANDED, 0.2f);
    }

    public void LeftGround()
    {
        anim.SetBool("Jumping", true);
        _isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Collectable")
        {
            other.gameObject.GetComponent<ICollectable>().Collect();
        }
    }

    internal void IncreaseSpeed(float amount)
    {
        _speed += amount;
        _sprintSpeed += amount;
    }

    internal void Die()
    {
        _speed = 0;
    }

    // API that applies knockback, etc. from outside
    public void ApplyExternalImpulse(Vector2 impulse)
    {
        externalImpulse += impulse;
    }

    // For determining the knockback direction based on position
    public void ApplyKnockbackFrom(Vector2 sourcePosition, float power)
    {
        Vector2 dir = ((Vector2)transform.position - sourcePosition).normalized;
        externalImpulse += dir * power;
    }

    void ToggleAutoRun()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            autoRun = !autoRun;

            if (autoRun)
            {
                horizontalMove = 1;
            }
        }
    }

}