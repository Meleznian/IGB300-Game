using UnityEngine;

public class FloorBullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Bullet Triggered");
        if (other.CompareTag("Player"))
        {
            //GameManager.instance.IncreaseAmmo(1);
            Destroy(transform.parent.gameObject);
        }
    }
}
