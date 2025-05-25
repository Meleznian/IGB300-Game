using UnityEngine;

public class MeterNeedleController : MonoBehaviour
{
    [Header("Needle Settings")]
    public RectTransform needle;  // Assign your needle object here
    public float minAngle = -90f; // Leftmost angle
    public float maxAngle = 90f;  // Rightmost angle
    public float speed = 1f;      // Speed of movement

    [Header("Auto Ping-Pong")]
    public bool pingPong = true;  // Should the needle move back and forth?

    private float t = 0f;
    private bool forward = true;

    void Update()
    {
        if (pingPong)
        {
            t += Time.deltaTime * speed * (forward ? 1 : -1);
            if (t > 1f)
            {
                t = 1f;
                forward = false;
            }
            else if (t < 0f)
            {
                t = 0f;
                forward = true;
            }
        }
        else
        {
            t += Time.deltaTime * speed;
            t = Mathf.Clamp01(t);
        }

        float angle = Mathf.Lerp(minAngle, maxAngle, t);
        needle.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
