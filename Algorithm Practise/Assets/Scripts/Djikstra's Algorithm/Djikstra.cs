using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Legacy;
using UnityEngine;

public class Djikstra : MonoBehaviour
{
    private Node start;
    private Node end;
    private List<Node> unexploredNodes;

    private void Awake() {
        unexploredNodes = new List<Node>();    
    } 

    private void Start() {
        CreateWeigtedGraph();
        StartCoroutine(BeginAlgoritm());
    }

    private void CreateWeigtedGraph(){
        start = new Node(0, "Start");
        Node node1 = new Node(int.MaxValue, "Node1");
        Node node2 = new Node(int.MaxValue, "Node2");
        end = new Node(int.MaxValue, "End");

        //! This is for testing purposes
        Debug.Log($"Start: {start}, Node1: {node1}, Node2: {node2}, End: {end}");
        start.connections.Add((node1, 2));
        start.connections.Add((node2, 3));

        node1.connections.Add((start, 2));
        node1.connections.Add((end, 3));

        node2.connections.Add((start, 3));
        node2.connections.Add((end, 1));
    }

    private IEnumerator BeginAlgoritm(){
        Node current = start;
        current.explored = true; // The start has already been explored since we start there
        while(!end.explored){
            Debug.Log($"Exploring {current.name}");
            ExploreConnections(current);
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Found the shortest path");
    }

    private void ExploreConnections(Node current){
        Node nextNode = new Node(int.MaxValue, "Temp");
        current = current.connections[0].node;
        foreach((Node node, int connectionValue) in current.connections){
            // Update estimates
            int updatedEstimate = current.estimate + connectionValue;
            if(updatedEstimate < node.estimate) node.estimate = updatedEstimate;

            // Chose next node
            if(node.estimate < nextNode.estimate && !node.explored){
                nextNode = node;
            }
            else if(node.estimate > nextNode.estimate && !node.explored){
                unexploredNodes.Add(node);
            }
        }
        current = nextNode;
        current.explored = true;
    }
}
