using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public GameObject Node;
    public float distance;
    public float radius;
    public List<GameObject> Nodes;
    public float leftBorder;
    public float rightBorder;
    public float topBorder;
    public float bottomBorder;
    public bool SafeToSpawn = false;
    
    [SerializeField] private LayerMask grounds;

    private bool canDrawGizmos;
    [SerializeField] private WaypointGraph graph;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        graph = GetComponent<WaypointGraph>();
         CreateNodes();
    }


    void CreateNodes()
    {
        for (float i = topBorder - (0.5f * distance); i >= bottomBorder; i -= distance)
        {
            for (float j = leftBorder - (0.5f * distance); j <= rightBorder; j += distance)
            {
                //Debug.Log(Physics2D.OverlapCircle(new Vector2(j, i), radius, grounds));
                if (!Physics2D.OverlapCircle(new Vector2(j, i), radius, grounds))
                {
                    Nodes.Add(Instantiate(Node, new Vector3(j, i, 0), Quaternion.identity, transform));
                    graph.AddNodes(Nodes[Nodes.Count - 1]);
                }
            }
        }
        CreateConnections();
    }

    void CreateConnections()
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            for (int j = i + 1; j < Nodes.Count; j++)
            {
                if (Vector2.Distance(Nodes[i].transform.position, Nodes[j].transform.position) <= Mathf.Sqrt(2 * Mathf.Pow(distance, 2)) + 0.001f) { ConnectNodes(Nodes[i], Nodes[j]); ConnectNodes(Nodes[j], Nodes[i]); }
            }
        }
        //canDrawGizmos = true;
        SafeToSpawn = true;
    }

    void ConnectNodes(GameObject from, GameObject to)
    {
        if(from == to) return;

        from.GetComponent<LinkedNodes>().AddConnection(to);

    }

    private void OnDrawGizmos()
    {
        if (canDrawGizmos)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < Nodes.Count; i++)
            {
                for (int j = 0; j < Nodes[i].GetComponent<LinkedNodes>().linkedNodeObjects.Count; j++)
                {
                    Gizmos.DrawLine(Nodes[i].transform.position, Nodes[i].GetComponent<LinkedNodes>().linkedNodeObjects[j].transform.position);
                }
            }
        }
    }

    public void RefreshGraph()
    {
        SafeToSpawn = false;
        SafeToSpawn = false;
        graph.graphNodes.Clear();
        for(int i = 0; i < Nodes.Count; i++)
        {
            Destroy(Nodes[i]);
        }
        Nodes.Clear();
        CreateNodes();
    }

}
