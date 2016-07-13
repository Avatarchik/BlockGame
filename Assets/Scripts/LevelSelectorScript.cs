using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class LevelSelectorScript : MonoBehaviour
{

    public int gridSize;
    public int levelToLoad;

    void Awake()
    {
        SaveLoad.LoadProgress();
        if (SaveLoad.mapsCompleted[gridSize, levelToLoad] == true)
        {
            GetComponentInChildren<Text>().text += "*";
        }
    }

    public void Click()
    {
        PlayerSave.currentGridSize = gridSize;
        PlayerSave.currentLevel = levelToLoad;
        SaveLoad.LoadMaps();
        try
        {
            Game currentGame = SaveLoad.savedMaps[gridSize][levelToLoad];
        }
        catch
        {
            Debug.LogWarning("Não foi possível carregar o mapa " + gridSize + "x" + levelToLoad);
            return;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Base Map", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
