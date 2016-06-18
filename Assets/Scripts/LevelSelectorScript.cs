using UnityEngine;
using System.Collections;

public class LevelSelectorScript : MonoBehaviour {
    
    public int gridSize;
    public int levelToLoad;

    public void Click()
    {
        PlayerSave.currentGridSize = gridSize;
        PlayerSave.currentLevel = levelToLoad;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Base Map", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
