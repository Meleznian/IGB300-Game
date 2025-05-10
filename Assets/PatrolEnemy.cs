using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{

    public float moveSpeed = 2f;
    public Transform checkPoint;
    public float distance = 1f;
    public LayerMask layerMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

        //Physics2D.Raycast()
    }
}
