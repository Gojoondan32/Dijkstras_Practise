using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Transform _nodeParent;
    [SerializeField] private NodeObject _nodePrefab;
    [SerializeField] private NodeConnection _nodeConnectionPrefab;   
    private bool _isBuilding;

    private NodeConnection _activeNodeConnection;
    private NodeObject _node1;
    private NodeObject _node2;
    
    private void Awake() {
        GameStateManager.Instance.OnGameStateChange += GameStateChanged;
        _activeNodeConnection = null;
        _node1 = null;
        _node2 = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isBuilding) return;
        // If space is empty, create a node
        // If space is not empty, use the line renderer to create a connection between two nodes
        if(Input.GetMouseButtonDown(1)){
            RaycastHit2D hit = Physics2D.Raycast(GetMousePosition(), Vector2.zero);

            // If we hit a node and we don't have a current connection, create a connection
            // If we hit a node and we have a current connection, snap the line renderer to the node
            // If we don't hit a node, create a node
            // If we hit a connection, do nothing

            if(hit.collider == null){
                CreateNode(GetMousePosition());
                return;
            }

            if(hit.collider.TryGetComponent<NodeConnection>(out NodeConnection nodeConnection)){
                return; // We are currently trying to put something in an input field so don't do anything
            }
            else if(_activeNodeConnection == null){
                // Create a connection between two nodes
                // Store the current node hit 
                _node1 = hit.collider.GetComponent<NodeObject>();
                NodeConnection newNodeConnection = Instantiate(_nodeConnectionPrefab, _nodeParent);
                _activeNodeConnection = newNodeConnection;
                _activeNodeConnection.SetPosition(hit.collider.transform.position, 0);
            }
            else if(_activeNodeConnection != null){
                // Snap the line renderer to the node
                _node2 = hit.collider.GetComponent<NodeObject>();
                _activeNodeConnection.SetPosition(hit.collider.transform.position, 1);
                _activeNodeConnection.SetCostPosition();
                CreateConnection();
            }
            
        }

        _activeNodeConnection?.SetPosition(GetMousePosition(), 1);
    }

    private Vector3 GetMousePosition(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    private void CreateNode(Vector3 position){
        Instantiate(_nodePrefab, position, Quaternion.identity, _nodeParent);
        
    }

    private void CreateConnection(){
        //! Use _activeNodeConnection.Cost in future
        //? Could I use activeNodeConnection.Cost initially and then update it later?
        int cost = Random.Range(1, 10);
        _node1.AddConnection(_node2, cost);
        _node2.AddConnection(_node1, cost);
        _activeNodeConnection.SetConnectionCost(cost);

        // Reset everything
        _activeNodeConnection = null;
        _node1 = null;
        _node2 = null;

    }

    private void GameStateChanged(GameState gameState){
        if(gameState == GameState.NodeBuilder) _isBuilding = true;
        else _isBuilding = false;
    }
}
