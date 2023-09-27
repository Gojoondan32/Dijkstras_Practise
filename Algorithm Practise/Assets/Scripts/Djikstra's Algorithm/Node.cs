using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public List<(Node node, int connectionValue)> connections;
    public Node prev;
    public int estimate;
    public bool explored;
    public string name;

    public Node(int estimate, string name)
    {
        //this.connections = connections;
        connections = new List<(Node node, int connectionValue)>();
        prev = null;
        this.estimate = estimate;
        explored = false;
        this.name = name;
    }

    public override string ToString()
    {
        return estimate == int.MaxValue ? "∞" : base.ToString();
    }

}
