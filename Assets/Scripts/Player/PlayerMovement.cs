using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Required Components
    private Rigidbody2D rb;
    private TrailRenderer tr;
    [SerializeField ]private Animator anim;

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

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(moveAction.ReadValue<Vector2>());
        //int oldHMove = horizontalMove;
        horizontalMove = moveAction.ReadValue<Vector2>().x > 0 ? 1 : 0;
        horizontalMove = moveAction.ReadValue<Vector2>().x < 0 ? -1 : horizontalMove;
        verticalMove = moveAction.ReadValue<Vector2>().y > 0 ? 1 : 0;
        verticalMove = moveAction.ReadValue<Vector2>().y < 0 ? -1 : verticalMove;

        if (horizontalMove != 0)
        {
            anim.SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
        }

        //if (horizontalMove != oldHMove) velocityAdded = false;
        //Debug.Log(moveAction.ReadValue<Vector2>().y);
        //Debug.Log(verticalMove);
        //horizontalMove = Convert.ToInt32(Input.GetKey(KeyCode.D)) - Convert.ToInt32(Input.GetKey(KeyCode.A));
        //verticalMove = Convert.ToInt32(Input.GetKey(KeyCode.W)) - Convert.ToInt32(Input.GetKey(KeyCode.S));

        //Resets Jump Counter if Grounded
        //jumps = isGrounded ? 0 : jumps;
        //jumped = isGrounded ? false : jumped;

        _lastDirection = Input.GetKeyDown(KeyCode.D) ? 1 : _lastDirection;
        _lastDirection = Input.GetKeyDown(KeyCode.A) ? -1 : _lastDirection;

        if(_isGrounded && !_sprinting) _sprintAvailable = true;

        //if(!shouldJump || jumped) shouldJump = Input.GetKeyDown(KeyCode.Space);
        if (jumpAction.WasPressedThisFrame() && (_isGrounded || !_isGrounded && !_airJump)) Jump();

        //Makes the player sprint if able to
        if (dashAction.WasPressedThisFrame() && _sprintAvailable) StartCoroutine(Sprint(horizontalMove == 0 && verticalMove == 0 ? _lastDirection : horizontalMove, verticalMove));

        //sprintReleased = !dashAction.IsPressed();
        //jumpReleased = !jumpAction.IsPressed();
    }

    private void FixedUpdate()
    {
        if(!_sprinting) Move(horizontalMove, verticalMove);
        //if (shouldJump) Jump();
    }

    void Jump()
    {
        Debug.Log("Jumping");
        //isGrounded = false;
        rb.linearVelocityY = 0;
        rb.AddForce(transform.up * _jumpSpeed, ForceMode2D.Impulse);

        if (!_isGrounded) _airJump = true;
        //jumped = true;

        //Adds to the Jump Counter
        //jumps++;
    }

    void Move(int x, int y)
    {
        //Debug.Log($"Moving: {x}, {y}");
        //Moves the Player Horizontally
        //if(!sprinting) rb.AddForce((transform.right * x * speed) - new Vector3(rb.linearVelocity.x, 0, 0), ForceMode2D.Impulse);
        //if (x != 0) rb.AddForce((transform.right * x * speed) - new Vector3(rb.linearVelocity.x, 0, 0), ForceMode2D.Impulse);
        //transform.position = new Vector3(transform.position.x + (x * speed), transform.position.y, 0);
        //rb.linearVelocityX += x * speed;
        //velocityAdded = true;

        float newVelocity = Mathf.Clamp(Mathf.Abs(rb.linearVelocityX), _speed, int.MaxValue);
        rb.linearVelocityX = newVelocity * x;



        /*//Handles Jumping
        if ((isGrounded || jumps < 2) && shouldJump) Jump();
        */

    }


    
    /// <summary>
    /// The function handling the player sprint mechanics
    /// </summary>
    /// <param name="x">The X direction to sprint in (-1 = left, 0 = none, 1 = right)</param>
    /// <param name="y">The Y direction to sprint in (-1 = down, 0 = none, 1 = up)</param>
    /// <returns>N/A (handled in corouting)</returns>
    private IEnumerator Sprint(int x, int y)
    {
        //Debug.Log("Sprinting");

        //Sets state variables related to a sprint
        _sprintAvailable = false;
        _sprinting = true;
        tr.emitting = true;

        //Sets the velocity to sprint
        rb.linearVelocity = new Vector2(x * _sprintSpeed, y * _sprintSpeed);

        //Turns off physics Components
        rb.linearDamping = 0;
        rb.gravityScale = 0;

        //Waits for Sprint to conclude
        yield return new WaitForSeconds(_sprintTime);
        
        //Resets state variables
        _sprinting = false;
        tr.emitting = false;

        //Resets physics components (which will slow and stop the player's motion)
        rb.linearDamping = _friction;
        rb.gravityScale = gravScale;
    }

    public void LandedOnGround()
    {
        anim.SetBool("Jumping", false);
        _isGrounded = true;
        _airJump = false;
    }

    public void LeftGround()
    {
        anim.SetBool("Jumping", true);
        _isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Collectable")
        {
            other.gameObject.GetComponent<ICollectable>().Collect();
        }
    }
}
