using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    private NodeObject nodeObject; // This is required so we can change the visual representation of this node
    public NodeObject NodeObject { get { return nodeObject; } }

    private List<(Node node, int connectionValue)> connections;
    public List<(Node node, int connectionValue)> Connections { get { return connections; }}

    public Node prev;
    public int estimate;
    public bool explored;
    public string name;

    public Node(NodeObject nodeObject, int estimate, string name)
    {
        this.nodeObject = nodeObject;
        //this.connections = connections;
        connections = new List<(Node node, int connectionValue)>();
        prev = null;
        this.estimate = estimate;
        explored = false;
        this.name = name;
    }

    public void AddConnection(Node node, int connectionValue) => connections.Add((node, connectionValue));

    public override string ToString()
    {
        return estimate == int.MaxValue ? "∞" : base.ToString();
    }

}
