using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LogicManager : MonoBehaviour
{
    #region Singleton Pattern
    private static LogicManager instance = null;

    public static LogicManager Instance
    {
        get { return instance; }
    }

    void Awake() { instance = this; }
    #endregion

    public int tilesUsed;
    public int totalTiles;

    public List<GameObject> unplacedBlocks = new List<GameObject>();

    int rotatingSpeed = 8;
    int moveSpeed = 25;

    public const string BlockPlacedNotification = "LogicManager.BlockPlaced";
    public const string BlockRemovedNotification = "LogicManager.BlockRemoved";
    public const string LevelCompletedNotification = "LogicManager.LevelCompleted";

    #region Notifications
    public void OnEnable()
    {
        this.AddObserver(BlockPlaced, BlockPlacedNotification);
        this.AddObserver(BlockRemoved, BlockRemovedNotification);
        this.AddObserver(RotateBlock, RotationScript.RotateBlock);
    }

    public void OnDisable()
    {
        this.RemoveObserver(BlockPlaced, BlockPlacedNotification);
        this.RemoveObserver(BlockRemoved, BlockRemovedNotification);
        this.RemoveObserver(RotateBlock, RotationScript.RotateBlock);
    }

    void BlockPlaced(object sender, object info)
    {
        GameObject block = info as GameObject;
        tilesUsed += block.GetComponent<BlockScript>().tileList.Count;
        unplacedBlocks.Remove(block);
        GameObject.Find("Tiles Text").GetComponent<Text>().text = tilesUsed + "/" + totalTiles;
        Debug.Log(block.name + " was placed on " + block.GetComponent<BlockScript>().bPos);

        if (tilesUsed == totalTiles && StateMachine.state == GameState.InGame)
            LevelCompleted();
    }

    void BlockRemoved(object sender, object info)
    {
        GameObject block = info as GameObject;
        tilesUsed -= block.GetComponent<BlockScript>().tileList.Count;
        unplacedBlocks.Add(block);
        GameObject.Find("Tiles Text").GetComponent<Text>().text = tilesUsed + "/" + totalTiles;
        Debug.Log(block.name + " was removed");
    }

    void RotateBlock(object sender, object info)
    {
        GameObject spawn = info as GameObject;
        spawn.GetComponentInChildren<RotationScript>().rotating = true;
        StartCoroutine("RotateAnimation", spawn);
        Debug.Log(spawn.GetComponent<RotationScript>().block.name + " was rotated");
    }
    #endregion

    public bool CheckPosition(GameObject blockGO, Vector2 destiny)
    {
        BlockScript bs = blockGO.GetComponent<BlockScript>();
        foreach (BlockTile bTile in blockGO.GetComponent<BlockScript>().tileList)
        {
            //Bloco esta dentro do grid?
            Vector2 tilePos = destiny - (bs.rotPos[bs.rotIndex] - bTile.relativePos);
            if (tilePos.x < 0 || tilePos.x >= StateMachine.currentGridSize || tilePos.y < 0 || tilePos.y >= StateMachine.currentGridSize)
            {
                //Debug.Log(blockGO + " fora do grid!");
                return false;
            }

            //Existem outros blocos la?
            if (GridScript.Instance.gridGO[(int)tilePos.x, (int)tilePos.y].GetComponent<GridTile>().gType != GridType.Empty)
            {
                //Debug.Log("Não pode ser colocado " + blockGO + " em " + destiny.x + "," + destiny.y + " pois ja existe um bloco la");
                return false;
            }
        }
        return true;
    }

    public void PlaceBlock(GameObject blockGO, Vector2 destiny)
    {
        BlockScript bs = blockGO.GetComponent<BlockScript>();

        blockGO.transform.localScale = Vector3.one;
        foreach (BlockTile bTile in bs.tileList)
        {
            Vector2 tilePos = destiny - (bs.rotPos[bs.rotIndex] - bTile.relativePos);
            GridTile gTile = GridScript.Instance.gridGO[(int)tilePos.x, (int)tilePos.y].GetComponent<GridTile>();
            gTile.gType = GridType.Used;
            gTile.gridPos = tilePos;
            gTile.parentBlock = bs;
            gTile.bNumber = bs.bNumber;

            bTile.transform.position = new Vector3(tilePos.x, tilePos.y, -1);
            bTile.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
        bs.bPlaced = true;
        bs.bPos = destiny;

        this.PostNotification(BlockPlacedNotification, blockGO);
        RearrangeBlocks();
        SpawnScript.Instance.spawnLocations[unplacedBlocks.Count].GetComponentInChildren<RotationScript>().block = null;
    }

    public void RespawnBlock(GameObject blockGO)
    {
        SpawnScript.Instance.spawnLocations[unplacedBlocks.Count - 1].GetComponentInChildren<RotationScript>().block = blockGO;

        BlockScript bs = blockGO.GetComponent<BlockScript>();
        blockGO.transform.localScale = new Vector3(SpawnScript.Instance.blockScale, SpawnScript.Instance.blockScale, 1f);
        foreach (BlockTile tile in bs.tileList)
            tile.transform.localPosition = tile.relativePos;

        int newPos = bs.bPlaced ? unplacedBlocks.Count - 1 : bs.spawnNumber;
        blockGO.transform.position = SpawnScript.Instance.spawnLocations[newPos].transform.position;

        bs.bPlaced = false;
    }

    public void RearrangeBlocks()
    {
        for (int i = 0; i < unplacedBlocks.Count; i++)
        {
            StartCoroutine(MoveAnimation(unplacedBlocks[i], unplacedBlocks[i].transform.position, SpawnScript.Instance.spawnLocations[i].transform.position, moveSpeed));
            unplacedBlocks[i].GetComponent<BlockScript>().spawnNumber = i;
            SpawnScript.Instance.spawnLocations[i].GetComponentInChildren<RotationScript>().block = unplacedBlocks[i];
        }
    }

    IEnumerator RotateAnimation(GameObject spawn)
    {
        GameObject block = spawn.GetComponent<RotationScript>().block;
        foreach (BlockTile tile in block.GetComponent<BlockScript>().tileList)
            tile.GetComponent<BoxCollider2D>().enabled = false;

        Transform spawnLoc = SpawnScript.Instance.spawnLocations[spawn.GetComponent<RotationScript>().spawnNumber].transform;
        for (float i = 0; i < 90 / rotatingSpeed; i++)
        {
            block.transform.RotateAround(spawnLoc.position + new Vector3(SpawnScript.Instance.blockScale, SpawnScript.Instance.blockScale, 0), Vector3.forward, -rotatingSpeed);
            yield return null;
        }
        block.transform.position = spawnLoc.position - Vector3.forward;
        block.transform.rotation = Quaternion.Euler(Vector3.zero);
        block.GetComponent<BlockScript>().RotateBlock();

        foreach (BlockTile tile in block.GetComponent<BlockScript>().tileList)
            tile.GetComponent<BoxCollider2D>().enabled = true;

        RearrangeBlocks();
        spawn.GetComponent<RotationScript>().rotating = false;
    }

    IEnumerator MoveAnimation(GameObject block, Vector3 startPos, Vector3 endPos, int speed)
    {
        float step = (speed / (startPos - endPos).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            block.transform.position = Vector3.Lerp(startPos, endPos, t); // Move objectToMove closer to b
            yield return null;         // Leave the routine and return here in the next frame
        }
        block.transform.position = endPos;
    }

    void LevelCompleted()
    {
        GameObject.Find("Tiles Text").GetComponent<Text>().text = "DONE!";
        SaveLoad.SaveProgress();
        this.PostNotification(LevelCompletedNotification);
        UIManager.Instance.levelCompletedCanvas.SetActive(true);
    }
}
