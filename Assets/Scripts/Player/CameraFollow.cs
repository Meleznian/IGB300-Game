using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    [Header("Stage Bounds")]
    public Transform stageBoundsTopLeft;
    public Transform stageBoundsBottomRight;

    private float camHalfHeight;
    private float camHalfWidth;

    void Start()
    {
        Camera cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;
        target = GameObject.Find("Player").transform;
    }

    void LateUpdate()
    {
        if (target == null || stageBoundsTopLeft == null || stageBoundsBottomRight == null) return;

        Vector3 desiredPosition = target.position + offset;

        float minX = stageBoundsTopLeft.position.x + camHalfWidth;
        float maxX = stageBoundsBottomRight.position.x - camHalfWidth;
        float minY = stageBoundsBottomRight.position.y + camHalfHeight;
        float maxY = stageBoundsTopLeft.position.y - camHalfHeight;

        float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(desiredPosition.y, minY, maxY);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, new Vector3(clampedX, clampedY, desiredPosition.z), smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}

