using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeObject : MonoBehaviour
{
    private Node node;
    private TextMeshProUGUI estimateText;

    private void Awake() {
        node = new Node(int.MaxValue, gameObject.name);
        estimateText.text = node.estimate.ToString();
    }

    public void SetEstimate(int estimate){
        node.estimate = estimate;
        estimateText.text = node.estimate.ToString();
    }

    public void AddConnection(NodeObject nodeObject, int connectionValue){
        node.connections.Add((nodeObject.node, connectionValue));
    }
}
