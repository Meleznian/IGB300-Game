using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {

            //attack animation 

            Debug.Log("Crawler Hit!");
        }
    }
}
