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

            if(hit.collider != null && _activeNodeConnection == null){
                // Create a connection between two nodes
                NodeConnection nodeConnection = Instantiate(_nodeConnectionPrefab, _nodeParent);
                _activeNodeConnection = nodeConnection;
                _activeNodeConnection.SetPosition(hit.collider.transform.position, 0);
            }
            else if(hit.collider != null && _activeNodeConnection != null){
                // Snap the line renderer to the node
                _activeNodeConnection.SetPosition(hit.collider.transform.position, 1);
                _activeNodeConnection.SetCostPosition();
                _activeNodeConnection = null;
            }
            else{
                CreateNode(GetMousePosition());
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
