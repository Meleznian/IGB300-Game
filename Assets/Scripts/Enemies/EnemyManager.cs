using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        TrySpawn();

        if(currentlyAlive == 0)
        {
            CheckSpawners();
        }
    }

    [SerializeField] private int enemyCap;
    [SerializeField] private int currentlyAlive;
    [SerializeField] private float spawnCooldown;
    [SerializeField] float timer;

    public int gameStage;

    public List<EnemySpawner> spawners = new();
    [SerializeField] int spawnerIndex;

    internal void Spawn(Transform pos, GameObject prefab)
    {
        GameObject enemy;

        enemy = Instantiate(prefab, pos.position, pos.rotation);
        currentlyAlive += 1;

        if (enemy.GetComponent<ChargerAgent>() != null)
        {
            enemy.GetComponent<ChargerAgent>().target = GameObject.Find("Player");
            enemy.GetComponent<ChargerAgent>().graphNodes = GameObject.Find("ChargerNodes").GetComponent<WaypointGraph>();
        }

        timer = 0;
    }

    internal void TrySpawn()
    {
        if(currentlyAlive < enemyCap && timer > spawnCooldown)
        {
            if (!spawners[spawnerIndex].currentGroup.queueFinished)
            {
                spawners[spawnerIndex].BeginSpawn();
            }

            spawnerIndex++;

            if(spawnerIndex >= spawners.Count)
            {
                spawnerIndex = 0;   
            }
        }

        timer += Time.deltaTime;
    }

    internal void EnemyKilled()
    {
        currentlyAlive -= 1;
    }

    internal void BeginNextWave()
    {
        gameStage++;
        print("Starting Wave " + gameStage);

        foreach (EnemySpawner s in spawners)
        {
            s.GetNextGroup();
        }
        ForceSpawnAll();
    }

    void SetUp()
    {
        //currentlyAlive = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None).Length;
        spawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None).ToList();
        spawnerIndex = 0;

        foreach (EnemySpawner s in spawners)
        {
            s.GetNextGroup();
        }
        ForceSpawnAll();
    }

    void CheckSpawners()
    {
        int i = 0;
        int e = 0;

        foreach(EnemySpawner s in spawners)
        {
            if (s.currentGroup.queueFinished)
            {
                i++;
            }
            if (s.spawnerExhausted)
            {
                e++;
            }
        }
        if (e == spawners.Count)
        {
            NextLevel();
        }
        else if (i == spawners.Count)
        {
            BeginNextWave();
        }
    }

    void ForceSpawnAll()
    {
        foreach (EnemySpawner s in spawners)
        {
            s.BeginSpawn();
        }
    }

    void NextLevel()
    {
        print("All Spawners Exhausted");
    }
}
