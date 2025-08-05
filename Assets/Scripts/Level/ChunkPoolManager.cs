using System.Collections.Generic;
using UnityEngine;

public class ChunkPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class ChunkPool
    {
        public GameObject chunkPrefab;
        public int poolSize = 5;
        public Queue<GameObject> poolQueue = new Queue<GameObject>();
    }
    
    [SerializeField] private ChunkPool[] _chunksPool;
    
    public static ChunkPoolManager Instance { get; private set; }
    
    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        foreach (var pool in _chunksPool)
        {
            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.chunkPrefab);
                obj.SetActive(false);
                pool.poolQueue.Enqueue(obj);
            }
        }
    }

    public GameObject GetChunkFromPool(int poolIndex, Vector3 position)
    {
        ChunkPool pool = _chunksPool[poolIndex];

        if (pool.poolQueue.Count == 0)
        {
            Debug.LogWarning("Pool exhausted, adding to the pool");
            GameObject extra = Instantiate(pool.chunkPrefab);
            extra.SetActive(false);
            pool.poolQueue.Enqueue(extra);
        }
        
        GameObject chunk = pool.poolQueue.Dequeue();
        chunk.transform.position = position;
        chunk.SetActive(true);
        return chunk;
    }

    public void ReturnChunkToPool(GameObject chunk, int poolIndex)
    {
        chunk.SetActive(false);
        _chunksPool[poolIndex].poolQueue.Enqueue(chunk);
    }
    
    public int PoolCount => _chunksPool.Length;
}
