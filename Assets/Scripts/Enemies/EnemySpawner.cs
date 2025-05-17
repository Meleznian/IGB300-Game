using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] GameObject nextEnemy;
    [SerializeField] internal bool spawnerExhausted;

    [Serializable]
    public class SpawnGroup 
    {
        [SerializeField] List<GameObject> queue = new();
        [SerializeField] internal int gameStage;
        [SerializeField] int index;
        [SerializeField] internal bool queueFinished;

        public GameObject GetEnemy()
        {
            if(index >= queue.Count)
            {
                queueFinished = true;
                return null;
            }

            var enemy = queue[index];
            index++;
            return enemy;
        }
    }

    [SerializeField] List<SpawnGroup> spawnGroups = new();

    internal SpawnGroup currentGroup;
    private int groupIndex;

    private void Start()
    {
        currentGroup = spawnGroups[0];
        nextEnemy = currentGroup.GetEnemy();
        groupIndex = 0;
    }

    internal void GetNextGroup()
    {
        groupIndex++;
        if (groupIndex < spawnGroups.Count)
        {
            currentGroup = spawnGroups[groupIndex];
        }
        else
        {
            spawnerExhausted = true;
            print("Spawner Exhausted");
        }
    }

    internal void BeginSpawn()
    {
        if (!currentGroup.queueFinished)
        {
            if (nextEnemy != null)
            {
                EnemyManager.instance.Spawn(transform, nextEnemy);
            }

            nextEnemy = currentGroup.GetEnemy();
        }
    }
}
