using UnityEngine;

public class WaypointGraph : MonoBehaviour
{

    public GameObject[] graphNodes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < graphNodes.Length; i++)
        {
            graphNodes[i].GetComponent<LinkedNodes>().index = i;
        }
    }

}
