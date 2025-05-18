using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    bool done;

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

    bool setup;
    [SerializeField] int triesSinceLastSpawn;


    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        if (!done)
        {
            TrySpawn();

            if (currentlyAlive == 0)
            {
                print("No Enemies Alive, Checking Spawner Statues");
                CheckSpawners();
            }
        }
    }

    [SerializeField] private int enemyCap;
    [SerializeField] private int currentlyAlive;
    [SerializeField] private float spawnCooldown;
    [SerializeField] float timer;

    public int currentWave;

    public List<EnemySpawner> spawners = new();
    [SerializeField] int spawnerIndex;

    internal GameObject Spawn(Transform pos, GameObject prefab)
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
        return enemy;
    }

    internal void TrySpawn()
    {
        if(currentlyAlive < enemyCap && timer > spawnCooldown)
        {
            bool didSpawn = false;

            if (!spawners[spawnerIndex].currentGroup.queueFinished && spawners[spawnerIndex].currentGroup.wave == currentWave) 
            {
                if (!spawners[spawnerIndex].currentGroup.waitTillPrevDead)
                {
                    spawners[spawnerIndex].BeginSpawn();
                    didSpawn = true;
                    triesSinceLastSpawn = 0;
                }
                else if(spawners[spawnerIndex].prev == null)
                {
                    spawners[spawnerIndex].BeginSpawn();
                    didSpawn = true;
                    triesSinceLastSpawn = 0;
                }
            }

            spawnerIndex++;

            if (spawnerIndex >= spawners.Count)
            {
                spawnerIndex = 0;
            }

            timer = 0;
            triesSinceLastSpawn++;
        }

        timer += Time.deltaTime;
    }

    internal void EnemyKilled(GameObject enemy)
    {
        print("Enemy " + enemy.name + " Killed");

        currentlyAlive -= 1;
        Destroy(enemy);

        if(currentlyAlive < 0)
        {
            currentlyAlive = CountEnemies();
        }
    }

    internal void BeginNextWave()
    {
        if (currentWave+1 >= waves.Count)
        {
            print("All waves complete: Game Finished");
            done = true;

            GameManager.instance.CompleteLevel();
            GameManager.instance.EndGame();
        }

        if (!done)
        {
            print(waves[currentWave].waveID + " Complete");
            waves[currentWave].finished = true;

            //Move All Spawners onto next wave Spawn Group
            foreach (EnemySpawner s in spawners)
            {
                if (s.currentGroup.wave == currentWave)
                {
                    s.GetNextGroup();
                }
            }

            currentWave++;
            print("Starting " + waves[currentWave].waveID);

            while (currentWave != waves[currentWave].waveNum)
            {
                Debug.LogError("Error mismatched wave values: " + currentWave + " Does not match " + waves[currentWave].waveNum);
                currentWave++;
                Debug.LogError("Moving to " + waves[currentWave].waveID);
            }

            SetupWave();

            ForceSpawnAll();
        }
    }

    void SetUp()
    {
        SetupWave();

        currentlyAlive = CountEnemies();
        spawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None).ToList();
        spawnerIndex = 0;

        ForceSpawnAll();

        print("Setup Complete, Starting " + waves[currentWave].waveID);
    }

    void CheckSpawners()
    {
        int i = 0;
        int e = 0;

        foreach(EnemySpawner s in spawners)
        {
            print("Checking spawner " + s.name);
            if (s.currentGroup.queueFinished)
            {
                print("Spawner " + s.name + " Queue complete");
                i++;
            }
            else if(s.currentGroup.wave != currentWave)
            {
                print("Spawner " + s.name + " queue does not match current wave: " + s.currentGroup.wave + " vs " + currentWave);
                i++;
            }
            if (s.spawnerExhausted)
            {
                print("Spawner " + s.name + " is exhausted");
                e++;
            }
        }
        if (e == spawners.Count)
        {
            print("All Spawners Exhausted");
            //NextLevel();
        }
        else if (i == spawners.Count)
        {
            print("All Spawner Queues complete, moving to next");
            BeginNextWave();
        }
        else
        {
            print(i + " Spawn Queues Finished, " + spawners.Count + " Remaining");
            print(e + " Spawners Exhausted, " + spawners.Count + " Remaining");
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

    int CountEnemies()
    {
        int i = FindObjectsByType<ThisIsAnEnemy>(FindObjectsSortMode.None).Length;

        return i;
    }

    void SetupWave()
    {
        enemyCap = waves[currentWave].enemyCap;
        spawnCooldown = waves[currentWave].spawnSpeed;
    }
}
