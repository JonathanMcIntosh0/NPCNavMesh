using System.Collections.Generic;
using UnityEngine.AI;

public readonly struct Node
{
    public readonly Point pt;
    public readonly int step;      //Index of step in our path (1 step corresponds to a movement of 1 square)

    public Node(Point pt, int step)
    {
        this.pt = pt;
        this.step = step;
    }

    //Includes obstacles! This just a list of possible moves essentially.
    //However this does not include out of bds
    // public List<Node> GetNeighbours()
    // {
    //     var l = new List<Node>();
    //     foreach (var p in pt.GetNeighbours())
    //     {
    //         l.Add(new Node(p, step + 1));
    //     }
    //     l.Add(new Node(pt, step + 1)); //Wait
    //
    //     return l;
    // }
}