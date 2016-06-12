using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    #region Singleton Pattern
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get { return instance; }
    }
    #endregion

    public GridScript gridS;
    public SpawnScript spawnS;

    public int gridSize;
    public int blockSize;

    public List<GameObject> activeBlocks = new List<GameObject>();
    public List<GameObject>[] allBlocks = new List<GameObject>[6];

    void Awake()
    {
        instance = this;
        getAllBlocks();
    }

    void getAllBlocks()
    {
        for (int a = 2; a < allBlocks.Length; a++)
            allBlocks[a] = new List<GameObject>();
        
        for (int i = 2; i < 6; i++)
        {
            Object[] blockFormats = (Object[])Resources.LoadAll(string.Format("Prefabs/Block {0}", i));
            foreach (GameObject block in blockFormats)
            {
                allBlocks[i].Add(block as GameObject);
            }
        }
    }
}
