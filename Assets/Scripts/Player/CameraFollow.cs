using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;
    Vector3 velocity = Vector3.zero;
    public bool tutorial;

    [Header("Stage Bounds (Only Y)")]
    public Transform stageBoundsTopLeft;
    public Transform stageBoundsBottomRight;

    float camHalfHeight, camHalfWidth;
    float maxCamX; // Maximum X reach to the right

    void Start()
    {
        var cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;

        if (!target) target = GameObject.Find("Player").transform;

        // Initialize maximum X at initial position reference
        var startDesired = (target ? target.position : transform.position) + offset;
        maxCamX = Mathf.Max(transform.position.x, startDesired.x);
    }

    void FixedUpdate()
    {
        if (!target) return;

        Vector3 desired = target.position + offset;

        // It is acceptable to update to the right (update to a maximum of X).
        if (desired.x > maxCamX) maxCamX = desired.x;

        // X is fixed at the maximum value (does not return to the left).
        float lockedX = maxCamX;

        // Y is the same as before Clamp
        float minY = stageBoundsBottomRight ? stageBoundsBottomRight.position.y + camHalfHeight : -Mathf.Infinity;
        float maxY = stageBoundsTopLeft ? stageBoundsTopLeft.position.y - camHalfHeight : Mathf.Infinity;
        float clampedY = Mathf.Clamp(desired.y, minY, maxY);

        Vector3 targetPos = new Vector3(lockedX + 3, transform.position.y, desired.z);
        if (!tutorial)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothSpeed);
        }
        else
        {

        }
        //transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);

    }
}