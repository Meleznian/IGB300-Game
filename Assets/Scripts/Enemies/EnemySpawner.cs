using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] private float cooldown;
    [SerializeField] private float cooldownTimer;

    [Serializable]
    public class SpawnGroup 
    {
        [SerializeField] List<GameObject> queue = new();
        [SerializeField] internal int gameStage;
        [SerializeField] int index;

        public GameObject GetEnemy()
        {
            if(index > queue.Count)
            {
                return null;
            }

            var enemy = queue[index];
            index++;
            return enemy;
        }
    }

    [SerializeField] List<SpawnGroup> spawnGroups = new();

    private SpawnGroup currentGroup;
    private int groupIndex;
    [SerializeField] bool paused;

    private void Start()
    {
        currentGroup = spawnGroups[0];
        groupIndex = 0;
    }
    private void Update()
    {
        if (!paused)
        {
            cooldownTimer += Time.deltaTime;
        }

        if (cooldownTimer >= cooldown)
        {
            cooldownTimer = 0;
            var nextEnemy = currentGroup.GetEnemy();

            if (nextEnemy != null)
            {
                EnemyManager.instance.Spawn(transform, nextEnemy);
            }
            else
            {
                GetNextGroup();
            }
        }      
    }


    void GetNextGroup()
    {
        groupIndex++;
        currentGroup = spawnGroups[groupIndex];

        if(currentGroup.gameStage < EnemyManager.instance.gameStage)
        {
            paused = true;
        }
    }
}
