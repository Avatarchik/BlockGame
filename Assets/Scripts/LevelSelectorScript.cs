using UnityEngine;
using System.Collections;

public class LevelSelectorScript : MonoBehaviour {
    
    public int typeLevel;
    public int levelToLoad;

    public void Click()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(typeLevel + " map " + levelToLoad);
    }
}
