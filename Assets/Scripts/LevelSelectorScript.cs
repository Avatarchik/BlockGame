using UnityEngine;
using System.Collections;

public class LevelSelectorScript : MonoBehaviour {
    
    public int gridSize;
    public int levelToLoad;

    void Awake()
    {
        Screen.fullScreen = false;
    }

    public void Click()
    {
        PlayerSave.currentGridSize = gridSize;
        PlayerSave.currentLevel = levelToLoad;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Base Map", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
