using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGameState : MonoBehaviour
{
    public void SwitchToNodePicker(){
        GameStateManager.Instance.SetGameState(GameState.NodePicker);
    }
}
