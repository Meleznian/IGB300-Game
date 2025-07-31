using UnityEngine;

public class ScrollingObjects : MonoBehaviour
{

    [SerializeField] private float _ScrollSpeed = 1f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * _ScrollSpeed * Time.deltaTime;

        if(transform.position.x < -20f)
        {
            Destroy(gameObject);
        }
    }
}
