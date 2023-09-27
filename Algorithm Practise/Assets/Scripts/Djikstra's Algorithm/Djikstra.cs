using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Legacy;
using UnityEngine;
using System.Linq;

public class Djikstra : MonoBehaviour
{
    private Node start;
    private Node end;
    private Node current;
    private Dictionary<Node, int> unexploredNodes;

    private void Awake() {
        unexploredNodes = new Dictionary<Node, int>();    
    } 

    private void Start() {
        //CreateWeigtedGraph();
        //StartCoroutine(BeginAlgoritm());
    }

    private void CreateWeigtedGraph(){
        start = new Node(0, "Start");
        Node node1 = new Node(int.MaxValue, "Node1");
        Node node2 = new Node(int.MaxValue, "Node2");
        end = new Node(int.MaxValue, "End");

        //! This is for testing purposes
        //Debug.Log($"Start: {start}, Node1: {node1}, Node2: {node2}, End: {end}");
        start.connections.Add((node1, 2));
        start.connections.Add((node2, 3));

        node1.connections.Add((start, 2));
        node1.connections.Add((end, 3));

        node2.connections.Add((start, 3));
        node2.connections.Add((end, 1));
    }

    private IEnumerator BeginAlgoritm(){
        current = start;
        current.explored = true; // The start has already been explored since we start there
        while(!end.explored){
            //Debug.Log($"Exploring {current.name}");
            ExploreConnections();
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Found the shortest path");
        StartCoroutine(DisplayPath());
    }

    private void ExploreConnections(){
        Node nextNode = new Node(int.MaxValue, "Temp");
        //current = current.connections[0].node;
        foreach((Node node, int connectionValue) in current.connections){
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
        Node current = end;
        //Debug.Log(start.prev);
        while(current != null){
            Debug.Log(current.name); // This will be backwards
            current = current.prev;
            yield return new WaitForSeconds(0.1f);
        }

    }
}
