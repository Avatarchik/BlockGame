using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    #region Singleton Pattern
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get { return instance; }
    }
    #endregion

    public int level;
    public int gridSize;
    public bool gamePaused;

    public List<GameObject> activeBlocks = new List<GameObject>();
    public List<GameObject>[] allBlocks = new List<GameObject>[6];


    void Awake()
    {
        instance = this;
        GetAllBlocks();

        if (FindObjectOfType<LevelCreator>())
            StateMachine.state = GameState.LevelCreator;
        else
        {
            if (FindObjectOfType<LevelGeneratorScript>())
                StateMachine.state = GameState.LevelGenerator;
            else
                StateMachine.state = GameState.InGame;
        }
        Debug.Log("STATE: " + StateMachine.state.ToString());

        gridSize = StateMachine.currentGridSize;
        if (StateMachine.state == GameState.InGame)
        {
            level = StateMachine.currentLevel;
            Debug.LogWarning("Trying to load map " + gridSize + "x" + level);
        }
    }

    void Start()
    {
        if (StateMachine.state == GameState.InGame)
        {
            LoadLevel();
            LogicManager.Instance.totalTiles = gridSize * gridSize - GridScript.Instance.filledListPos.Count;
            LogicManager.Instance.unplacedBlocks = activeBlocks;
            GameObject.Find("Tiles Text").GetComponent<Text>().text = LogicManager.Instance.tilesUsed + "/" + LogicManager.Instance.totalTiles;

            SpawnScript.Instance.DeleteExtraSpawns();
            SpawnScript.Instance.FixSpawnsPosition();
        }
    }

    void LoadLevel()
    {
        SaveLoad.LoadMaps();
        Game currentGame = SaveLoad.savedMaps[gridSize][level];
        gridSize = currentGame.gridSize;
        LogicManager.Instance.totalTiles = gridSize * gridSize;

        //Get filled grid positions
        for (int n = 0; n < currentGame.filledListPosX.Count; n++)
        {
            GridScript.Instance.filledListPos.Add(new Vector2(currentGame.filledListPosX[n], currentGame.filledListPosY[n]));
            LogicManager.Instance.totalTiles--;
        }
        GridScript.Instance.FillGrid();

        //Get blocks and resolutions
        foreach (SerializableBlock sBlock in currentGame.sBlockList)
        {
            GameObject blockGO = Instantiate(Resources.Load(string.Format("Prefabs/Block {0}/{1}", sBlock.tilesNumber, sBlock.blockName))) as GameObject;
            blockGO.transform.SetParent(GameObject.Find("Blocks").transform);
            BlockScript bScript = blockGO.GetComponent<BlockScript>();
            bScript.solutionIndex = sBlock.solvedIndex;
            bScript.solutionPos = new Vector2(sBlock.solvedPosX, sBlock.solvedPosY);

            for (int r = 0; r < Random.Range(0, 4); r++)
                blockGO.GetComponent<BlockScript>().RotateBlock();
        }

        StateMachine.state = GameState.InGame;
        Debug.LogWarning("Map Loaded!");
    }

    void GetAllBlocks()
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
