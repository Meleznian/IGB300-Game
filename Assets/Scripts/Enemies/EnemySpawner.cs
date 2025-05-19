using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject nextEnemy;
    [SerializeField] internal GameObject prev;

    [Serializable]
    public class SpawnGroup 
    {
        [SerializeField] internal List<GameObject> queue = new();
        [SerializeField] internal int wave;
        [SerializeField] int index;
        [SerializeField] internal bool queueFinished;
        [SerializeField] internal bool waitTillPrevDead;

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
    [SerializeField] internal bool spawnerExhausted;
    private int groupIndex;

    private void Start()
    {
        Setup();
    }

    internal void GetNextGroup()
    {
        groupIndex++;

        if(EnemyManager.instance.LogSpawnerStates)
            print(name + " moving onto spawn group " + groupIndex);

        if (groupIndex < spawnGroups.Count)
        {
            currentGroup = spawnGroups[groupIndex];
        }
        else
        {
            spawnerExhausted = true;
            if (EnemyManager.instance.LogSpawnerStates)
                print(name + " is Exhausted");
        }
    
    }

    internal void BeginSpawn()
    {
        if (EnemyManager.instance.currentWave == currentGroup.wave)
        {
            if (!currentGroup.queueFinished)
            {
                if (nextEnemy != null)
                {
                    if (EnemyManager.instance.LogEnemySpawns)
                    {
                        print("Spawning " + nextEnemy.name + " at " + name);
                    }

                    prev = EnemyManager.instance.Spawn(transform, nextEnemy);
                }

                nextEnemy = currentGroup.GetEnemy();
            }
        }
    }

    void Setup()
    {
        currentGroup = spawnGroups[0];
        nextEnemy = currentGroup.GetEnemy();
        groupIndex = 0;
    }


    internal void GenerateGroup(int wave, GameObject[] enemyList)
    {

        SpawnGroup group = new SpawnGroup();
        group.wave = wave;
        int enemyNum = UnityEngine.Random.Range(1, 5);

        while(enemyNum > 0)
        {
            group.queue.Add(enemyList[UnityEngine.Random.Range(0, group.queue.Count)]);
            enemyNum--;
        }

        spawnGroups.Add(group);
    }
}
