using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NodeConnection : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    [SerializeField] private TMP_InputField _costInputField;
    public int Cost => int.Parse(_costInputField.text);

    private void Awake() {
        _lineRenderer = transform.GetComponentInChildren<LineRenderer>();
    }

    private void Start() {
        _costInputField.onValueChanged.AddListener(OnCostChanged);
    }

    public void SetPosition(Vector3 position, int index) => _lineRenderer.SetPosition(index, position);

    public void SetCostPosition(){
        Vector3 position = (_lineRenderer.GetPosition(0) + _lineRenderer.GetPosition(1)) / 2;
        position.z = 0;
        transform.position = position;
    }

    //! This is for testing purposes
    public void SetConnectionCost(int value) => _costInputField.text = value.ToString(); 
    //! -----------------------------
    
    private void OnCostChanged(string value){
        Debug.Log("Cost changed to: " + value);
    }
}
