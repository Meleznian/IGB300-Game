using UnityEditor.Build;
using UnityEngine;

public class FeetCollisions : MonoBehaviour
{

    [SerializeField] private PlayerMovement _movementScript;
    [SerializeField] ParticleSystem landEffect;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float raycastDistance;
    [SerializeField] bool doDebugRay;
    //[SerializeField] Vector3 feetPos;


    /// <summary>
    /// Used to detected when the player lands on the ground
    /// </summary>
    /// <param name="collision">The Collider that the player has triggered</param>
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Instantiate(landEffect, new Vector3(transform.position.x, transform.position.y -0.8f, transform.position.z), transform.rotation);
    //    print("Player Entered Ground");
    //    _movementScript.LandedOnGround();
    //}

    /// <summary>
    /// Used to detect when the player leaves the ground (through jump or dash or fall)
    /// </summary>
    /// <param name="collision">The Collider the player is no longer triggering</param>
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    print("Player Left Ground, Trigger by: " + collision.gameObject.name);
    //    _movementScript.LeftGround();
    //}

    private void Update()
    {
        CheckGround();
    }

    void CheckGround()
    {
        //RaycastHit hit;
        Debug.DrawRay(transform.position, Vector3.down, Color.red, raycastDistance);

        if (doDebugRay)
        {
            RaycastHit2D hit;

            hit = Physics2D.Raycast(transform.position, Vector3.down, raycastDistance);

            if (hit.collider == null)
            {
                Debug.LogError("Nothing Detected");
            }
            else
            {
                print(hit.collider.name);
            }
        }

        if (Physics2D.Raycast(transform.position, Vector3.down, raycastDistance, groundLayer))
        {
            print("Detecting Ground");
            _movementScript.LandedOnGround();
        }
        else
        {
            print("Not Detecting Ground");
            _movementScript.LeftGround();
        }
    }
}
