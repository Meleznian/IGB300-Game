using UnityEngine;

public class CrawlerSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject crawlerPrefab;
    public Transform[] spawnPoints; // Assign in Inspector
    public float spawnInterval = 0.5f;
    public int maxSpawnCount = 100;

    private int _spawnedCount = 0;
    private float _nextSpawnTime;

    void Update()
    {
        if (_spawnedCount >= maxSpawnCount) return;

        if (Time.time >= _nextSpawnTime)
        {
            SpawnCrawler();
            _nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnCrawler()
    {
        // Pick a random spawn point
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(crawlerPrefab, point.position, Quaternion.identity);
        _spawnedCount++;
    }
}