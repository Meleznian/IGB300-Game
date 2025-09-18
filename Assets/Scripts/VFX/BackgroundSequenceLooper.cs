using System.Collections.Generic;
using UnityEngine;

public class BackgroundSequenceLooper : MonoBehaviour
{
    // ===== Camera & Parallax =====
    [Header("Camera & Parallax")]
    [SerializeField, Range(0f, 1f)] private float parallax = 0.6f;
    [SerializeField] private float preload = 2.0f;   // How many units ahead of the camera's right edge should it be pre-generated?
    [SerializeField] private bool pixelSnap = true; // Position rounded to the nearest pixel
    [SerializeField] private float overlapOffset = 0f; // Overlap segment boundaries slightly (>0 indicates overlap)
    [SerializeField] private bool useBoundsWidth = true;

    // ===== Prefabs =====
    [Header("Sequence Prefabs")]
    [SerializeField] private GameObject firstPrefab; 
    [SerializeField] private GameObject goalPrefab;  
    [SerializeField] private List<GameObject> betweenPool; 

    // ===== Sequence Length =====
    [Header("Sequence Length")]
    [Tooltip("Total number of cards (including the first 1 and the final Goal). Example: 12 → 1 + Random 10 + Goal")]
    [SerializeField, Min(2)] private int totalSegments = 12;

    // ===== Lifecycle / Memory =====
    [Header("Lifecycle / Memory")]
    [Tooltip("Number of active segments to maintain")]
    [SerializeField, Min(2)] private int keepActiveSegments = 4;

    // ===== Random =====
    [Header("Random")]
    [Tooltip("0 for non-fixed, >0 for fixed shuffle (reproducible randomness)")]
    [SerializeField] private int randomSeed = 0;

    // ===== Optional Autostart =====
    [Header("Autostart (optional)")]
    [SerializeField] private bool autoStart = false;
    [SerializeField, Min(2)] private int autoTotalSegments = 12;
    [Tooltip("When auto-starting, how much further forward to extend from the left edge of the camera")]
    [SerializeField] private float startOffsetFromCameraLeft = -2f;

    // Internal state
    private Camera cam;
    private float camHalf;        // Camera width
    private float baseX;          // Parent Standard X 
    private float unitPx;         // 1px world length
    private System.Random rng;

    private bool paused = false;  // For alternating control: Do not generate when stopped
    private bool finished = false;
    private int placedCount = 0;

    private readonly List<Segment> active = new List<Segment>();

    private class Segment
    {
        public Transform t;
        public float width;

        public float LeftEdge => t.position.x - width * 0.5f;
        public float RightEdge => t.position.x + width * 0.5f;
    }

    // API
    public bool Finished => finished;

    // The rightmost one currently. If none exists, return your own X.
    public float RightEdge => (active.Count > 0) ? active[active.Count - 1].RightEdge : transform.position.x;

    public void RestartAt(float startLeftX, int total)
    {
        for (int i = 0; i < active.Count; i++)
        {
            if (active[i]?.t) Destroy(active[i].t.gameObject);
        }
        active.Clear();

        totalSegments = Mathf.Max(2, total);
        finished = false;
        placedCount = 0;

        // Place the first one
        float firstWidth = GetWidth(firstPrefab);
        float firstCenter = startLeftX + firstWidth * 0.5f;
        var s0 = SpawnSegment(firstPrefab, firstCenter, firstWidth);
        active.Add(s0);
        placedCount = 1;
    }

    public void SetPaused(bool p) => paused = p;
    public bool IsPaused => paused;

