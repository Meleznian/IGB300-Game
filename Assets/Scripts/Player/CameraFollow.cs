using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;      // Players and other tracking targets
    [SerializeField] Vector3 offset;        // In-screen positioning
    [SerializeField] float smoothSpeed = 5f; // Smoothness of camera tracking

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
