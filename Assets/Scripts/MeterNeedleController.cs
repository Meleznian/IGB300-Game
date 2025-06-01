using UnityEngine;

public class MeterNeedleController : MonoBehaviour
{
    [Header("Needle Settings")]
    public RectTransform needle;
    public float minAngle = -90f; 
    public float maxAngle = 90f; 
    public float speed = 1f;    

    [Header("Auto Ping-Pong")]
    public bool pingPong = true;

    private float t = 0f;
    private bool forward = true;

    void Update()
    {
        // The needle will still continue animating even when the game is paused
        float delta = Time.unscaledDeltaTime; 

        if (pingPong)
        {
            t += delta * speed * (forward ? 1 : -1);
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
            t += delta * speed;
            t = Mathf.Clamp01(t);
        }

        float angle = Mathf.Lerp(minAngle, maxAngle, t);
        needle.localRotation = Quaternion.Euler(0, 0, angle);
    }

}
