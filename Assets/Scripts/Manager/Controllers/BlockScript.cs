using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockScript : MonoBehaviour
{
    public List<BlockTile> tileList = new List<BlockTile>();
    public int bNumber;
    public int spawnNumber;
    public bool bPlaced;
    public Vector2 bPos;

    public int rotIndex;
    public string[] rotID = new string[4];
    public Vector2[] rotPos = new Vector2[4];

    public Color bColor;

    public Vector2 solutionPos;
    public int solutionIndex;
    public bool bTip;

    #region Notifications
    void OnEnable()
    {
        this.AddObserver(Pause, UIManager.PauseNotification);
        this.AddObserver(LevelCompleted, LogicManager.LevelCompletedNotification);
    }

    void OnDisable()
    {
        this.RemoveObserver(Pause, UIManager.PauseNotification);
        this.RemoveObserver(LevelCompleted, LogicManager.LevelCompletedNotification);
    }

    void Pause(object sender, object info)
    {
        bool paused = GameManager.Instance.gamePaused;
        foreach (BlockTile tile in tileList)
            tile.GetComponent<BoxCollider2D>().enabled = !paused;
    }

    void LevelCompleted(object sender, object info)
    {
        foreach (BlockTile tile in tileList)
            tile.GetComponent<BoxCollider2D>().enabled = false;
    }
    #endregion

    void Awake()
    {
        bNumber = GameManager.Instance.activeBlocks.Count;
        spawnNumber = bNumber;
        GameManager.Instance.activeBlocks.Add(gameObject);

        bColor = GridScript.Instance.blocksColor[bNumber];

        transform.position = SpawnScript.Instance.spawnLocations[bNumber].transform.position - Vector3.forward;
        SpawnScript.Instance.spawnLocations[bNumber].GetComponentInChildren<RotationScript>().block = gameObject;

        RespawnBlock();
    }

    public void RotateBlock()
    {
        rotIndex++;
        rotIndex = rotIndex % 4;
        RespawnBlock();
    }

    void CreateBlockSprite()
    {
        bColor = GridScript.Instance.blocksColor[bNumber];

        int x = (int)rotPos[rotIndex].x;
        int y = (int)rotPos[rotIndex].y;

        for (int n = 0; n < rotID[rotIndex].Length; n++)
        {
            if (rotID[rotIndex][n] == '1')
            {
                GameObject baseSquare = Instantiate(Resources.Load("Prefabs/Base Block Square")) as GameObject;
                baseSquare.transform.localPosition = new Vector3(x, y, 0);
                baseSquare.GetComponent<SpriteRenderer>().color = bColor;
                baseSquare.transform.parent = transform;

                BlockTile bTile = baseSquare.GetComponent<BlockTile>();
                bTile.relativePos = new Vector2(x, y);
                bTile.parentBlock = this;
                bTile.bNumber = bNumber;
                baseSquare.name = this.gameObject.name + bTile.relativePos.ToString();
                tileList.Add(bTile);
            }

            x++;
            if (x >= 3 && y >= 0)
            {
                x = 0;
                y--;
            }
        }


        this.transform.localScale = new Vector3(SpawnScript.Instance.blockScale, SpawnScript.Instance.blockScale, 1f);
        bPlaced = false;

        foreach (BlockTile bTile in tileList)
        {
            bTile.GetComponent<SpriteRenderer>().color = bColor;
            bTile.transform.localPosition = bTile.relativePos;
            bTile.transform.localScale = new Vector3(0.9f, 0.9f, 0);
        }
    }

    void RespawnBlock()
    {
        DestroyBlockSprite();
        CreateBlockSprite();

        foreach (BlockTile bTile in tileList)
        {
            bTile.transform.localScale = new Vector3(0.9f, 0.9f, 0);
            bTile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/stoneBlock");
        }
    }

    void DestroyBlockSprite()
    {
        tileList.Clear();
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }
}