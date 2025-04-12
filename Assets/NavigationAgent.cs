using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NavigationAgent : MonoBehaviour
{
    public WaypointGraph graphNodes;
    public List<int> openList = new List<int>();
    public List<int> closedList = new List<int>();
    public Dictionary<int,int> cameFrom = new Dictionary<int,int>();

    public List<int> currentPath = new List<int>();
    public List<int> greedyPaintList = new List<int>();
    public int currentPathIndex = 0;
    public int currentNodeIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPath.Add(currentNodeIndex);
    }


    public List<int> AStarSearch(int start, int end)
    {
        openList.Clear();
        closedList.Clear();
        cameFrom.Clear();

        openList.Add(start);

        float gScore = 0;
        float fScore = gScore + Heuristic(start, end);
        //Closed List stays empty
        while(openList.Count > 0){
            //Remove best
            int current = bestOpenListFScore(start, end);

            if (current == end) return ReconstructPath(cameFrom, current);

            openList.Remove(current);
            closedList.Add(current);

            //Index children
            for (int i = 0; i < graphNodes.graphNodes[current].GetComponent<LinkedNodes>().linkedNodesIndex.Length; i++) { 
                int child = graphNodes.graphNodes[current].GetComponent<LinkedNodes>().linkedNodesIndex[i];

                if (!closedList.Contains(child))
                {
                    float tentativeGScore = Heuristic(start, current) + Heuristic(current, child);

                    if (!openList.Contains(child) || tentativeGScore < gScore)
                    {
                        openList.Add(child);
                    }

                    if (!cameFrom.ContainsKey(child))
                    {
                        cameFrom.Add(child, current);
                    }
                    gScore = tentativeGScore;
                    fScore = Heuristic(start, child) + Heuristic(child, end);
                }
            }
        }
        return null;
    }

    public float Heuristic(int a, int b)
    {
        return Vector3.Distance(graphNodes.graphNodes[a].transform.position, graphNodes.graphNodes[b].transform.position);
    }

    public int bestOpenListFScore(int start, int end)
    {
        int bestIndex = 0;

        for (int i = 0; i < openList.Count; i++)
        {
            if ((Heuristic(openList[i], start) + Heuristic(openList[i], end)) < (Heuristic(openList[bestIndex], start) + Heuristic(openList[bestIndex], end)))
            {
                bestIndex = i;
            }
        }

        return openList[bestIndex];
    }

    public List<int> ReconstructPath(Dictionary<int, int> CF, int current)
    {
        List<int> finalPath = new List<int>();

        finalPath.Add(current);
        while (CF.ContainsKey(current))
        {
            current = CF[current];
            finalPath.Add(current);
        }
        finalPath.Reverse();
        return finalPath;

    }

    public List<int> GreedySearch(int current, int end, List<int> path)
    {
        return null;
    }
}
