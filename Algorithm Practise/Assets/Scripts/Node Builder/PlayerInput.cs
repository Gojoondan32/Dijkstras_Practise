using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region Class References
    [SerializeField] private Djikstra _djikstra; 
    #endregion
    [SerializeField] private Transform _nodeParent;
    [SerializeField] private NodeObject _nodePrefab;
    [SerializeField] private NodeConnection _nodeConnectionPrefab;   
    private bool _startNodeSelected;

    private NodeConnection _activeNodeConnection;
    private NodeObject _node1;
    private NodeObject _node2;
    
    private void Awake() {
        _activeNodeConnection = null;
        _node1 = null;
        _node2 = null;
        _startNodeSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If space is empty, create a node
        // If space is not empty, use the line renderer to create a connection between two nodes
        if(Input.GetMouseButtonDown(1)){
            RaycastHit2D hit = Physics2D.Raycast(GetMousePosition(), Vector2.zero);

            // If we hit a node and we don't have a current connection, create a connection
            // If we hit a node and we have a current connection, snap the line renderer to the node
            // If we don't hit a node, create a node
            // If we hit a connection, do nothing

            if(GameStateManager.Instance.CurrentGameState == GameState.NodeBuilder){
                BuildNodePhase(hit); // We are trying to build the weighted graph of nodes 
            }
            else if(GameStateManager.Instance.CurrentGameState == GameState.NodePicker){
                PickNodePhase(hit); // We are trying to pick the start and end nodes
            }
        }

        // This makes the line renderer follow the mouse when we are trying to build a connection
        _activeNodeConnection?.SetPosition(GetMousePosition(), 1);
    }

    private void BuildNodePhase(RaycastHit2D hit){
        // We need to create a node if the player clicks on an empty space
        if (hit.collider == null){
            CreateNode(GetMousePosition());
            return;
        }

        //! This might be redundant now that I have switched from using the primary mouse button to the secondary mouse button
        if (hit.collider.TryGetComponent<NodeConnection>(out NodeConnection nodeConnection))
        {
            return; // We are currently trying to put something in an input field so don't do anything
        }
        // We have hit a node and we don't have a current connection
        else if (_activeNodeConnection == null) 
        {
            _node1 = hit.collider.GetComponent<NodeObject>();
            NodeConnection newNodeConnection = Instantiate(_nodeConnectionPrefab, _nodeParent);
            _activeNodeConnection = newNodeConnection;
            _activeNodeConnection.SetPosition(hit.collider.transform.position, 0);
        }
        // We have hit a node and we have a current connection
        else if (_activeNodeConnection != null)
        {
            // Snap the line renderer to the node
            _node2 = hit.collider.GetComponent<NodeObject>();
            _activeNodeConnection.SetPosition(hit.collider.transform.position, 1);
            _activeNodeConnection.SetCostPosition();
            CreateConnection();
        }
    }

    private void PickNodePhase(RaycastHit2D hit){
        if(hit.collider == null) return;

        if(hit.collider.TryGetComponent<NodeObject>(out NodeObject node)){
            if(!_startNodeSelected){
                // Set the start node
                node.SetColor(Color.blue);
                _djikstra.StartNode = node.Node;
                _startNodeSelected = true;
            }
            else{
                // Set the end node
                node.SetColor(Color.red);
                _djikstra.EndNode = node.Node;
            }
            
        }
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
        _startNodeSelected = false;
    }
}