    private void Awake()
    {
        cam = Camera.main;
        if (!cam) { Debug.LogError("[BackgroundSequenceLooper] MainCamera not found."); enabled = false; return; }
        camHalf = cam.orthographicSize * cam.aspect;
        baseX = transform.position.x;

        if (betweenPool == null) betweenPool = new List<GameObject>();
        if (betweenPool.Count == 0)
        {
            Debug.LogWarning("[BackgroundSequenceLooper] BetweenPool is empty. Only first→goal will be placed.");
        }

        rng = (randomSeed == 0) ? new System.Random() : new System.Random(randomSeed);

        // Estimated length of 1px
        var refSR = firstPrefab ? firstPrefab.GetComponentInChildren<SpriteRenderer>() : null;
        if (refSR)
            unitPx = (1f / refSR.sprite.pixelsPerUnit) * refSR.transform.lossyScale.x;
        else
            pixelSnap = false;
    }

    private void Start()
    {
        if (autoStart)
        {
            float camLeft = cam.transform.position.x - camHalf;
            float startLeft = camLeft + startOffsetFromCameraLeft; // From the left side of the screen, starting from the front
            RestartAt(startLeft, autoTotalSegments);
        }
    }

    private void LateUpdate()
    {
        // Parallax scrolling 
        float camX = cam.transform.position.x;
        var p = transform.position;
        p.x = baseX + camX * parallax;
        if (pixelSnap && unitPx > 0f) p.x = Snap(p.x);
        transform.position = p;

        if (finished || paused) return;
        if (active.Count == 0) return;

        // Generation check in world coordinates: Camera right edge + preload
        float camRight = camX + camHalf;

        // Fill the entire right edge with `while` loops
        Segment rightmost = active[active.Count - 1];
        while (rightmost.RightEdge < camRight + preload)
        {
            PlaceNext(rightmost.RightEdge);
            if (finished) break;
            rightmost = active[active.Count - 1];
        }

        while (active.Count > keepActiveSegments)
        {
            var seg = active[0];
            active.RemoveAt(0);
            if (seg.t) Destroy(seg.t.gameObject);
        }
    }

    private void PlaceNext(float currentRightEdge)
    {
        // After placing the next piece, the goal is reached when totalSegments is achieved.
        if (placedCount + 1 >= Mathf.Max(2, totalSegments))
        {
            float wGoal = GetWidth(goalPrefab);
            float nextL = currentRightEdge - overlapOffset;   
            float center = nextL + wGoal * 0.5f;
            var segGoal = SpawnSegment(goalPrefab, center, wGoal);
            active.Add(segGoal);
            placedCount++;
            finished = true;
            return;
        }

        // Until then, randomly from 2 to (n-1)
        if (betweenPool.Count == 0)
        {
            // If the pool is empty, play it safe and make it the goal.
            float wG = GetWidth(goalPrefab);
            float nxtL = currentRightEdge - overlapOffset;
            float ctr = nxtL + wG * 0.5f;
            var sG = SpawnSegment(goalPrefab, ctr, wG);
            active.Add(sG);
            placedCount++;
            finished = true;
            return;
        }

        int idx = rng.Next(0, betweenPool.Count);
        GameObject prefab = betweenPool[idx];
        float w = GetWidth(prefab);
        float left = currentRightEdge - overlapOffset;
        float cen = left + w * 0.5f;

        var seg = SpawnSegment(prefab, cen, w);
        active.Add(seg);
        placedCount++;
    }

    private Segment SpawnSegment(GameObject prefab, float centerX, float width)
    {
        var go = Instantiate(prefab, transform);
        var pos = go.transform.position;
        pos.x = (pixelSnap && unitPx > 0f) ? Snap(centerX) : centerX;
        go.transform.position = pos;
        return new Segment { t = go.transform, width = width };
    }

    private float GetWidth(GameObject prefab)
    {
        if (!prefab) return 10f;
        if (useBoundsWidth)
        {
            var sr = prefab.GetComponentInChildren<SpriteRenderer>();
            if (sr) return sr.bounds.size.x;
        }
        return Mathf.Abs(prefab.transform.localScale.x);
    }

    private float Snap(float x) => Mathf.Round(x / unitPx) * unitPx;
}
