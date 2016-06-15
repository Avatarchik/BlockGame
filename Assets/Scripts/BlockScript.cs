using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockScript : MonoBehaviour
{
    [SerializeField]
    int[] bLocations;
    public int[,] bMatrix;

    public List<BlockTile> tileList = new List<BlockTile>();
    public int bNumber;
    public bool bPlaced;
    public Vector2 bPos;

    public int rotIndex;
    public string[] rotID = new string[4];
    public Vector2[] rotPos = new Vector2[4];

    public Color bColor;


    void Awake()
    {
        bNumber = GameManager.Instance.activeBlocks.Count;
        GameManager.Instance.activeBlocks.Add(gameObject);

        bColor = GridScript.Instance.blocksColor[bNumber];

        transform.position = SpawnScript.Instance.spawnLocations[bNumber].transform.position - Vector3.forward;
        SpawnScript.Instance.spawnLocations[bNumber].GetComponent<RotationScript>().parentBlock = gameObject;

        if (tileList.Count == 0)
        {
            bMatrix = new int[3, 3];
            CreateBlock();
            GetBlockIDs();
            CreateBlockSprite();
        }

        foreach (BlockTile bTile in tileList)
            bTile.GetComponent<SpriteRenderer>().color = bColor;

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
        }
    }

    void DestroyBlockSprite()
    {
        tileList.Clear();
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

    void CreateBlock()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                bMatrix[i, j] = -1;

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

    void GetBlockIDs()
    {
        for (int r = 0; r < 4; r++)
        {
            int bitPos = 0;
            int bID = 0;
            for (int y = 0; y < 3; y++)
            {
                for (int x = 3 - 1; x >= 0; x--)
                {
                    if (bMatrix[x, y] == bNumber)
                    {
                        bID += (int)Mathf.Pow(2, bitPos);
                        rotPos[r] = new Vector2(x, y);
                    }
                    bitPos++;
                }
            }
            rotID[r] = System.Convert.ToString(bID, 2);
            char[] removeChars = { '0' };
            rotID[r] = rotID[r].TrimEnd(removeChars);

            //Rotate block matrix
            int[,] rotatedMatrix = new int[3, 3];
            int[,] originalMatrix = bMatrix;

            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                    rotatedMatrix[x, y] = originalMatrix[3 - y - 1, x];

            bMatrix = rotatedMatrix;
            rotIndex = (rotIndex % 4);
        }
    }
}