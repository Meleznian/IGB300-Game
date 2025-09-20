using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static EnemyManager;
using static UnityEngine.Rendering.DebugUI;


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

    [Header("Spawn Variables")]
    [SerializeField] float spawnSpeed;
    [SerializeField] int currentlyAlive;
    [SerializeField] int enemyCap;
    [SerializeField] int minSpawnGroup;
    [SerializeField] int maxSpawnGroup;
    [SerializeField] float spawnVariationX;
    [SerializeField] float spawnVariationY;

    [Header("Scaling Variables")]
    [SerializeField] float speedIncrease = 0.5f;
    [SerializeField] int capIncrease = 1;
    [SerializeField] int groupIncrease = 1;
    [Header("Scaling Clamps")]
    [SerializeField] float speedClamp = 1;
    [SerializeField] int capClamp = 20;
    [SerializeField] int groupClamp = 5;



    [SerializeField] Vector2 spawnPosition;

    [SerializeField] int currentLevel;
    int numToSpawn;
    float spawnTimer;
    float totalWeight;

    public Enemy[] enemies;
    public List<GameObject> livingEnemiesList = new List<GameObject>();

    [Serializable]
    public class Enemy
    {
        public GameObject enemyPrefab;
        public int level;
        public float spawnWeight;
        public string spawnPercentage;
    }

    [Header("Visuals")]
    [SerializeField] GameObject enemyDeathEffect;
    [SerializeField] GameObject enemyHurtEffect;
    [SerializeField] GameObject bulletWallEffect;
    [SerializeField] GameObject bulletHitEffect;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Enemy enemy in enemies)
        {
            totalWeight += enemy.spawnWeight;
        }

        foreach (Enemy enemy in enemies)
        {
            GetSpawnPercentage(enemy);
        }
    }

    float GetSpawnPercentage(Enemy e)
    {
        float s = e.spawnWeight / totalWeight;
        s *= 100;
        e.spawnPercentage = s + "%";
        return s;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer >= spawnSpeed && spawnSpeed != 0)
        {
            numToSpawn = UnityEngine.Random.Range(minSpawnGroup, maxSpawnGroup + 1);
            SpawnEnemies();
            spawnTimer = 0;
        }

        spawnTimer += Time.deltaTime;
    }

    private void SpawnEnemies()
    {
        int i = 0;

        while (i <= numToSpawn)
        {
            if (currentlyAlive < enemyCap)
            {
                Enemy e = ChooseEnemy();

                if (e.level <= ScoreManager.instance.currentScore)
                {
                    float x = UnityEngine.Random.Range(-spawnVariationX, spawnVariationX);
                    float y = UnityEngine.Random.Range(-spawnVariationY, spawnVariationY);

                    Vector2 spawnPos = new Vector2(transform.position.x + x, transform.position.y + y);

                    livingEnemiesList.Add(Instantiate(e.enemyPrefab, spawnPos, Quaternion.identity));
                    currentlyAlive += 1;
                    i++;
                }
                else
                {
                    continue;
                }
            }
            else
            {
                break;
            }
        }
    }

    internal void EnemyKilled(GameObject enemy)
    {
        currentlyAlive--;

        Instantiate(enemyDeathEffect, enemy.transform.position, Quaternion.identity);
        livingEnemiesList.Remove(enemy);
        Destroy(enemy);

        
    }

    internal void EnemyHurt(Vector3 pos)
    {
        print("Enemy Hurt");
        Instantiate(enemyHurtEffect, pos, Quaternion.identity);
    }

    internal void BulletWall(Vector3 pos)
    {
        //print("Enemy Hurt");
        Instantiate(bulletWallEffect, pos, Quaternion.identity);
    }

    internal void BulletHit(Vector3 pos)
    {
        //print("Enemy Hurt");
        Instantiate(bulletHitEffect, pos, Quaternion.identity);
    }

    internal void IncreaseDifficulty()
    {
        spawnSpeed -= speedIncrease;
        spawnSpeed = Mathf.Clamp(spawnSpeed, speedClamp, 5);

        enemyCap += capIncrease;
        maxSpawnGroup += groupIncrease;

        maxSpawnGroup = Mathf.Clamp(maxSpawnGroup, 1, groupClamp);
    }

    Enemy ChooseEnemy()
    {
        float roll = UnityEngine.Random.Range(0, totalWeight);
        int i = 0;

        foreach (Enemy e in enemies)
        {
            roll -= enemies[i].spawnWeight;

            if (roll < 0)
            {
                return enemies[i];
            }
            i++;
        }

        return enemies[0];
    }
}
