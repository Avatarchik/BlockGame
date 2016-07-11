using UnityEngine;
using System.Collections;

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

    public int tilesLeft;

    public int rotatingSpeed;
    public bool rotatingBlock;

    public const string BlockPlacedNotification = "LogicManager.BlockPlaced";
    public const string BlockRemovedNotification = "LogicManager.BlockRemoved";

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
        Debug.Log(block.name + " was placed on " + block.GetComponent<BlockScript>().bPos);
        tilesLeft -= block.GetComponent<BlockScript>().tileList.Count;

        if (tilesLeft == 0)
            LevelCompleted();      
    }

    void BlockRemoved(object sender, object info)
    {
        GameObject block = info as GameObject;
        tilesLeft += block.GetComponent<BlockScript>().tileList.Count;
        Debug.Log(block.name + " was removed");
    }

    void RotateBlock(object sender, object info)
    {
        GameObject block = info as GameObject;
        StartCoroutine("RotateAnimation", block);
        Debug.Log(block.name + " was rotated");
    }
    #endregion

    public bool CheckPosition(GameObject blockGO, Vector2 destiny)
    {
        BlockScript bs = blockGO.GetComponent<BlockScript>();
        foreach (BlockTile bTile in blockGO.GetComponent<BlockScript>().tileList)
        {
            //Bloco esta dentro do grid?
            Vector2 tilePos = destiny - (bs.rotPos[bs.rotIndex] - bTile.relativePos);
            if (tilePos.x < 0 || tilePos.x >= GameManager.Instance.gridSize || tilePos.y < 0 || tilePos.y >= GameManager.Instance.gridSize)
            {
                //Debug.Log(blockGO + " fora do grid!");
                return false;
            }

            //Existem outros blocos la?
            if (GridScript.Instance.gridGO[(int)tilePos.x, (int)tilePos.y].GetComponent<GridTile>().gType != GridType.Empty)
            {
                //Debug.Log("Não pode ser colocado " + blockGO + " em " + destiny.x + "," + destiny.y + " pois ja existe um bloco la");
                blockGO.GetComponent<BlockScript>().bPlaced = false;
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
    }

    public void RemoveBlockGrid(int blockNumber)
    {
        GameObject block = GameManager.Instance.activeBlocks[blockNumber];
        BlockScript bs = block.GetComponent<BlockScript>();
        for (int x = 0; x < GameManager.Instance.gridSize; x++)
        {
            for (int y = 0; y < GameManager.Instance.gridSize; y++)
            {
                if (GridScript.Instance.gridGO[x, y].GetComponent<GridTile>().bNumber == blockNumber)
                {
                    GridTile gTile = GridScript.Instance.gridGO[x, y].GetComponent<GridTile>();
                    gTile.gType = GridType.Empty;
                    gTile.parentBlock = null;
                    gTile.bNumber = -1;
                }
            }
        }
       if (bs.bPlaced)
            this.PostNotification(BlockRemovedNotification, block);
        bs.bPlaced = false;
    }

    IEnumerator RotateAnimation(GameObject block)
    {
        rotatingBlock = true;
        int bNumber = block.GetComponent<BlockScript>().bNumber;
        Transform spawnLoc = SpawnScript.Instance.spawnLocations[bNumber].transform;
        for (float i = 0; i < 90 / rotatingSpeed; i++)
        {
            block.transform.RotateAround(spawnLoc.position + new Vector3(SpawnScript.Instance.blockScale, SpawnScript.Instance.blockScale, 0), Vector3.forward, -rotatingSpeed);
            yield return null;
        }
        block.transform.position = spawnLoc.position - Vector3.forward;
        block.transform.rotation = Quaternion.Euler(Vector3.zero);
        block.GetComponent<BlockScript>().RotateBlock();
        rotatingBlock = false;
    }

    void LevelCompleted()
    {
        switch (GameManager.Instance.gridSize)
        {
            case 4: PlayerSave.completedLevels4[GameManager.Instance.level] = true; break;
            case 5: PlayerSave.completedLevels4[GameManager.Instance.level] = true; break;
            case 6: PlayerSave.completedLevels4[GameManager.Instance.level] = true; break;
        }
        Debug.LogWarning("Level " + GameManager.Instance.gridSize + "x" + GameManager.Instance.level + " completed!");

        Time.timeScale = 0f;
        UIManager.Instance.levelCompletedCanvas.SetActive(true);
        GameManager.Instance.gamePaused = true;
    }
}
