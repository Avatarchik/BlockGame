using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockScript : MonoBehaviour
{
    [SerializeField]
    int[] bLocations;
    int maxBlockSize;
    public string[] rotationIDs = new string[4];
    public Vector2[] bOffsets;
    public int IDindex;

    public int[,] bMatrix;
    public List<BlockTile> tileList = new List<BlockTile>();
    
    public int bNumber;
    public bool bPlaced;
    public Vector2 bPos;
    public Color bColor;


    void Awake()
    {
        maxBlockSize = SpawnScript.Instance.blockSize;
        bMatrix = new int[maxBlockSize, maxBlockSize];
        bOffsets = new Vector2[4];
        SpawnScript.Instance.activeBlocksNumber++;
        bNumber = SpawnScript.Instance.activeBlocksNumber;
        
        CreateBlock();
        GetBlockIDs();
        CreateBlockSprite();
        transform.position = SpawnScript.Instance.spawnLocations[bNumber - 1].transform.position - Vector3.forward;
        SpawnScript.Instance.spawnLocations[bNumber - 1].GetComponent<RotationScript>().block = this.gameObject;
    }

    void CreateBlock()
    {
        foreach (int loc in bLocations)
        {
            switch (loc)
            {
                case 1: bMatrix[0, 0] = bNumber; break;
                case 2: bMatrix[1, 0] = bNumber; break;
                case 3: bMatrix[2, 0] = bNumber; break;
                case 4: bMatrix[0, 1] = bNumber; break;
                case 5: bMatrix[1, 1] = bNumber; break;
                case 6: bMatrix[2, 1] = bNumber; break;
                case 7: bMatrix[0, 2] = bNumber; break;
                case 8: bMatrix[1, 2] = bNumber; break;
                case 9: bMatrix[2, 2] = bNumber; break;
                default:
                    Debug.Log("Número inválido para criação de bloco" + loc);
                    break;
            }
        }
    }

    public void CreateBlockSprite()
    {
        bColor = GridScript.Instance.blocksColor[bNumber - 1];
        for (int y = 0; y < SpawnScript.Instance.blockSize; y++)
        {
            for (int x = SpawnScript.Instance.blockSize - 1; x >= 0; x--)
            {
                if (bMatrix[x, y] == bNumber)
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
            }
        }
        this.transform.localScale = new Vector3(SpawnScript.Instance.blockScale, SpawnScript.Instance.blockScale, 1f);
        bPlaced = false;
    }  

    public void RespawnBlock()
    {
        DestroyBlockSprite();
        CreateBlockSprite();

        foreach (BlockTile bTile in tileList)
        {
            bTile.transform.localPosition = bTile.relativePos;
            bTile.transform.localScale = new Vector3(0.9f, 0.9f, 0);
        }
    }

    public void RotateMatrix(bool clockwise)
    {
        int[,] rotatedMatrix = new int[maxBlockSize, maxBlockSize];
        int[,] originalMatrix = bMatrix;

        //Rotate block matrix
        for (int x = 0; x < maxBlockSize; x++)
        {
            for (int y = 0; y < maxBlockSize; y++)
            {
                if (clockwise)
                    rotatedMatrix[x, y] = originalMatrix[maxBlockSize - y - 1, x];
                else
                    rotatedMatrix[x, y] = originalMatrix[y, maxBlockSize - x - 1];
            }
        }
        bMatrix = rotatedMatrix;
        IDindex++;
        IDindex = (IDindex % 4);
        RespawnBlock();
    }


    void DestroyBlockSprite()
    {
        tileList.Clear();
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

    void GetBlockIDs()
    {
        for (int r = 0; r < 4; r++)
        {
            int bitPos = 0;
            int bID = 0;
            for (int y = 0; y < SpawnScript.Instance.blockSize; y++)
            {
                for (int x = SpawnScript.Instance.blockSize - 1; x >= 0; x--)
                {
                    if (bMatrix[x, y] == bNumber)
                    {
                        bID += (int)Mathf.Pow(2, bitPos);
                        bOffsets[r] = new Vector2(x, y);
                    }
                    bitPos++;
                }
            }
            rotationIDs[r] = System.Convert.ToString(bID, 2);
            char[] removeChars = { '0' };
            rotationIDs[r] = rotationIDs[r].TrimEnd(removeChars);

            //Rotate block matrix
            int[,] rotatedMatrix = new int[maxBlockSize, maxBlockSize];
            int[,] originalMatrix = bMatrix;
            
            for (int x = 0; x < maxBlockSize; x++)
                for (int y = 0; y < maxBlockSize; y++)
                    rotatedMatrix[x, y] = originalMatrix[maxBlockSize - y - 1, x];

            bMatrix = rotatedMatrix;
            IDindex = (IDindex % 4);
        }
    }

}