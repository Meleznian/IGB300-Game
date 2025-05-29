using UnityEngine;

public class AimCursorController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float maxDistance = 5f;
    [SerializeField] float inputSwitchDelay = 0.5f;

    Vector2 gamepadAimPos;
    bool usingMouse = true;
    float lastMouseTime = -10f;
    float lastStickTime = -10f;

    void Start()
    {
        gamepadAimPos = player.position;
    }

    void Update()
    {
        DetectInputSource();

        if (usingMouse)
        {
            Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 offset = mouse - (Vector2)player.position;

            if (offset.magnitude > maxDistance)
            {
                offset = offset.normalized * maxDistance;
            }

            transform.position = (Vector2)player.position + offset;
        }
        else
        {
            float x = Input.GetAxis("RightStickX");
            float y = -Input.GetAxis("RightStickY");
            Vector2 stickInput = new Vector2(x, y);

            if (stickInput.magnitude > 0.1f)
            {
                gamepadAimPos += stickInput * moveSpeed * Time.deltaTime;

                Vector2 offset = gamepadAimPos - (Vector2)player.position;
                if (offset.magnitude > maxDistance)
                {
                    offset = offset.normalized * maxDistance;
                    gamepadAimPos = (Vector2)player.position + offset;
                }

                transform.position = gamepadAimPos;
            }
        }

        SetVisible(true);
    }

    void DetectInputSource()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            lastMouseTime = Time.time;

        float x = Input.GetAxis("RightStickX");
        float y = Input.GetAxis("RightStickY");
        if (Mathf.Abs(x) > 0.1f || Mathf.Abs(y) > 0.1f)
            lastStickTime = Time.time;

        if (lastMouseTime > lastStickTime + inputSwitchDelay)
            usingMouse = true;
        else if (lastStickTime > lastMouseTime + inputSwitchDelay)
            usingMouse = false;
    }

    void SetVisible(bool visible)
    {
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer)
            renderer.enabled = visible;
    }
}
