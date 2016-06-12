using UnityEngine;
using System.Collections;

public class BlockTile : Tile
{
    public Vector2 relativePos;


    Vector3 screenPoint;
    Vector3 offset;
    Vector3 originalPos;
    GameObject[,] gridGO { get { return GridScript.Instance.gridGO; } set { GridScript.Instance.gridGO = value; } }

    float clickTime;
    float holdTime = 0.5f;
    bool blockMoved;

    void OnMouseDown()
    {
        if (!GridScript.Instance.paused)
        {
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            originalPos = gameObject.transform.position;
            clickTime = Time.time;
            blockMoved = false;
        }
    }

    void OnMouseDrag()
    {
        if (!GridScript.Instance.paused)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            if (((curPosition - originalPos).sqrMagnitude <= 0.1f) && !parentBlock.bPlaced && !blockMoved)
            {
                if ((Time.time - clickTime > holdTime))
                {
                    clickTime = Time.time;
                    parentBlock.RotateBlock();
                }
            }
            else
            {
                blockMoved = true;
                parentBlock.transform.localScale = Vector3.one;
                RemoveBlockGrid(parentBlock.bNumber);
                ClearGridColor();

                //Traz o bloco para frente na camera
                Vector3 blockPosition = new Vector3(curPosition.x - (transform.localPosition.x * parentBlock.transform.localScale.x), curPosition.y - (transform.localPosition.y * parentBlock.transform.localScale.x), -1f);
                parentBlock.transform.position = blockPosition;

                foreach (BlockTile bTile in this.parentBlock.tileList)
                    bTile.GetComponent<SpriteRenderer>().sortingOrder = 0;

                PaintGridPreview();
            }

        }
    }

    void OnMouseUp()
    {
        if (!GridScript.Instance.paused)
        {

            Vector2 closestGridLoc = new Vector2(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y));

            if (CheckPosition(this.parentBlock.gameObject, closestGridLoc - this.relativePos))
                MoveBlockGrid(this.parentBlock.gameObject, closestGridLoc);

            else
            {
                RemoveBlockGrid(parentBlock.bNumber);
                parentBlock.bPlaced = false;

                Vector3 spawnPos = SpawnScript.Instance.spawnLocations[parentBlock.bNumber].transform.position;
                parentBlock.transform.localPosition = new Vector3(spawnPos.x, spawnPos.y, -1);
                parentBlock.gameObject.transform.localScale = new Vector3(SpawnScript.Instance.blockScale, SpawnScript.Instance.blockScale, 1f);
            }
            if (GridScript.Instance.CheckWin())
            {
                Debug.Log("Voce ganhou!!");
                GridScript.Instance.WinEvent();
            }

        }
        ClearGridColor();

    }


    #region Private
    //Checa se o bloco pode ser posicionado em certo local
    bool CheckPosition(GameObject blockGO, Vector2 destiny)
    {
        Vector2 closestGridLoc = new Vector2(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y));
        foreach (BlockTile square in blockGO.GetComponent<BlockScript>().tileList)
        {
            //Bloco esta dentro do grid?
            Vector2 squarePos = closestGridLoc - this.relativePos + square.relativePos;
            if (squarePos.x < 0 || squarePos.x >= SpawnScript.Instance.gridSize || squarePos.y < 0 || squarePos.y >= SpawnScript.Instance.gridSize)
            {
                //Debug.Log("Bloco fora do grid!");
                return false;
            }

            //Existem outros blocos la?
            Vector2 relPos = destiny + square.relativePos;
            if (gridGO[(int)relPos.x, (int)relPos.y].GetComponent<GridTile>().gType != GridType.Empty)
            {
                Debug.Log("Não pode ser colocado " + blockGO + " em " + destiny.x + "," + destiny.y + " pois ja existe um bloco la");
                blockGO.GetComponent<BlockScript>().bPlaced = false;
                return false;
            }
        }
        return true;
    }

    //Move bloco para posição do grid
    void MoveBlockGrid(GameObject blockGO, Vector2 destiny)
    {
        Color color = blockGO.GetComponent<BlockScript>().bColor;
        foreach (BlockTile bTile in parentBlock.tileList)
        {
            Vector2 pos = (destiny - relativePos) + bTile.relativePos;
            GridTile gTile = gridGO[(int)pos.x, (int)pos.y].GetComponent<GridTile>();
            gTile.gType = GridType.Used;
            gTile.gridPos = pos;
            gTile.parentBlock = parentBlock.GetComponent<BlockScript>();
            gTile.bNumber = gTile.parentBlock.bNumber;
            gTile.GetComponent<SpriteRenderer>().color = color;
            gTile.GetComponent<SpriteRenderer>().sortingLayerName = "block";
            parentBlock.GetComponent<BlockScript>().bPlaced = true;
            parentBlock.GetComponent<BlockScript>().bPos = destiny;

            bTile.transform.position = new Vector3(Mathf.RoundToInt(bTile.transform.position.x), Mathf.RoundToInt(bTile.transform.position.y), 0f);
            bTile.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
    }

    //Remove todos os blocos do grid com o bNumber 
    void RemoveBlockGrid(int blockNumber)
    {
        for (int x = 0; x < SpawnScript.Instance.gridSize; x++)
        {
            for (int y = 0; y < SpawnScript.Instance.gridSize; y++)
            {
                if (gridGO[x, y].GetComponent<GridTile>().bNumber == blockNumber)
                {
                    gridGO[x, y].GetComponent<SpriteRenderer>().sortingLayerName = "grid";
                    gridGO[x, y].GetComponent<SpriteRenderer>().color = Color.grey;
                    gridGO[x, y].transform.parent = GameObject.Find("Grid").transform;
                    gridGO[x, y].name = ("grid pos " + x + "," + y);
                    GridTile gTile = gridGO[x, y].GetComponent<GridTile>();
                    gTile.gType = GridType.Empty;
                    gTile.gridPos = new Vector2(x, y);
                }

            }
        }

        foreach (Vector2 filledPos in GridScript.Instance.filledListPos)
        {
            int x = (int)filledPos.x;
            int y = (int)filledPos.y;
            gridGO[x, y].GetComponent<SpriteRenderer>().color = GridScript.Instance.filledColor;
            gridGO[x, y].GetComponent<GridTile>().bNumber = -1;
            gridGO[x, y].GetComponent<GridTile>().gType = GridType.Filled;
        }
    }

    void ClearGridColor()
    {
        for (int x = 0; x < SpawnScript.Instance.gridSize; x++)
        {
            for (int y = 0; y < SpawnScript.Instance.gridSize; y++)
            {
                gridGO[x, y].GetComponent<SpriteRenderer>().color = Color.grey;
            }
        }

        foreach (Vector2 filledPos in GridScript.Instance.filledListPos)
        {
            int x = (int)filledPos.x;
            int y = (int)filledPos.y;
            gridGO[x, y].GetComponent<SpriteRenderer>().color = GridScript.Instance.filledColor;
        }
    }

    void PaintGridPreview()
    {
        foreach (BlockTile square in parentBlock.tileList)
        {
            Vector2 closestGridLoc = new Vector2(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y));
            Vector2 squarePos = closestGridLoc - this.relativePos + square.relativePos;
            if (squarePos.x >= 0 && squarePos.x < SpawnScript.Instance.gridSize && squarePos.y >= 0 && squarePos.y < SpawnScript.Instance.gridSize)
            {
                Color gridPreviewColor;
                if (CheckPosition(this.parentBlock.gameObject, closestGridLoc - this.relativePos))
                    gridPreviewColor = Color.green;
                else
                    gridPreviewColor = Color.red;

                gridGO[Mathf.RoundToInt(squarePos.x), Mathf.RoundToInt(squarePos.y)].GetComponent<SpriteRenderer>().color = gridPreviewColor;
            }
        }
    }
    #endregion
}
