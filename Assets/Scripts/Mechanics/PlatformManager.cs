using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using static PlatformManager;
using UnityEngine.Tilemaps;

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager instance { get; private set; }
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

    bool moving;
    bool addFinished;
    bool removeFinished;
    int added;
    int removed;

    [Serializable]
    public class Platform
    {
        public GameObject platform;
        [SerializeField] internal Vector3 startPos;
        [SerializeField] internal bool reached;
        internal bool setup;

        internal void SetUpPlatform()
        {
            startPos = platform.transform.position;

            CheckColliders(false);

            platform.SetActive(false);
        }

        internal void ActivatePlatform()
        {
            CheckColliders(true);
        }

        internal void DeactivatePlatform()
        {
            platform.SetActive(false);
        }

        internal void CheckColliders(bool on)
        {
            if (platform.GetComponent<TilemapCollider2D>() != null)
            {
                platform.GetComponent<TilemapCollider2D>().enabled = on;
            }
            else if (platform.GetComponent<Collider>() != null)
            {
                platform.GetComponent<Collider>().enabled = on;
            }

            if (platform.transform.childCount != 0)
            {
                foreach (Transform t in platform.transform)
                {
                    if (t.GetComponent<TilemapCollider2D>() != null)
                    {
                        t.GetComponent<TilemapCollider2D>().enabled = on;
                    }
                    else if (platform.GetComponent<Collider>() != null)
                    {
                        platform.GetComponent<Collider>().enabled = on;
                    }
                }
            }
        }
    }

    [Serializable]
    public class PlatformAddSet
    {
        public List<Platform> Platforms = new();
        public int wave;
    }

    [Serializable]
    public class PlatformRemoveSet
    {
        public List<Platform> Platforms = new();
        public int wave;
    }

    public List<PlatformAddSet> platformAddSets = new();
    public List<PlatformRemoveSet> platformRemoveSets = new();

    PlatformAddSet setToAdd;
    PlatformRemoveSet setToRemove;


    [Header("Variables")]
    [SerializeField] float platformMoveSpeed;
    [SerializeField] NodeManager nodeManager;

    //Start
    private void Start()
    {
        SetUpPlatforms();
    }


    // Update is called once per frame
    private void Update()
    {
        if (moving)
        {
            if (setToAdd != null)
            {
                AddPlatforms();
            }
            if (setToRemove != null)
            {
                RemovePlatforms();
            }

            if(removeFinished && addFinished)
            {
                StartRefresh();
                print("Platform Arrangement Finished");
                if (nodeManager.SafeToSpawn)
                {
                    LegacyEnemyManager.instance.StartWave();
                    moving = false;
                    refreshing = false;
                }
            }
        }
    }

    void AddPlatforms()
    {
        foreach(Platform p in setToAdd.Platforms)
        {
            print("moving platform");
            if (!p.reached)
            {
                p.platform.transform.position = Vector3.MoveTowards(p.platform.transform.position, p.startPos,  platformMoveSpeed*Time.deltaTime);

                if (p.platform.transform.position == p.startPos)
                {
                    p.reached = true;
                    p.ActivatePlatform();
                    added++;
                }
            }
        }

        if (added >= setToAdd.Platforms.Count)
        {
            addFinished = true;
        }
    }



    void RemovePlatforms()
    {
        foreach (Platform p in setToRemove.Platforms)
        {
            if (!p.reached)
            {
                p.platform.transform.position = Vector3.MoveTowards(p.platform.transform.position, new Vector3(p.platform.transform.position.x, -10, p.platform.transform.position.z), platformMoveSpeed* Time.deltaTime);

                if (p.platform.transform.position.y == -10)
                {
                    p.DeactivatePlatform();
                    p.reached = true;
                    removed++;
                }
            }
        }

        if (removed >= setToRemove.Platforms.Count)
        {
            removeFinished = true;
        }
    }

    public void BeginMoving()
    {
        print("Starting Platform Arrangement");
        setToAdd = null;
        setToRemove = null;
        addFinished = false;
        removeFinished = false;
        added = 0;
        removed = 0;

        foreach (PlatformAddSet p in platformAddSets)
        {
            if(p.wave == LegacyEnemyManager.instance.currentWave)
            {
                setToAdd = p;
                foreach(Platform pf in setToAdd.Platforms)
                {
                    pf.platform.SetActive(true);
                    pf.reached = false;
                    print("Set To Add Found");
                }
                break;
            }
        }

        foreach (PlatformRemoveSet p in platformRemoveSets)
        {
            if (p.wave == LegacyEnemyManager.instance.currentWave)
            {
                setToRemove = p;
                foreach (Platform pf in setToRemove.Platforms)
                {
                    pf.CheckColliders(false);
                    pf.reached = false;
                }
                print("Set To Remove Found");
                break;
            }
        }

        if(setToAdd == null)
        {
            print(" No Set To Add");
            addFinished = true;
        }
        if(setToRemove == null)
        {
            print(" No Set To Remove");
            removeFinished = true;
        }

        moving = true;
    }


    void SetUpPlatforms()
    {
        foreach (PlatformAddSet p in platformAddSets)
        {
            foreach(Platform pf in p.Platforms)
            {

                pf.SetUpPlatform();
                
            }
        }

        foreach (PlatformAddSet p in platformAddSets)
        {
            foreach (Platform pf in p.Platforms)
            {

                pf.platform.transform.position = new Vector3(pf.platform.transform.position.x, -11, pf.platform.transform.position.z);
                
            }
        }

    }

    bool refreshing;
    void StartRefresh()
    {
        if (!refreshing)
        {
            refreshing = true;
            print("Refreshing Nodes");
            nodeManager.RefreshGraph();
        }
    }
}
