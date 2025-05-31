using UnityEngine;

public class CorrectRotation : MonoBehaviour
{
    [SerializeField] Vector3 intendedRotation;

    // Update is called once per frame
    void Update()
    {
        if(transform.rotation != Quaternion.Euler(intendedRotation))
        {
            transform.rotation = Quaternion.Euler(intendedRotation);
        }
    }
}
