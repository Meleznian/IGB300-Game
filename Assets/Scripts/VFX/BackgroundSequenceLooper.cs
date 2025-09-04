using System.Collections.Generic;
using UnityEngine;

public class BackgroundSequenceLooper : MonoBehaviour
{
    // ===== Camera & Parallax =====
    [Header("Camera & Parallax")]
    [Range(0f, 1f)][SerializeField] float parallax = 0.6f;
    [SerializeField] float preload = 2.0f;          // Distance ahead of the camera's right edge
    [SerializeField] bool pixelSnap = true;        // Rounding positions to the nearest pixel
    [SerializeField] float overlapOffset = 0f;      // Overlap segments slightly (>0 indicates overlap)
    [SerializeField] bool useBoundsWidth = true;   // Method for obtaining width (bounds recommended)

    // ===== Prefabs =====
    [Header("Sequence Prefabs")]
    [SerializeField] GameObject firstPrefab;        // Always at the top
    [SerializeField] GameObject goalPrefab;         // Always the end (goal)
    [SerializeField] List<GameObject> betweenPool;  // Insert 2 to 6
    [SerializeField] GameObject pipe;               // To hide seams

    // ===== Sequence Length =====
    [Header("Sequence Length")]
    [Tooltip("Total number of cards")]
    [SerializeField] int totalSegments = 12;
    [Tooltip("Stop generation when 7 (Goal) is reached.")]
    [SerializeField] bool stopAfterGoal = true;

    // ===== Lifecycle / Memory =====
    [Header("Lifecycle / Memory")]
    [Tooltip("Number of active segments to maintain")]
    [SerializeField] int keepActiveSegments = 4;

    // ===== Random =====
    [Header("Random")]
    [Tooltip("0 for non-fixed, >0 for fixed shuffle")]
    [SerializeField] int randomSeed = 0;

    Transform cam;
    float camHalf;
    float baseX;
    float unitPx;                           // 1px world length
    readonly List<Segment> active = new List<Segment>();
    int placedCount = 0;                   // Number of generated items
    bool finished = false;               // If roll a 7 and finish, it's true.
    System.Random rng;

    class Segment
    {
        public Transform t;
        public float width;
        public float leftEdge => t.position.x - width * 0.5f;
        public float rightEdge => t.position.x + width * 0.5f;
    }

    void Awake()
    {
        // Camera
        var mainCam = Camera.main;
        if (!mainCam) { enabled = false; Debug.LogError("[BackgroundSequenceLooper] MainCamera not found."); return; }
        cam = mainCam.transform;
        camHalf = mainCam.orthographicSize * mainCam.aspect;
        baseX = transform.position.x;

        // Pool Confirmation
        if (betweenPool == null || betweenPool.Count == 0)
        {
            Debug.LogError("[BackgroundSequenceLooper] BetweenPool is empty. Assign BG_2..BG_6.");
            enabled = false; return;
        }

        // RNG
        rng = (randomSeed == 0) ? new System.Random() : new System.Random(randomSeed);

        // Estimated 1px Length
        var refSR = firstPrefab ? firstPrefab.GetComponentInChildren<SpriteRenderer>() : null;
        if (refSR)
            unitPx = (1f / refSR.sprite.pixelsPerUnit) * refSR.transform.lossyScale.x;
        else
            pixelSnap = false;

        // Top
        var s0 = SpawnSegment(firstPrefab, transform.position.x);
        active.Add(s0);
        placedCount = 1;
    }

    void LateUpdate()
    {
        if (finished) return;

        // Parent's Parallax Movement
        float camX = cam.position.x;
        var p = transform.position;
        p.x = baseX + camX * parallax;
        if (pixelSnap && unitPx > 0f) p.x = Snap(p.x);
        transform.position = p;

        float camRight = camX + camHalf;

        // Once sufficiently close to the right edge, add the next segment off-screen.
        var rightmost = active[active.Count - 1];
        while (!finished && rightmost.rightEdge < camRight + preload)
        {
            var next = ChooseNextPrefab();                 // Determine the next prefab
            float nextLeftEdge = rightmost.rightEdge - overlapOffset;
            float nextCenter = nextLeftEdge + GetWidth(next) * 0.5f;

            var seg = SpawnSegment(next, nextCenter);
            active.Add(seg);
            rightmost = seg;
            placedCount++;

            // Once the goal is set, it's over.
            if (stopAfterGoal && ReferenceEquals(next, goalPrefab))
            {
                finished = true;
                break;
            }

            // Sorting through old things
            if (active.Count > keepActiveSegments)
            {
                Destroy(active[0].t.gameObject);
                active.RemoveAt(0);
            }
        }
    }

    // Next prefab to place
    GameObject ChooseNextPrefab()
    {
        // After placing the next segment, return the goal when the totalSegments reaches the target value.
        int minTotal = Mathf.Max(2, totalSegments); // Minimum 2
        if (placedCount + 1 >= minTotal)
            return goalPrefab;

        // Until then, randomly selected from 2 to 6
        int idx = rng.Next(0, betweenPool.Count);
        return betweenPool[idx];
    }

    Segment SpawnSegment(GameObject prefab, float centerX)
    {
        var go = Instantiate(prefab, transform);
        var pos = go.transform.position;
        pos.x = (pixelSnap && unitPx > 0f) ? Snap(centerX) : centerX;
        go.transform.position = pos;
        var newPipe = Instantiate(pipe, go.transform);
        newPipe.transform.localPosition = new Vector2(9.5f, 0);


        return new Segment { t = go.transform, width = GetWidth(go) };
    }

    float GetWidth(GameObject go)
    {
        var sr = go.GetComponentInChildren<SpriteRenderer>();
        if (!sr) return 10f;
        return useBoundsWidth
            ? sr.bounds.size.x
            : (sr.sprite.rect.width / sr.sprite.pixelsPerUnit) * sr.transform.lossyScale.x;
    }

    float Snap(float x) => Mathf.Round(x / unitPx) * unitPx;

    public void ArmGoalNowAndFinish()
    {
        if (!finished)
            totalSegments = placedCount + 1; // Next stop is the finish line
    }
}
