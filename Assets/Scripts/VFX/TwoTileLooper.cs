using UnityEngine;

public class TwoTileLooper : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] float parallax = 0.6f;

    Transform cam;
    Transform a, b;      // Two tiles
    float tileWidth;     // 1 sheet width (world)
    float baseX;         // Parental Standards X
    float maxCamX;       // Update only in the right direction

    void Awake()
    {
        cam = Camera.main.transform;

        // Get child (order of A and B is either acceptable)
        a = transform.GetChild(0);
        b = transform.GetChild(1);

        // Width is obtained from the child SpriteRenderer (assuming both are the same)
        var sr = a.GetComponent<SpriteRenderer>();
        tileWidth = sr.bounds.size.x;

        // Assuming two pieces are connected side-by-side (if necessary, place b to the right of a here)
        baseX = transform.position.x;
        maxCamX = cam.position.x;
    }

    void LateUpdate()
    {
        // Parallax-based parent display position (Y/Z fixed)
        float camX = cam.position.x;
        if (camX > maxCamX) maxCamX = camX;              // Do not return to the left
        float dist = maxCamX * parallax;

        Vector3 pos = transform.position;
        pos.x = baseX + dist;
        transform.position = pos;

        // Rotate the ÅgleftÅh of the two panels further to the right of the right edge (always covering the screen with both panels).
        // Since the parent moves, the tile that has shifted sufficiently to the left from the camera's perspective is judged to have rotated.
        var left = a.position.x < b.position.x ? a : b;
        var right = a.position.x < b.position.x ? b : a;

        // If it extends more than one screen's width to the left, move it further to the right of the right edge.
        if (camX - left.position.x > tileWidth)
        {
            left.position = new Vector3(right.position.x + tileWidth, left.position.y, left.position.z);
            // Swap
            var tmp = a; a = b; b = tmp;
        }
    }
}
