using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);

    [Header("Clamp Settings")]
    [SerializeField] private Vector2 minClamp; // �����̐������W
    [SerializeField] private Vector2 maxClamp; // �E��̐������W

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // �J�����̈ʒu�𐧌�
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minClamp.x, maxClamp.x);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minClamp.y, maxClamp.y);

        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothed;
    }
}
