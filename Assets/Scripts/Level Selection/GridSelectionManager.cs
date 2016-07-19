using UnityEngine;
using System.Collections;

public class GridSelectionManager : MonoBehaviour
{
    void Awake()
    {
        StateMachine.state = GameState.GridSelector;
        SaveLoad.LoadProgress();
        SaveLoad.LoadMaps();
    }
}
