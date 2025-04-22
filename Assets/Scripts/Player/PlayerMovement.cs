using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Required Components
    private Rigidbody2D rb;
    private TrailRenderer tr;

    //Input Actions
    InputAction moveAction;
    InputAction jumpAction;
    InputAction dashAction;

    //Variables that relate to multiple movement components
    [Header("Universal Variables")]
    [SerializeField] int horizontalMove;
    [SerializeField] int verticalMove;
    [SerializeField] private bool isGrounded;
    [SerializeField] private int lastDirection = 1;


    //Variables that only relate to default movement (WASD)
    [Header("Movement Variables")]
    [SerializeField] private float speed;
    private bool velocityAdded;

    //Variables that only relate to sprinting/dashing (left shift)
    [Header("Dash/Sprint Variables")]
    [SerializeField] private bool sprintAvailable;
    [SerializeField] private float sprintSpeed;
    private bool sprinting;
    [SerializeField] private float sprintTime;
    [SerializeField] private float friction;
    private bool sprintReleased;

    //Variables that only relate to jumping (spacebar)
    [Header("Jump Variables")]
    //[SerializeField] bool shouldJump;
    [SerializeField] bool jumped;
    [SerializeField] private float jumpSpeed;
    //[SerializeField] private int jumps;
    [SerializeField] private bool airJump;
    private bool jumpReleased;





    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();

        rb.linearDamping = friction;

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashAction = InputSystem.actions.FindAction("Sprint");

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

        //if (horizontalMove != oldHMove) velocityAdded = false;
        //Debug.Log(moveAction.ReadValue<Vector2>().y);
        //Debug.Log(verticalMove);
        //horizontalMove = Convert.ToInt32(Input.GetKey(KeyCode.D)) - Convert.ToInt32(Input.GetKey(KeyCode.A));
        //verticalMove = Convert.ToInt32(Input.GetKey(KeyCode.W)) - Convert.ToInt32(Input.GetKey(KeyCode.S));

        //Resets Jump Counter if Grounded
        //jumps = isGrounded ? 0 : jumps;
        //jumped = isGrounded ? false : jumped;

        lastDirection = Input.GetKeyDown(KeyCode.D) ? 1 : lastDirection;
        lastDirection = Input.GetKeyDown(KeyCode.A) ? -1 : lastDirection;

        if(isGrounded && !sprinting) sprintAvailable = true;

        //if(!shouldJump || jumped) shouldJump = Input.GetKeyDown(KeyCode.Space);
        if (jumpAction.WasPressedThisFrame() && (isGrounded || !isGrounded && !airJump)) Jump();

        //Makes the player sprint if able to
        if (dashAction.WasPressedThisFrame() && sprintAvailable) StartCoroutine(Sprint(horizontalMove == 0 && verticalMove == 0 ? lastDirection : horizontalMove, verticalMove));

        //sprintReleased = !dashAction.IsPressed();
        //jumpReleased = !jumpAction.IsPressed();
    }

    private void FixedUpdate()
    {
        if(!sprinting) Move(horizontalMove, verticalMove);
        //if (shouldJump) Jump();
    }

    void Jump()
    {
        Debug.Log("Jumping");
        //isGrounded = false;
        rb.linearVelocityY = 0;
        rb.AddForce(transform.up * jumpSpeed, ForceMode2D.Impulse);

        if (!isGrounded) airJump = true;
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

        float newVelocity = Mathf.Clamp(Mathf.Abs(rb.linearVelocityX), speed, int.MaxValue);
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
        sprintAvailable = false;
        sprinting = true;
        tr.emitting = true;

        //Sets the velocity to sprint
        rb.linearVelocity = new Vector2(x * sprintSpeed, y * sprintSpeed);

        //Turns off physics Components
        rb.linearDamping = 0;
        rb.gravityScale = 0;

        //Waits for Sprint to conclude
        yield return new WaitForSeconds(sprintTime);
        
        //Resets state variables
        sprinting = false;
        tr.emitting = false;

        //Resets physics components (which will slow and stop the player's motion)
        rb.linearDamping = friction;
        rb.gravityScale = 3;


    }

    public void LandedOnGround()
    {
        isGrounded = true;
        airJump = false;
    }

    public void LeftGround()
    {
        isGrounded = false;
    }
}
