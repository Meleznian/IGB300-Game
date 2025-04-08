using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private int jumps;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool sprintAvailable;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private bool sprinting;
    [SerializeField] private int lastDirection = 1;

    [SerializeField] private float timer = 0;
    [SerializeField] private float sprintCD;
    [SerializeField] private float sprintStart;

    [SerializeField] int horizontalMove;
    [SerializeField]int verticalMove;

    [SerializeField] bool shouldJump;
    [SerializeField] bool jumped;
    [SerializeField] bool shouldSprint;
    [SerializeField] bool sprinted;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer = Time.time;

        //Debug.Log(Input.GetKey(KeyCode.D));
        horizontalMove = Convert.ToInt32(Input.GetKey(KeyCode.D)) - Convert.ToInt32(Input.GetKey(KeyCode.A));
        verticalMove = Convert.ToInt32(Input.GetKey(KeyCode.W)) - Convert.ToInt32(Input.GetKey(KeyCode.S));

        //Resets Jump Counter if Grounded
        jumps = isGrounded ? 0 : jumps;
        jumped = isGrounded ? false : jumped;

        sprinted = sprinting ? sprinted : false;

        lastDirection = Input.GetKeyDown(KeyCode.D) ? 1 : lastDirection;
        lastDirection = Input.GetKeyDown(KeyCode.A) ? -1 : lastDirection;

        if(sprintCD + sprintStart < timer ) sprinting = false;
        if(isGrounded && !sprinting) sprintAvailable = true;

        if(!shouldJump || jumped) shouldJump = Input.GetKeyDown(KeyCode.Space);
        if(!shouldSprint || sprinted) shouldSprint = Input.GetKeyDown(KeyCode.LeftShift);

        
    }

    private void FixedUpdate()
    {
        Move(horizontalMove, verticalMove);
    }

    void Jump()
    {
        Debug.Log("Jumping");
        isGrounded = false;

        rb.AddForce(transform.up * jumpSpeed, ForceMode2D.Impulse);

        jumped = true;

        //Adds to the Jump Counter
        jumps++;
    }

    void Move(int x, int y)
    {
        //Debug.Log($"Moving: {x}, {y}");
        //Moves the Player Horizontally
        if(!sprinting) rb.AddForce((transform.right * x * speed) - new Vector3(rb.linearVelocity.x, 0, 0), ForceMode2D.Impulse);
        if (x != 0) rb.AddForce((transform.right * x * speed) - new Vector3(rb.linearVelocity.x, 0, 0), ForceMode2D.Impulse);
        //transform.position = new Vector3(transform.position.x + (x * speed), transform.position.y, 0);

        //Handles Jumping
        if ((isGrounded || jumps < 2) && shouldJump) Jump();

        if (sprintAvailable && shouldSprint) Sprint(x, y);

    }

    void Sprint(int x, int y)
    {
        Debug.Log("Sprinting");
        if (y != 0)
        {
            rb.AddForce((transform.right * x * sprintSpeed) + (transform.up * y * sprintSpeed), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce((transform.right * lastDirection * sprintSpeed) + (transform.up * y * sprintSpeed), ForceMode2D.Impulse);
        }
        sprintAvailable = false;
        sprinting = true;
        sprintStart = Time.time;
        sprinted = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger hit");
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground")) isGrounded = true;
    }
}
