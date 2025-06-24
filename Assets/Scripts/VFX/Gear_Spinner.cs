using UnityEngine;

public class Gear_Spinner : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 0, 100);

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.unscaledDeltaTime);
    }
}
