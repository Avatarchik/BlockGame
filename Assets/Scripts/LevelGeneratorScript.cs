using UnityEngine;
using System.Collections;

public class LevelGeneratorScript : MonoBehaviour
{

    int gridSize { get { return SpawnScript.Instance.gridSize; } }
    int blockSize { get { return SpawnScript.Instance.blockSize; } }
    GameObject[,] gridGO { get { return GridScript.Instance.gridGO; } set { GridScript.Instance.gridGO = value; } }
    public int gridFree;

    //public IEnumerator TryMove(GameObject blockModel, Vector2 position)
    //{
    //    //Espera proximo frame para fazer Start() do bloco e garar sList
    //    yield return new WaitForFixedUpdate();

    //    ////Rotaciona o bloco 0 a 3 vezes
    //    //for (int r = 0; r < Random.Range(0, 4); r++)
    //    //    blockModel.GetComponent<BlockScript>().RotateMatrix(true);

    //    //int pontoX = Random.Range(0, gridSize - (blockSize - 1));
    //    //int pontoY = Random.Range(0, gridSize - (blockSize - 1));


    //    string gridSpace = GridLocID(position);
    //    int nBlocks = gridSpace.Split('1').Length - 1;
    //    nBlocks = Mathf.Clamp(nBlocks, 0, 5);
    //    Debug.Log(nBlocks);
    //    if (nBlocks <= 1)
    //        yield break;

    //    foreach (GameObject block in SpawnScript.Instance.blocksList[nBlocks])
    //    {
    //        for (int r = 0; r < 4; r++)
    //        {
    //            block.GetComponent<BlockScript>().RotateMatrix(true);
    //            if (block.GetComponent<BlockScript>().blockID == gridSpace)
    //            {
    //                if (CheckPosition(blockModel, position))
    //                {
    //                    MoveBlockGrid(blockModel, position);
    //                    blockModel.transform.localScale = Vector3.one;
    //                    Debug.Log(blockModel + "posicionado em " + position.x + " " + position.y);
    //                    gridFree -= blockModel.GetComponent<BlockScript>().sList.Count;
    //                }
    //                else
    //                    Destroy(blockModel);

    //            }
    //        }
    //    }



    //}

    void Start()
    {
        gridFree = gridSize * gridSize;
    }


    public void Click2()
    {
        for (int n = 0; n < 100; n++)
        {
            Vector2 ponto = new Vector2(Random.Range(0, gridSize), Random.Range(0, gridSize));


            GameObject block = SpawnScript.Instance.blocksList[5][Random.Range(0, SpawnScript.Instance.blocksList[5].Count)] as GameObject;
            GameObject randomBlock = Instantiate(block);

            if (CheckPosition(randomBlock, ponto))
            {
                MoveBlockGrid(randomBlock, ponto);
                randomBlock.transform.localScale = Vector3.one;
                randomBlock.transform.position = Vector3.zero;
                Debug.Log(randomBlock + "posicionado em " + ponto.x + " " + ponto.y);
                gridFree -= randomBlock.GetComponent<BlockScript>().sList.Count;
            }
            else
                Destroy(randomBlock);
        }

        Debug.Log("--------------------------");
        CompleteGrid();
    }

    //Cria string de um local 3x3 do grid. 1 = livre, 0 = ocupado
    string GridLocID(Vector2 position)
    {
        string blockID;
        int bitPos = 0;
        int bID = 0;

        for (int y = (int)position.y; y < (int)position.y + blockSize; y++)
        {
            for (int x = (int)position.x + (blockSize - 1); x >= (int)position.x; x--)
            {
                if (gridGO[x, y].GetComponent<SquareScript>().sType == SquareType.GridEmpty)
                {
                    bID += (int)Mathf.Pow(2, bitPos);
                }
                bitPos++;
            }
        }
        blockID = System.Convert.ToString(bID, 2);
        char[] removeChars = { '0' };
        return blockID.TrimEnd(removeChars);
    }

