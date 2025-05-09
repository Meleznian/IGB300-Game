using System;
using System.Linq.Expressions;
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

    [SerializeField] private int enemyCap;
    [SerializeField] private int currentlyAlive;
    public int gameStage;

    internal void Spawn(Transform pos, GameObject prefab)
    {
        if (currentlyAlive < enemyCap)
        {
            GameObject enemy;

            enemy = Instantiate(prefab, pos.position, pos.rotation);
            currentlyAlive += 1;

            if (enemy.GetComponent<ChargerAgent>() != null)
            {
                enemy.GetComponent<ChargerAgent>().target = GameObject.Find("Player");
                enemy.GetComponent<ChargerAgent>().graphNodes = GameObject.Find("ChargerNodes").GetComponent<WaypointGraph>();
            }
            else if (enemy.GetComponent<KamikazeAgent>() != null)
            {
                enemy.GetComponent<KamikazeAgent>().target = GameObject.Find("Player");
                enemy.GetComponent<KamikazeAgent>().graphNodes = GameObject.Find("ChargerNodes").GetComponent<WaypointGraph>();
            }
        }
    }

    internal void EnemyKilled()
    {
        currentlyAlive -= 1;
    }


}
