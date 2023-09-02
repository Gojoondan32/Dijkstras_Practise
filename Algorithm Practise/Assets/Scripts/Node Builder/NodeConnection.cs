using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeConnection : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    [SerializeField] private Canvas _costText;

    private void Awake() {
        _lineRenderer = transform.GetComponentInChildren<LineRenderer>();
    }

    public void SetPosition(Vector3 position, int index) => _lineRenderer.SetPosition(index, position);

    public void SetCostPosition(){
        Vector3 position = (_lineRenderer.GetPosition(0) + _lineRenderer.GetPosition(1)) / 2;
        position.z = 0;
        _costText.transform.position = position;
    }

    void Update(){

    }
}
