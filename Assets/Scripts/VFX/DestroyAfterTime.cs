using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float lifeTime = 0.3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
