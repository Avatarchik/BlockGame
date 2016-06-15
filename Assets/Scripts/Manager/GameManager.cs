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
    public int tilesLeft;

    public List<GameObject> activeBlocks = new List<GameObject>();
    public List<GameObject>[] allBlocks = new List<GameObject>[6];

    public float rotationSpeed = 3;
    public bool rotatingBlock;

    #region Notifications
    public void OnEnable()
    {
        this.AddObserver(BlockPlaced, BlockTile.BlockPlaced);
        this.AddObserver(BlockRemoved, BlockTile.BlockRemoved);
        this.AddObserver(RotateBlock, BlockTile.RotateBlock);
    }

    public void OnDisable()
    {
        this.RemoveObserver(BlockPlaced, BlockTile.BlockPlaced);
        this.RemoveObserver(BlockRemoved, BlockTile.BlockRemoved);
        this.RemoveObserver(RotateBlock, BlockTile.RotateBlock);
    }

    void BlockPlaced(object sender, object info)
    {
        GameObject block = info as GameObject;
        tilesLeft -= block.GetComponent<BlockScript>().tileList.Count;
        Debug.Log(block.name + " was placed");
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
        StartCoroutine("Rotate", block);
        Debug.Log(block.name + " was rotated");
    }
    #endregion

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

    IEnumerator Rotate(GameObject block)
    {
        rotatingBlock = true;
        int bNumber = block.GetComponent<BlockScript>().bNumber;
        Transform spawnLoc = SpawnScript.Instance.spawnLocations[bNumber].transform;
        for (float i = 0; i < 90 / rotationSpeed; i++)
        {
            block.transform.RotateAround(spawnLoc.position + new Vector3(0.55f, 0.55f, 0), Vector3.forward, -rotationSpeed);
            yield return null;
        }
        block.transform.position = spawnLoc.position - Vector3.forward;
        block.transform.rotation = Quaternion.Euler(Vector3.zero);
        block.GetComponent<BlockScript>().RotateBlock();
        rotatingBlock = false;
    }
}
