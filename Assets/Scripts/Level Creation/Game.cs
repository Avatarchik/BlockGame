using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Game {

    public static Game current;
    public int levelNumber;
    public int gridSize;

    public List<int> filledListPosX = new List<int>();
    public List<int> filledListPosY = new List<int>();

    public List<SerializableBlock> sBlockList = new List<SerializableBlock>();
    

    public Game()
    {
        GameManager gm = GameManager.Instance;
        levelNumber = gm.level;
        gridSize = gm.gridSize;
        foreach (Vector2 pos in GridScript.Instance.filledListPos)
        {
            filledListPosX.Add((int)pos.x);
            filledListPosY.Add((int)pos.y);
        }

        sBlockList = GameObject.FindObjectOfType<LevelGeneratorScript>().sBlockList;
    }
}
