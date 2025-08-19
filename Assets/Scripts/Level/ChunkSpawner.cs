using UnityEngine;
using UnityEngine.Android;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _chunkPrefabs;
    [SerializeField] private float _chunkWidth = 20f;
    [SerializeField] private int _chunksAhead = 3;

    private float _spawnX = 20f;

    void Start()
    {
        for (int i = 0; i < _chunksAhead; i++)
        {
            SpawnChunk();
        }
    }
    void Update()
    {
        if (Camera.main.transform.position.x + (_chunksAhead * _chunkWidth) > _spawnX)
        {
            SpawnChunk();
        }
      
    }

    void SpawnChunk()
    {
        int randomIndex = Random.Range(0, ChunkPoolManager.Instance.PoolCount);
        GameObject chunk = ChunkPoolManager.Instance.GetChunkFromPool(randomIndex, new Vector3(_spawnX, 0f, 0f));
        
        _spawnX += _chunkWidth;
    }
}
