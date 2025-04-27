using System.Collections.Generic;
using UnityEngine;

public class LinkedNodes : MonoBehaviour
{
    //Index of this node
    public int index;

    //Game Objects of each linked node
    public List<GameObject> linkedNodeObjects;

    //Indexes of each linked node
    public List<int> linkedNodesIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Creates Linked Nodes array
        //linkedNodesIndex = new List<int>();

        /*//Fills Linked Nodes array
        for(int i = 0; i < linkedNodeObjects.Count; i++)
        {
            linkedNodesIndex[i] = linkedNodeObjects[i].GetComponent<LinkedNodes>().index;
        }*/
    }

    public void AddConnection(GameObject to)
    {
        linkedNodeObjects.Add(to);
        linkedNodesIndex.Add(to.GetComponent<LinkedNodes>().index);
    }

}
