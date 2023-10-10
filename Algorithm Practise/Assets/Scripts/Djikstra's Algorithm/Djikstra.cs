using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Legacy;
using UnityEngine;
using System.Linq;

public class Djikstra : MonoBehaviour
{
    private Node startNode;
    public Node StartNode {private get{return startNode;} set{startNode = value;}}
    private Node endNode;
    public Node EndNode {private get{return endNode;} set{endNode = value;}}
    private Node current;
    private Dictionary<Node, int> unexploredNodes;

    private void Awake() {
        unexploredNodes = new Dictionary<Node, int>();    
    } 

    private void Start() {
        //CreateWeigtedGraph();
        //StartCoroutine(BeginAlgoritm());
    }

    /*
    private void CreateWeigtedGraph(){
        startNode = new Node(0, "Start");
        Node node1 = new Node(int.MaxValue, "Node1");
        Node node2 = new Node(int.MaxValue, "Node2");
        endNode = new Node(int.MaxValue, "End");

        //! This is for testing purposes
        //Debug.Log($"Start: {start}, Node1: {node1}, Node2: {node2}, End: {end}");
        startNode.AddConnection(node1, 2);
        startNode.AddConnection(node2, 3);

        node1.AddConnection(startNode, 2);
        node1.AddConnection(endNode, 3);

        node2.AddConnection(startNode, 3);
        node2.AddConnection(endNode, 1);
    }
    */
    // This is a passthrough function so a button component can call the coroutine
    public void BeginAlgorithm(){
        GameStateManager.Instance.SetGameState(GameState.Djikstra);
        StartCoroutine(BeginAlgoritm());
    }

    private IEnumerator BeginAlgoritm(){
        current = startNode;
        current.explored = true; // The start has already been explored since we start there
        while(!endNode.explored){
            //Debug.Log($"Exploring {current.name}");
            ExploreConnections();
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Found the shortest path");
        StartCoroutine(DisplayPath());
    }

    private void ExploreConnections(){
        Node nextNode = new Node(null, int.MaxValue, "Temp");
        //current = current.connections[0].node;
        foreach((Node node, int connectionValue) in current.Connections){
            //Debug.Log($"Exploring {node.name}, explored status: {node.explored}");
            // Update estimates
            int updatedEstimate = current.estimate + connectionValue;
            if(updatedEstimate < node.estimate) node.estimate = updatedEstimate;


            // Set the previous node 
            if (!node.explored) node.prev = current; // Used to trace back a path

            // Chose next node
            if(!unexploredNodes.ContainsKey(node) && !node.explored) unexploredNodes.Add(node, node.estimate);
            /*
            if(node.estimate < nextNode.estimate && !node.explored){
                nextNode = node;
            }
            else if(node.estimate > nextNode.estimate && !node.explored){
                unexploredNodes.Add(node);
            }
            */
        }
        unexploredNodes = unexploredNodes.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        current = unexploredNodes.First().Key;
        current.explored = true;
        unexploredNodes.Remove(current);
        //Debug.Log($"Chose {current.name} as next node");
    }

    private IEnumerator DisplayPath(){
        Node current = endNode;
        //Debug.Log(start.prev);
        while(current != null){
            Debug.Log(current.name); // This will be backwards
            current.NodeObject.SetColor(Color.green);
            current = current.prev;
            yield return new WaitForSeconds(0.1f);
        }

    }
}
