using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCreator : MonoBehaviour
{
    public List<SerializableBlock> sBlockList = new List<SerializableBlock>();

    #region Notifications
    void OnEnable()
    {
        this.AddObserver(BlockPlaced, LogicManager.BlockPlacedNotification);
        this.AddObserver(BlockRemoved, LogicManager.BlockRemovedNotification);
    }

    void OnDisable()
    {
        this.RemoveObserver(BlockPlaced, LogicManager.BlockPlacedNotification);
        this.RemoveObserver(BlockRemoved, LogicManager.BlockRemovedNotification);
    }

    void BlockPlaced(object sender, object info)
    {
        GameObject block = info as GameObject;
        BlockScript bs = block.GetComponent<BlockScript>();
        bs.solutionIndex = bs.rotIndex;
        bs.solutionPos = bs.bPos;
    }

    void BlockRemoved(object sender, object info)
    {
        GameObject block = info as GameObject;
        BlockScript bs = block.GetComponent<BlockScript>();
        bs.solutionIndex = -1;
        bs.solutionPos = Vector2.one * -1;
    }
    #endregion

    void Start()
    {
        LogicManager.Instance.unplacedBlocks = GameManager.Instance.activeBlocks;
        LogicManager.Instance.totalTiles = StateMachine.currentGridSize * StateMachine.currentGridSize;
        GameObject.Find("Tiles Text").GetComponent<Text>().text = LogicManager.Instance.tilesUsed + "/" + LogicManager.Instance.totalTiles;

        SpawnScript.Instance.DeleteExtraSpawns();
        SpawnScript.Instance.FixSpawnsPosition();    
        LogicManager.Instance.RearrangeBlocks();
    }

    public void SaveMap()
    {
        if (LogicManager.Instance.unplacedBlocks.Count != 0 || LogicManager.Instance.tilesUsed != LogicManager.Instance.totalTiles)
        {
            Debug.Log("Existem blocos não posicionados!");
            return;
        }

        Debug.LogWarning("Salvando Mapa");
        sBlockList.Clear();
        foreach (BlockScript bs in FindObjectsOfType<BlockScript>())
        {
            string blockID = bs.gameObject.name.Trim(' ', '(', ')', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0');
            sBlockList.Add(new SerializableBlock(blockID, bs.tileList.Count, bs.solutionIndex, (int)bs.solutionPos.x, (int)bs.solutionPos.y));
        }

        //SaveLoad.SaveMap();
    }

    public void LoadGridSelection()
    {
        Debug.LogWarning("Going to Grid Selector");
        StateMachine.state = GameState.GridSelector;
        SceneManager.LoadScene("Grid Selector");
    }

    public void InsertBlock()
    {
        GameManager.Instance.gamePaused = true;
        this.PostNotification(UIManager.PauseNotification);

        GameObject.Find("BlockSize Panel").SetActive(true);
    }
}
