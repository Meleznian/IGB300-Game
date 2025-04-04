using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;

    void Update()
    {
        // right mouse click
        if (Input.GetMouseButtonDown(1))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dir = mousePos - firePoint.position;

            FireBullet(dir);
        }

        // R2 + right stick direction to fire
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            float x = Input.GetAxis("RightStickX");
            float y = -Input.GetAxis("RightStickY");

            Vector2 stickInput = new Vector2(x, y);

            if (stickInput.magnitude > 0.3f) // Determine if the stick is down.
            {
                FireBullet(stickInput.normalized);
            }
        }

        Debug.Log($"RightStick: X={Input.GetAxis("RightStickX")} Y={Input.GetAxis("RightStickY")}");

    }

    void FireBullet(Vector2 dir)
    {
        if (!bulletPrefab || !firePoint) return;

        var bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(dir);
    }
}
