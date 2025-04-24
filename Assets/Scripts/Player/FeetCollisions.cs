using UnityEngine;

public class FeetCollisions : MonoBehaviour
{

    [SerializeField] private PlayerMovement movementScript;


    /// <summary>
    /// Used to detected when the player lands on the ground
    /// </summary>
    /// <param name="collision">The Collider that the player has triggered</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        movementScript.LandedOnGround();
    }

    /// <summary>
    /// Used to detect when the player leaves the ground (through jump or dash or fall)
    /// </summary>
    /// <param name="collision">The Collider the player is no longer triggering</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        movementScript.LeftGround();
    }
}
