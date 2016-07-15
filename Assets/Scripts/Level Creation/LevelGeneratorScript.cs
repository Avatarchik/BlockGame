using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelGeneratorScript : MonoBehaviour
{
    int gridSize { get { return GameManager.Instance.gridSize; } }
    int level { get { return GameManager.Instance.level; } }
    int blockSize = 3;
    GameObject[,] gridGO { get { return GridScript.Instance.gridGO; } set { GridScript.Instance.gridGO = value; } }

    public List<Vector2> filledListPos = new List<Vector2>();
    public List<SerializableBlock> sBlockList = new List<SerializableBlock>();
    
    Vector2 gridOffset;

    public const string BlockPlaced = "LogicManager.BlockPlaced";

    void Start()
    {
        for (int n = 0; n < 100; n++)
            PlaceRandomBlocks();

        Debug.Log("--------------------------------");

        for (int n = 0; n < 2; n++)
            for (int i = 0; i <= gridSize - blockSize; i++)
                for (int j = 0; j <= gridSize - blockSize; j++)
                    CompleteGrid(i, j);

        FillEmptyGrid();
        SpawnScript.Instance.DeleteExtraSpawns();
    }

    public void Click1()
    {
        SaveLoad.ChangeMap(level);
    }

    public void Click2()
    {
        SaveLoad.SaveMap();
    }      

    #region Private
    //Tenta colocar blocos de 4 ou 5 squares em uma posição aleatoria no grid
    void PlaceRandomBlocks()
    {
        Vector2 destiny = new Vector2(Random.Range(0, gridSize), Random.Range(0, gridSize));

        int squaresNumber = Random.Range(4, 6);
        GameObject randomBlock = GameManager.Instance.allBlocks[squaresNumber][Random.Range(0, GameManager.Instance.allBlocks[squaresNumber].Count)] as GameObject;
        GameObject blockGO = (GameObject)Instantiate(randomBlock, SpawnScript.Instance.spawnLocations[GameManager.Instance.activeBlocks.Count].transform.position, Quaternion.identity);

        for (int rot = 0; rot < Random.Range(0, 4); rot++)
            blockGO.GetComponent<BlockScript>().RotateBlock();

        if (LogicManager.Instance.CheckPosition(blockGO, destiny))
        {
            blockGO.transform.parent = GameObject.Find("Blocks").transform;
            LogicManager.Instance.PlaceBlock(blockGO, destiny);
            BlockScript bs = blockGO.GetComponent<BlockScript>();
            bs.solutionPos = destiny;
            bs.solutionIndex = bs.rotIndex;
            blockGO.transform.position = SpawnScript.Instance.spawnLocations[bs.bNumber].transform.position - Vector3.forward;

            Debug.Log(blockGO.name + " posicionado em " + bs.solutionPos.x + "," + bs.solutionPos.y);
            string blockID = blockGO.name.Replace("(Clone)", "").Trim();
            sBlockList.Add(new SerializableBlock(blockID, squaresNumber, bs.solutionIndex, (int)bs.solutionPos.x, (int)bs.solutionPos.y));
        }
        else
        {
            GameManager.Instance.activeBlocks.Remove(blockGO);
            Destroy(blockGO);
        }
    }

    //Cria string de um local 3x3 do grid. 1 = livre, 0 = ocupado.321654987
    string GridLocID(int posX, int posY)
    {
        string blockID;
        int bitPos = 0;
        int bID = 0;

        for (int y = posY; y < posY + blockSize; y++)
        {
            for (int x = posX + (blockSize - 1); x >= posX; x--)
            {
                if (gridGO[x, y].GetComponent<GridTile>().gType == GridType.Empty)
                {
                    bID += (int)Mathf.Pow(2, bitPos);
                    gridOffset = new Vector2(x, y);
                }
                bitPos++;
            }
        }
        blockID = System.Convert.ToString(bID, 2);
        char[] removeChars = { '0' };
        return blockID.TrimEnd(removeChars);
    }

    //Checa todos os espaços 3x3 do grid e tenta preencher com um bloco
    void CompleteGrid(int i, int j)
    {
        string gridSpace = GridLocID(i, j);
        int nTiles = gridSpace.Split('1').Length - 1;
        bool placed = false;

        if (nTiles >= 2 && nTiles <= 5)
        {
            for (int n = 0; n < GameManager.Instance.allBlocks[nTiles].Count; n++)
            {
                GameObject block = GameManager.Instance.allBlocks[nTiles][n];
                for (int r = 0; r < 4; r++)
                {
                    if (gridSpace == block.GetComponent<BlockScript>().rotID[r] && !placed)
                    {
                        GameObject blockGO = Instantiate(Resources.Load(string.Format("Prefabs/Block {0}/{1}", nTiles, block.name))) as GameObject;

                        //Roda o bloco até estar na posição certa
                        for (int rotations = r; rotations > 0; rotations--)
                            blockGO.GetComponent<BlockScript>().RotateBlock();

                        BlockScript bs = blockGO.GetComponent<BlockScript>();

                        if (LogicManager.Instance.CheckPosition(blockGO, gridOffset))
                        {
                            blockGO.transform.parent = GameObject.Find("Blocks").transform;
                            LogicManager.Instance.PlaceBlock(blockGO, gridOffset);
                            bs.solutionPos = gridOffset;
                            bs.solutionIndex = r;
                            block.transform.position = SpawnScript.Instance.spawnLocations[bs.bNumber].transform.position - Vector3.forward;

                            Debug.Log(blockGO.name + " completado em " + (int)gridOffset.x + "," + (int)gridOffset.y);
                            placed = true;
                            
                            sBlockList.Add(new SerializableBlock(block.name, nTiles, bs.solutionIndex, (int)bs.solutionPos.x, (int)bs.solutionPos.y));
                        }
                        else
                        {
                            GameManager.Instance.activeBlocks.Remove(blockGO);
                            Destroy(blockGO);
                        }
                            
                    }
                }
            }
        }
    }

    //Preenche blocos restantes com GridFilled
    void FillEmptyGrid()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (GridScript.Instance.gridGO[i, j].GetComponent<GridTile>().gType == GridType.Empty)
                {
                    GridScript.Instance.filledListPos.Add(new Vector2(i, j));
                }

            }
        }
        filledListPos = GridScript.Instance.filledListPos;
        GridScript.Instance.FillGrid();
    }
    #endregion
}
