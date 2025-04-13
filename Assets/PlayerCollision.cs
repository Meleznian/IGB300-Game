using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private float _damageCooldown = 1f; // Time between damage ticks
    private float _lastDamageTime;

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit Player");
        }
    }*/


    /*private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (Time.time > _lastDamageTime + _damageCooldown)
            {
                other.GetComponent<PlayerHealth>().TakeDamage(33);
                _lastDamageTime = Time.time;
                Debug.Log("Continuous Damage!");
            }
        }
    }*/

    void OnGUI()
    {
        // Always use try-finally for clipping
        GUI.BeginClip(new Rect(0, 0, 100, 100));
        try
        {
            GUI.Label(new Rect(0, 0, 100, 20), "Clipped Content");
        }
        finally
        {
            GUI.EndClip(); // Guaranteed to execute
        }
    }
}
