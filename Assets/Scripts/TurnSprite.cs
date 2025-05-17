using UnityEngine;
using UnityEngine.UIElements;

public class TurnSprite : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float velocity;

    private void Update()
    {
       CheckMovement();    
    }

    void CheckMovement()
    {
        if (Vector3.Dot(rb.linearVelocity, Vector3.right) > 0.1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Vector3.Dot(rb.linearVelocity, Vector3.right) < -0.1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        velocity = Vector3.Dot(rb.linearVelocity, Vector3.right);
    }

}
