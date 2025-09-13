using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [SerializeField] private int _poolIndex;
    [SerializeField] private float _despawnX = -30f;
    //[SerializeField] private GameObject _killWall;
    //[SerializeField] private GameObject _thisChunk;

    private void Update()
    {
        if (transform.position.x < _despawnX)
        {
            ChunkPoolManager.Instance.ReturnChunkToPool(gameObject, _poolIndex);
        }
    }
}
