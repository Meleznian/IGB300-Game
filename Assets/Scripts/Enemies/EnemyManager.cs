using System;
using UnityEngine;
using System.Collections.Generic;


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



    [SerializeField] Vector2 spawnPosition;

    [SerializeField] int currentLevel;
    int numToSpawn;
    float spawnTimer;

    public Enemy[] enemies;

    [Serializable]
    public class Enemy
    {
        public GameObject enemyPrefab;
        public int level;
    }

    [Header("Visuals")]
    [SerializeField] GameObject enemyDeathEffect;
    [SerializeField] GameObject enemyHurtEffect;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer >= spawnSpeed)
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
                Enemy e = enemies[UnityEngine.Random.Range(0, enemies.Length)];

                if (e.level <= currentLevel)
                {
                    Instantiate(e.enemyPrefab, spawnPosition, Quaternion.identity);
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
        Destroy(enemy);

        GameManager.instance.KillCount();
    }

    internal void EnemyHurt(GameObject enemy)
    {
        Instantiate(enemyHurtEffect, enemy.transform.position, Quaternion.identity);
    }
}
