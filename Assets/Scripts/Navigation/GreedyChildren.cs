using System;
using System.Collections.Generic;

public class GreedyChildren : IComparable<GreedyChildren>
{

    public int childID { get; set; }
    public float childHScore { get; set; }

    /// <summary>
    /// Constructor for the Greedy Children Class
    /// </summary>
    /// <param name="childrenID">The Index of the child</param>
    /// <param name="childrenHScore">The HScore of the child</param>
    public GreedyChildren(int childrenID, float childrenHScore)
    {
        this.childID = childrenID;
        this.childHScore = childrenHScore;
    }

    //Comparison Operator
    public int CompareTo(GreedyChildren other)
    {
        return this.childHScore.CompareTo(other.childHScore);
    }

}
