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

    [Serializable]
    public class Wave
    {
        public string waveID;
        public int waveNum;
        public float spawnSpeed;
        public int enemyCap;
        public bool finished;
    }

    public List<Wave> waves = new();

    [Header("Current Wave Info")]
    public int currentWave;
    [SerializeField] private int enemyCap;
    [SerializeField] private int currentlyAlive;
    [SerializeField] private float spawnCooldown;

    [Header("Spawning Info")]
    [SerializeField] float spawnTimer;
    [SerializeField] int spawnerIndex;
    public List<EnemySpawner> spawners = new();

    [Header("Debug Info")]
    [SerializeField] int triesSinceLastSpawn;
    [SerializeField] int recountAfter;
    public bool LogEnemySpawns;
    public bool LogEnemyDamage;
    public bool LogSpawnerStates;
    public bool LogWaveUpdates;

    public GameObject[] enemyList;

    [Header("Misc")]
    [SerializeField] GameObject enemyDeathEffect;


    bool done;
    bool inWave;
    bool generate;

    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        if (!done && inWave)
        {
            TrySpawn();
        }
    }

    internal GameObject Spawn(Transform pos, GameObject prefab)
    {
        GameObject enemy;

        enemy = Instantiate(prefab, pos.position, pos.rotation);
        currentlyAlive += 1;

        BehaviourAgent b = enemy.GetComponent<BehaviourAgent>();

        if (b != null)
        {
            b.target = GameObject.Find("Player");

            if (b.assignGraph)
            {
                if (b.grounded)
                {
                    b.graphNodes = GameObject.Find("ChargerNodes").GetComponent<WaypointGraph>();
                }
                else
                {
                    b.graphNodes = GameObject.Find("NodeManager").GetComponent<WaypointGraph>();
                }
            }
        }

        spawnTimer = 0;
        return enemy;
    }

    internal void TrySpawn()
    {
        if(currentlyAlive < enemyCap)
        {
            //bool didSpawn = false;

            if (spawnTimer > spawnCooldown)
            {
                if (!spawners[spawnerIndex].currentGroup.queueFinished && spawners[spawnerIndex].currentGroup.wave == currentWave)
                {
                    if (!spawners[spawnerIndex].currentGroup.waitTillPrevDead)
                    {
                        spawners[spawnerIndex].BeginSpawn();
                        spawnTimer = 0;
                        //didSpawn = true;
                        //triesSinceLastSpawn = 0;
                    }
                    else if (spawners[spawnerIndex].prev == null)
                    {
                        spawners[spawnerIndex].BeginSpawn();
                        spawnTimer = 0;
                        //didSpawn = true;
                        //triesSinceLastSpawn = 0;
                    }
                }

                spawnerIndex++;


                if (spawnerIndex >= spawners.Count)
                {
                    spawnerIndex = 0;
                }

            }


            //if (didSpawn == false)
            //{
            //    triesSinceLastSpawn++;
            //
            //    if(triesSinceLastSpawn > recountAfter)
            //    {
            //        currentlyAlive = CountEnemies();
            //        triesSinceLastSpawn = 0;
            //    }
            //}

            if (currentlyAlive == 0)
            {
                CheckSpawners();
            }

            spawnTimer += Time.deltaTime;
        }
    }

    internal void EnemyKilled(GameObject enemy)
    {
        if (LogEnemyDamage)
        {
            print("Enemy " + enemy.name + " Killed");
        }

        currentlyAlive--;

        int b = enemy.GetComponent<BulletLodging>().Lodgedbullets;
        if (b >= 1)
        {
            GameManager.instance.SpawnBullets(b, enemy.transform.position);
        }

        Instantiate(enemyDeathEffect, enemy.transform.position, Quaternion.identity);
        Destroy(enemy);

        GameManager.instance.IncreaseHype();
        GameManager.instance.KillCount();

        if(currentlyAlive < 0)
        {
            Debug.LogError("Currently Alive Enemies is " + currentlyAlive);
            currentlyAlive = CountEnemies();
        }
    }

    internal void SetupNextWave()
    {
        //if (currentWave+1 >= waves.Count)
        //{
        //    if(LogWaveUpdates)
        //        print("All waves complete: Game Finished");
        //    done = true;
        //
        //    GameManager.instance.CompleteLevel();
        //    GameManager.instance.EndGame();
        //}

        if (!done)
        {
            if (LogWaveUpdates)
                print(waves[currentWave].waveID + " Complete");
            waves[currentWave].finished = true;

            GenerateWave();

            //Move All Spawners onto next wave Spawn Group
            foreach (EnemySpawner s in spawners)
            {
                if (s.currentGroup.wave == currentWave)
                {
                    s.GetNextGroup();
                }
            }

            currentWave++;

            if (LogWaveUpdates)
                print(currentWave);
                print("Starting " + waves[currentWave].waveID);

            //while (currentWave != waves[currentWave].waveNum)
            //{
            //    Debug.LogError("Error mismatched wave values: " + currentWave + " Does not match " + waves[currentWave].waveNum);
            //    currentWave++;
            //    Debug.LogError("Moving to " + waves[currentWave].waveID);
            //}

            enemyCap = waves[currentWave].enemyCap;
            spawnCooldown = waves[currentWave].spawnSpeed;

            PlatformManager.instance.BeginMoving();      
        }
    }

    void SetUp()
    {
        enemyCap = waves[currentWave].enemyCap;
        spawnCooldown = waves[currentWave].spawnSpeed;

        currentlyAlive = CountEnemies();
        spawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None).ToList();
        spawnerIndex = 0;

        StartWave();

        if (LogWaveUpdates)
            print("Setup Complete, Starting " + waves[currentWave].waveID);
    }

    void CheckSpawners()
    {
        int i = 0;
        int e = 0;

        foreach(EnemySpawner s in spawners)
        {
            if(LogSpawnerStates)
                print("Checking spawner " + s.name);

            if (s.currentGroup.queueFinished)
            {
                if (LogSpawnerStates)
                    print("Spawner " + s.name + " Queue complete");
                i++;
            }
            else if(s.currentGroup.wave != currentWave)
            {
                if (LogSpawnerStates)
                    print("Spawner " + s.name + " queue does not match current wave: " + s.currentGroup.wave + " vs " + currentWave);
                i++;
            }
            if (s.spawnerExhausted)
            {
                if (LogSpawnerStates)
                    print("Spawner " + s.name + " is exhausted");
                e++;
            }
        }
        if (e == spawners.Count)
        {
            if (LogSpawnerStates)
                print("All Spawners Exhausted");

            //NextLevel();
            generate = true;
            SetupNextWave();

        }
        else if (i == spawners.Count)
        {
            if (LogSpawnerStates)
                print("All Spawner Queues complete, moving to next");
            inWave = false;
            SetupNextWave();
        }
        else
        {
            if (LogSpawnerStates)
            { 
                print(i + " Spawn Queues Finished, " + spawners.Count + " Remaining");
                print(e + " Spawners Exhausted, " + spawners.Count + " Remaining");
            }
        }

        if (LogSpawnerStates)
            print("Spawner Check Finished");
    }

    void ForceSpawnAll()
    {
        foreach (EnemySpawner s in spawners)
        {
            s.BeginSpawn();         
        }
    }

    int CountEnemies()
    {
        int i = FindObjectsByType<ThisIsAnEnemy>(FindObjectsSortMode.None).Length;
        print("Recounting Enemies: " + i + " Counted");

        return i;
    }

    public void StartWave()
    {
        print("Starting Wave");
        inWave = true;
        ForceSpawnAll();
    }


    void GenerateWave()
    {
        print("No Remaining Waves. Generating Wave " + currentWave +1);

        Wave newWave = new Wave();
        newWave.enemyCap = enemyCap + 1;
        newWave.spawnSpeed = 1;
        newWave.waveID = "Wave " + (currentWave + 1);
        newWave.waveNum = currentWave + 1;

        foreach(EnemySpawner s in spawners)
        {
            s.GenerateGroup(newWave.waveNum, enemyList);
        }

        waves.Add(newWave);
        print(waves.IndexOf(newWave));
    }
}
