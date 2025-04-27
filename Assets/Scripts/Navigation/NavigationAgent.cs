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

    /// <summary>
    /// Runs the A* Pathfinding Algorithm to produce a path to a goal node
    /// </summary>
    /// <param name="start">The position where the Agent currently is</param>
    /// <param name="end">The Goal Node</param>
    /// <returns>The shortest path from "Start" to "End"</returns>
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

    /// <summary>
    /// Calculates a Heuristic to weigh which nodes to visit
    /// </summary>
    /// <param name="a">The first node</param>
    /// <param name="b">The second node</param>
    /// <returns>The distance between the two nodes</returns>
    public float Heuristic(int a, int b)
    {
        return Vector3.Distance(graphNodes.graphNodes[a].transform.position, graphNodes.graphNodes[b].transform.position);
    }

    /// <summary>
    /// Calculates the best F Score in the Open List for A* Pathfinding
    /// </summary>
    /// <param name="start">The starting node</param>
    /// <param name="end">The goal node</param>
    /// <returns>The node with the shortest distance to both the start, and the goal, in the open list</returns>
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

    /// <summary>
    /// Reconstructs the A* Pathfinding path
    /// </summary>
    /// <param name="CF">A dictionary representing the previous node for each node in the path</param>
    /// <param name="current">The current node being reconstructed</param>
    /// <returns>The Path produced by A* Pathfinding</returns>
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

    /// <summary>
    /// The Greedy Search Algorithm -- It's a recursive function
    /// </summary>
    /// <param name="current">The current node being visited</param>
    /// <param name="end">The goal node</param>
    /// <param name="path">The current path produced</param>
    /// <returns>A path to the goal node from the current node</returns>
    public List<int> GreedySearch(int current, int end, List<int> path)
    {
        //Sort Children based on Heuristic
        //Debug.Log(end);
        //if (!greedyPaintList.Contains(current)) greedyPaintList.Add(current);
        //Debug.Log(current);
        if (current == currentNodeIndex) greedyPaintList.Add(current);

        //Debug.Log("Grabbing children");
        List<GreedyChildren> children = new List<GreedyChildren>();
        //Debug.Log(graphNodes.graphNodes[current].GetComponent<LinkedNodes>().linkedNodesIndex.Length);
        //Debug.Log("Children Count");
        for (int i = 0; i < graphNodes.graphNodes[current].GetComponent<LinkedNodes>().linkedNodesIndex.Length; i++) {
            //Debug.Log("Filling children");
            children.Add(new GreedyChildren(graphNodes.graphNodes[current].GetComponent<LinkedNodes>().linkedNodesIndex[i], Heuristic(graphNodes.graphNodes[current].GetComponent<LinkedNodes>().linkedNodesIndex[i], end)));
        }
        //Debug.Log(children.Count);
        //Debug.Log("Sorting children");
        children.Sort();
        //children.Reverse();
        //Log(children[0].childID);


        //Loop through children
        for (int i = 0; i < children.Count; i++)
        {
            //Debug.Log(i);
            int child = children[i].childID;
            //if child not painted
            if (!greedyPaintList.Contains(child))
            {
                //Debug.Log("Not painted");
                //paint the child node
                greedyPaintList.Add(child);
                //base case: goal node
                if (child == end)
                {
                    //Debug.Log("Goal Found");
                    path.Add(child);
                    return path;
                }
                //recurse search on child node
                path = GreedySearch(child, end, path);
                //path found, unwind stack
                if(path != null)
                {
                    //Debug.Log("Path Returning");
                    path.Add(child);
                    return path;
                }
            }
        }
       // Debug.Log("Failed");
        return path;
    }
}
