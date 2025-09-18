using UnityEngine;

public class BackgroundSequenceAlternator : MonoBehaviour
{
    [Header("Two Sequences")]
    [SerializeField] private BackgroundSequenceLooper oldSeq;
    [SerializeField] private BackgroundSequenceLooper newSeq;

    [Header("Shared Settings")]
    [SerializeField, Min(2)] private int totalSegments = 12;
    [SerializeField] private float overlapOffset = 0f;
    [SerializeField] private float initialPreload = 2f;   // The margin extending further forward than the left edge of the camera

    [Header("Start Options")]
    [SerializeField] private bool startWithOld = true;
    [SerializeField] private bool autoStart = true;

    private Camera cam;
    private enum Which { Old, New }
    private Which current;
    private bool initialized;

    void Start()
    {
        if (!oldSeq || !newSeq)
        {
            Debug.LogError("[BackgroundSequenceAlternator] Assign both oldSeq and newSeq.");
            enabled = false;
            return;
        }
        cam = Camera.main;
        if (!cam) { Debug.LogError("[BackgroundSequenceAlternator] Main Camera not found."); enabled = false; return; }

        oldSeq.SetPaused(true);
        newSeq.SetPaused(true);

        if (!autoStart) return;

        current = startWithOld ? Which.Old : Which.New;

        // Starting from the left edge of the camera and moving toward the foreground, prevent reflections from the blue background.
        float camHalf = cam.orthographicSize * cam.aspect;
        float camLeft = cam.transform.position.x - camHalf;
        float startLeft = camLeft - Mathf.Abs(initialPreload);

        if (current == Which.Old)
        {
            oldSeq.RestartAt(startLeft, totalSegments);
            oldSeq.SetPaused(false);   // Only one side is moved.
        }
        else
        {
            newSeq.RestartAt(startLeft, totalSegments);
            newSeq.SetPaused(false);
        }
        initialized = true;
    }

    void LateUpdate()
    {
        if (!initialized) return;

        if (current == Which.Old)
        {
            if (oldSeq.Finished)
            {
                // OLD Complete → Start NEW from the right edge
                float nextLeft = oldSeq.RightEdge - overlapOffset;

                // Switch: Stop the old and start the new
                oldSeq.SetPaused(true);
                newSeq.RestartAt(nextLeft, totalSegments);
                newSeq.SetPaused(false);

                current = Which.New;
            }
        }
        else 
        {
            if (newSeq.Finished)
            {
                float nextLeft = newSeq.RightEdge - overlapOffset;

                newSeq.SetPaused(true);
                oldSeq.RestartAt(nextLeft, totalSegments);
                oldSeq.SetPaused(false);

                current = Which.Old;
            }
        }
    }
}
