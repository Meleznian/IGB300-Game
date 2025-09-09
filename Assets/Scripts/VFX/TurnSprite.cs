using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TurnSprite : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    //[SerializeField] float velocity;
    [SerializeField] bool useVelocity;
    bool right;

    InputAction moveAction;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        if (useVelocity)
        {
            CheckMovement();
        }
    }

    void CheckMovement()
    {
        float horizontalMove = moveAction.ReadValue<Vector2>().x;

        if (horizontalMove > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (horizontalMove < -0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        //velocity = Vector3.Dot(rb.linearVelocity, Vector3.right);
    }

    internal void Flip()
    {
        right = !right;

        if (right)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (!right)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

}
