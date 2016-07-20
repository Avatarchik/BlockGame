using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GridSelectionButton : MonoBehaviour {

    public int gridSize;
    public Text percentageText;

    public int completedLevels;
    public int totalLevels;  

	void Start ()
    {
        totalLevels = 0;
        completedLevels = 0;

        for (int i = 0; i < SaveLoad.savedMaps[gridSize].Count; i++)
        {
            if (SaveLoad.savedMaps[gridSize][i] != null)
            {
                totalLevels++;
                if (SaveLoad.mapsCompleted[gridSize, i])
                    completedLevels++;
            }
        }
        float percentage = (completedLevels * 100f) / totalLevels;
        percentageText.text = Mathf.FloorToInt(percentage) + "%";
    }

    public void ButtonClick()
    {
        Debug.LogWarning("Going to Level Selector " + gridSize);

        StateMachine.currentGridSize = gridSize;
        StateMachine.state = GameState.LevelSelector;
        SceneManager.LoadScene("Level Selector", LoadSceneMode.Single);
    }
}
