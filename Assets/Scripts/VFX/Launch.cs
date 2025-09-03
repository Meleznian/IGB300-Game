using UnityEngine;

public class Launch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool launched;
    private void Start()
    {
        Camera.main.GetComponent<CameraFollow>().target = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!launched)
        { 
            GetComponent<Rigidbody2D>().AddForce((Vector3.up + Vector3.right) * 4, ForceMode2D.Impulse);
            GetComponent<Rigidbody2D>().AddTorque(30);
            launched = true;
        }
    }
}
