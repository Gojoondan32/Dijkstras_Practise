using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeObject : MonoBehaviour
{
    private Node node;
    [SerializeField] private TextMeshProUGUI estimateText;

    private void Awake() {
        node = new Node(int.MaxValue, gameObject.name);
        estimateText.text = node.estimate.ToInfinity(); // Converts the int.MaxValue to the infinity symbol
    }

    public void SetEstimate(int estimate){
        node.estimate = estimate;
        estimateText.text = node.estimate.ToInfinity();
    }

    public void AddConnection(NodeObject nodeObject, int connectionValue){
        node.AddConnection(nodeObject.node, connectionValue);
    }
}
