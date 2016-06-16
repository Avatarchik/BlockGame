using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Game : MonoBehaviour {

    public int levelNumber;
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