    //Checa todos os espaços 3x3 do grid e tenta preencher com um bloco
    void CompleteGrid()
    {
        bool teste = false;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                string gridSpace = GridLocID(new Vector2(i, j));
                int nBlocks = gridSpace.Split('1').Length - 1;

                if (nBlocks >= 2 && nBlocks <= 5)
                {
                    foreach (GameObject block in SpawnScript.Instance.blocksList[nBlocks])
                    {
                        GameObject blockGO = Instantiate(block);
                        for (int r = 0; r < 4; r++)
                        {
                            if (gridSpace == blockGO.GetComponent<BlockScript>().rotationIDs[r])
                            {
                                for (int rotations = r; rotations > 0; rotations--)
                                    blockGO.GetComponent<BlockScript>().RotateMatrix(true);


                                for (int x = 0; x < blockSize; x++)
                                {
                                    for (int y = 0; y < blockSize; y++)
                                    {
                                        if (CheckPosition(blockGO, new Vector2(i + x, j + y)))
                                        {
                                            MoveBlockGrid(blockGO, new Vector2(i + x, j + y));
                                            blockGO.transform.localScale = Vector3.one;
                                            blockGO.transform.position = Vector3.zero;
                                            Debug.Log(blockGO + "posicionado em " + (i + x).ToString() + " " + (j + y).ToString());
                                            teste = true;
                                            gridFree -= blockGO.GetComponent<BlockScript>().sList.Count;
                                        }
                                    }
                                }

                            }
                        }
                        if (!teste)
                            Destroy(blockGO);
                        teste = false;
                    }
                }
                gridSpace = GridLocID(new Vector2(i, j));
                nBlocks = gridSpace.Split('1').Length - 1;
                if (nBlocks == 1)
                    GridScript.Instance.FillGrid(new Vector2(i, j));
            }
        }
    }

    bool CheckPosition(GameObject blockGO, Vector2 destiny)
    {

        foreach (SquareScript square in blockGO.GetComponent<BlockScript>().sList)
        {
            //Bloco esta dentro do grid?
            Vector2 squarePos = destiny + square.relativePos;
            if (squarePos.x < 0 || squarePos.x >= SpawnScript.Instance.gridSize || squarePos.y < 0 || squarePos.y >= SpawnScript.Instance.gridSize)
            {
                //Debug.Log("Bloco fora do grid!");
                return false;
            }

            //Existem outros blocos la?
            if (GridScript.Instance.gridGO[(int)squarePos.x, (int)squarePos.y].GetComponent<SquareScript>().sType != SquareType.GridEmpty)
            {
                //Debug.Log("Não pode ser colocado " + blockGO + " em " + destiny.x + "," + destiny.y + " pois ja existe um bloco la");
                blockGO.GetComponent<BlockScript>().bPlaced = false;
                return false;
            }
        }
        return true;
    }

    void MoveBlockGrid(GameObject blockGO, Vector2 destiny)
    {
        Color color = Random.ColorHSV();
        foreach (SquareScript square in blockGO.GetComponent<BlockScript>().sList)
        {
            Vector2 pos = destiny + square.relativePos;
            SquareScript gridSquare = GridScript.Instance.gridGO[(int)pos.x, (int)pos.y].GetComponent<SquareScript>();
            gridSquare.sType = SquareType.GridUsed;
            gridSquare.squareGridPos = pos;
            gridSquare.relativePos = square.relativePos;
            gridSquare.parentBlock = blockGO.GetComponent<BlockScript>();
            gridSquare.bNumber = gridSquare.parentBlock.bNumber;
            gridSquare.GetComponent<SpriteRenderer>().color = color;
            gridSquare.GetComponent<SpriteRenderer>().sortingLayerName = "block";
            blockGO.GetComponent<BlockScript>().bPlaced = true;
            blockGO.GetComponent<BlockScript>().bPos = destiny;

            square.transform.position = new Vector3(5 + square.relativePos.x, square.relativePos.y, 0f);
        }

    }
}
