using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class LevelSelectorButton : MonoBehaviour
{
    public int levelToLoad;
    int gridSize;

    void Start()
    {
        CheckMapExists();
    }

    public void CheckMapExists()
    {
        gridSize = StateMachine.currentGridSize;
        if (levelToLoad < SaveLoad.savedMaps[gridSize].Count)
        {
            gameObject.GetComponent<Button>().interactable = true;

            if (SaveLoad.savedMaps[gridSize][levelToLoad] != null)
            {
                if (SaveLoad.mapsCompleted[gridSize, levelToLoad] == true)
                    GetComponentInChildren<Text>().text += "*";
            }
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void Click()
    {
        StateMachine.currentGridSize = gridSize;
        StateMachine.currentLevel = levelToLoad;
        try
        {
            Game currentGame = SaveLoad.savedMaps[gridSize][levelToLoad];
        }
        catch
        {
            Debug.LogWarning("Não foi possível carregar o mapa " + gridSize + "x" + levelToLoad);
            Debug.LogWarning("Total de mapas de tamanho " + gridSize + ": " + SaveLoad.savedMaps[gridSize].Count);
            return;
        }
        StateMachine.state = GameState.InGame;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Base Map", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
