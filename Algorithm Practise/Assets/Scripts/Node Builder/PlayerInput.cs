using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Transform _nodeParent;
    [SerializeField] private GameObject _nodePrefab;
    [SerializeField] private NodeConnection _nodeConnectionPrefab;   

    private NodeConnection _activeNodeConnection;
    
    private void Awake() {
        _activeNodeConnection = null;
    }

    // Update is called once per frame
    void Update()
    {
        // If space is empty, create a node
        // If space is not empty, use the line renderer to create a connection between two nodes
        if(Input.GetMouseButtonDown(0)){
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
                NodeConnection newNodeConnection = Instantiate(_nodeConnectionPrefab, _nodeParent);
                _activeNodeConnection = newNodeConnection;
                _activeNodeConnection.SetPosition(hit.collider.transform.position, 0);
            }
            else if(_activeNodeConnection != null){
                // Snap the line renderer to the node
                _activeNodeConnection.SetPosition(hit.collider.transform.position, 1);
                _activeNodeConnection.SetCostPosition();
                _activeNodeConnection = null;
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
}
