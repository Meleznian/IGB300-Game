using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SeamlessLooper : MonoBehaviour
{
    [Header("Parallax & Timing")]
    [Range(0f, 1f)]
    [SerializeField] float parallax = 0.6f;         // The closer it is, the larger it appears; the farther it is, the smaller it appears.
    [SerializeField] float preload = 1.0f;          // How many units ahead of the camera's right edge should they be positioned?
    [SerializeField] bool pixelSnap = true;         // Align fractions to 1px increments to prevent flickering

    [Header("Width Detection")]
    [SerializeField] bool useBoundsWidth = true;     // true: bounds.size.x
                                                     // false: sprite.rect/PPU*lossyScale

    Transform cam;
    float camHalf;                   // Half the width of the camera
    readonly List<Transform> tiles = new List<Transform>();
    float tileWidth;                 // One precise world width
    float unit;                      // 1px world length (for snapping)
    float baseX;                     // Parental Standards X

    void OnEnable()
    {
        Setup();
    }

    void Setup()
    {
        var mainCam = Camera.main;
        if (!mainCam) { Debug.LogWarning("MainCamera not found."); enabled = false; return; }

        cam = mainCam.transform;
        camHalf = mainCam.orthographicSize * mainCam.aspect;

        // Collect child SpriteRenderers (minimum of two)
        tiles.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            var t = transform.GetChild(i);
            if (t.GetComponent<SpriteRenderer>()) tiles.Add(t);
        }
        if (tiles.Count < 2)
        {
            Debug.LogError($"[{name}] Need at least 2 child tiles with SpriteRenderer.");
            enabled = false; return;
        }

        // Width acquisition
        var sr0 = tiles[0].GetComponent<SpriteRenderer>();
        if (useBoundsWidth)
            tileWidth = sr0.bounds.size.x; // Scale/Trim Reflection
        else
            tileWidth = (sr0.sprite.rect.width / sr0.sprite.pixelsPerUnit) * tiles[0].lossyScale.x;

        unit = (1f / sr0.sprite.pixelsPerUnit) * tiles[0].lossyScale.x;

        // Rearrange from left to right & Connect seamlessly
        tiles.Sort((a, b) => a.position.x.CompareTo(b.position.x));
        float leftX = tiles[0].position.x;
        for (int i = 0; i < tiles.Count; i++)
            SetTileX(tiles[i], leftX + i * tileWidth);

        baseX = transform.position.x;

        // Automatically suggest a minimum preload amount (if insufficient, a switch will appear on-screen)
        float suggested = Mathf.Max(tileWidth * 0.5f, camHalf * (1f - parallax));
        if (preload < suggested) preload = suggested;
    }

    void LateUpdate()
    {
        if (!cam) { Setup(); if (!cam) return; }

        // Parent's parallax movement (assumed to be only to the right)
        float camX = cam.position.x;
        Vector3 p = transform.position;
        p.x = baseX + camX * parallax;
        if (pixelSnap) p.x = Snap(p.x);
        transform.position = p;

        float camRight = camX + camHalf;

        // Maintain order (left→right)
        tiles.Sort((a, b) => a.position.x.CompareTo(b.position.x));
        var left = tiles[0];
        var right = tiles[tiles.Count - 1];

        // If the camera is about to reach the right edge, make the left-most tile appear right next to the right-most tile's right edge (i.e., connect them). 
        // This is done off-screen (after preloading).
        float rightEdge = right.position.x + tileWidth * 0.5f;

        // Use a while loop to run the necessary number of times to handle high-speed movement.
        while (rightEdge < camRight + preload)
        {
            float newLeftEdge = right.position.x + tileWidth * 0.5f; // The far right
            float newCenter = newLeftEdge + tileWidth * 0.5f;      // From the left edge → toward the center

            SetTileX(left, newCenter);

            // Array rotation
            tiles.RemoveAt(0);
            tiles.Add(left);

            // Recalculation
            left = tiles[0];
            right = tiles[tiles.Count - 1];
            rightEdge = right.position.x + tileWidth * 0.5f;
        }
    }

    void SetTileX(Transform t, float centerX)
    {
        var pos = t.position;
        pos.x = pixelSnap ? Snap(centerX) : centerX;
        t.position = pos;
    }

    float Snap(float x) => pixelSnap ? Mathf.Round(x / unit) * unit : x;

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!Camera.main) return;
        float h = Camera.main.orthographicSize * Camera.main.aspect;
        float x = Camera.main.transform.position.x + h + preload;
        Gizmos.color = new Color(0, 1, 1, 0.25f);
        Gizmos.DrawLine(new Vector3(x, -1000, 0), new Vector3(x, 1000, 0)); // 入替ライン（画面外）
    }
#endif

#if UNITY_EDITOR
[ContextMenu("Print Metrics")]
void PrintMetrics()
{
    var camComp = Camera.main;
    float camHalfCalc = camComp.orthographicSize * camComp.aspect;

    // 子0の SpriteRenderer から幅を出す（useBoundsWidth と同じロジック）
    var t0 = transform.GetChild(0);
    var sr0 = t0.GetComponent<SpriteRenderer>();
    float tileWidthCalc = useBoundsWidth
        ? sr0.bounds.size.x
        : (sr0.sprite.rect.width / sr0.sprite.pixelsPerUnit) * t0.lossyScale.x;

    float suggested = Mathf.Max(tileWidthCalc * 0.5f, camHalfCalc * (1f - parallax));

    Debug.Log(
        $"[{name}] parallax={parallax:F2}\n" +
        $"- tileWidth = {tileWidthCalc:F3}\n" +
        $"- camHalf   = {camHalfCalc:F3}\n" +
        $"- preload (suggested min) = {suggested:F3}\n" +
        $"- current preload = {preload:F3}\n" +
        $"(Tip: set preload to ~{(suggested + 0.5f):F3} for safety)"
    );
}
#endif

}
