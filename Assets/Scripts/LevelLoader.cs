using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour {

    #region Singleton Pattern
    private static LevelLoader instance = null;

    public static LevelLoader Instance
    {
        get { return instance; }
    }
    #endregion

    public int gridSize;
    public List<GameObject> activeBlocks = new List<GameObject>();
    public List<Vector2> filledListPos = new List<Vector2>();

    public void LoadValues()
    {
        gridSize = GameManager.Instance.gridSize;
        activeBlocks = GameManager.Instance.activeBlocks;
        filledListPos = GridScript.Instance.filledListPos;
    }
}
