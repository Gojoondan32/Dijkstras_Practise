using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NodeObject : MonoBehaviour
{
    private Node node;
    public Node Node {get{return node;}}
    [SerializeField] private TextMeshProUGUI _estimateText;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Awake() {
        node = new Node(this, int.MaxValue, gameObject.name);
        _estimateText.text = node.estimate.ToInfinity(); // Converts the int.MaxValue to the infinity symbol
    }

    public void SetEstimate(int estimate){
        node.estimate = estimate;
        _estimateText.text = node.estimate.ToInfinity();
    }

    public void AddConnection(NodeObject nodeObject, int connectionValue){
        node.AddConnection(nodeObject.node, connectionValue);
    }

    public void SetColor(Color color){
        _spriteRenderer.color = color;
    }
}
