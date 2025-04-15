using UnityEngine;

public class LinkedNodes : MonoBehaviour
{
    public int index;

    public GameObject[] linkedNodeObjects;

    public int[] linkedNodesIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        linkedNodesIndex = new int[linkedNodeObjects.Length];

        for(int i = 0; i < linkedNodeObjects.Length; i++)
        {
            linkedNodesIndex[i] = linkedNodeObjects[i].GetComponent<LinkedNodes>().index;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
