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

        StateMachine.currentGridSize = (int)FindObjectOfType<Slider>().value;
    }

    public void LevelCreatorButton()
    {
        StateMachine.state = GameState.LevelCreator;
        SceneManager.LoadScene("Level Creator");
    }

    public void SliderMoved()
    {
        StateMachine.currentGridSize = (int)FindObjectOfType<Slider>().value;
    }
}
