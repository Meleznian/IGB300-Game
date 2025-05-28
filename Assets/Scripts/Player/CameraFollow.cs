using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);

    [Header("Clamp Settings")]
    [SerializeField] private Vector2 minClamp; // 左下の制限座標
    [SerializeField] private Vector2 maxClamp; // 右上の制限座標

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // カメラの位置を制限
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minClamp.x, maxClamp.x);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minClamp.y, maxClamp.y);

        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothed;
    }
}
