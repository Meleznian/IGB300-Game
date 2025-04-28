using UnityEngine;

public class LinkedNodes : MonoBehaviour
{
    //Index of this node
    public int index;

    //Game Objects of each linked node
    public GameObject[] linkedNodeObjects;

    //Indexes of each linked node
    public int[] linkedNodesIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Creates Linked Nodes array
        linkedNodesIndex = new int[linkedNodeObjects.Length];

        //Fills Linked Nodes array
        for(int i = 0; i < linkedNodeObjects.Length; i++)
        {
            linkedNodesIndex[i] = linkedNodeObjects[i].GetComponent<LinkedNodes>().index;
        }
    }

}
