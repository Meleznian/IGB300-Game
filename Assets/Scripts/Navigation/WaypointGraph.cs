using System.Collections.Generic;
using UnityEngine;

public class WaypointGraph : MonoBehaviour
{

    public List<GameObject> graphNodes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*for (int i = 0; i < graphNodes.Count; i++)
        {
            graphNodes[i].GetComponent<LinkedNodes>().index = i;
        }*/
    }

    public void AddNodes(GameObject node)
    {
        node.GetComponent<LinkedNodes>().index = graphNodes.Count;
        graphNodes.Add(node);
    }

}
