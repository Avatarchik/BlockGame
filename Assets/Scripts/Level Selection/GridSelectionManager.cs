using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridSelectionManager : MonoBehaviour
{
    void Awake()
    {
        StateMachine.state = GameState.GridSelector;
        SaveLoad.LoadProgress();
        SaveLoad.LoadMaps();
    }
}
