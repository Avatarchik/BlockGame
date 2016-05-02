using UnityEngine;
using System.Collections;

public enum SquareType { Block, GridEmpty, GridFilled }

public class SquareScript : MonoBehaviour
{
    public SquareType sType;
    public Vector2 squareGridPos;
    public Vector2 relativePos;
    public BlockScript parentBlock;
    public int bNumber;

    Vector3 screenPoint;
    Vector3 offset;
    GameObject[,] gridGO { get { return GridScript.Instance.gridGO; } set { GridScript.Instance.gridGO = value; } }

    void OnMouseDown()
    {
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        if (this.sType == SquareType.Block)
        {
            parentBlock.transform.localScale = Vector3.one;
        }
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        if (this.sType == SquareType.Block)
        {
            RemoveBlockGrid(parentBlock.bNumber);
            ClearGridColor();

            //Traz o bloco para frente na camera
            Vector3 blockPosition = new Vector3(curPosition.x - (transform.localPosition.x * parentBlock.transform.localScale.x), curPosition.y - (transform.localPosition.y * parentBlock.transform.localScale.x), -1f);
            parentBlock.transform.position = blockPosition;
            PaintGridPreview();
        }
    }

    void OnMouseUp()
    {
        if (this.sType == SquareType.Block)
        {
            Vector2 closestGridLoc = new Vector2(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y));

            if (CheckPosition(this.parentBlock.gameObject, closestGridLoc - this.relativePos))
                MoveBlockGrid(this.parentBlock.gameObject, closestGridLoc);

            else
            {
                RemoveBlockGrid(parentBlock.bNumber);
                parentBlock.bPlaced = false;

                parentBlock.transform.localPosition = SpawnScript.Instance.spawnLocations[parentBlock.bNumber - 1].transform.position;
                parentBlock.gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
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
        foreach (SquareScript square in blockGO.GetComponent<BlockScript>().sList)
        {
            //Bloco esta dentro do grid?
            Vector2 squarePos = closestGridLoc - this.relativePos + square.relativePos;
            if (squarePos.x < 0 || squarePos.x >= GridScript.Instance.gridSize || squarePos.y < 0 || squarePos.y >= GridScript.Instance.gridSize)
            {
                Debug.Log("Bloco fora do grid!");
                return false;
            }

            //Existem outros blocos la?
            Vector2 relPos = destiny + square.relativePos;
            if (gridGO[(int)relPos.x, (int)relPos.y].GetComponent<SquareScript>().sType != SquareType.GridEmpty)
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
        foreach (SquareScript square in parentBlock.sList)
        {
            Vector2 pos = (destiny - relativePos) + square.relativePos;
            SquareScript gridSquare = gridGO[(int)pos.x, (int)pos.y].GetComponent<SquareScript>();
            gridSquare.sType = SquareType.GridFilled;
            gridSquare.squareGridPos = pos;
            gridSquare.relativePos = square.relativePos;
            gridSquare.parentBlock = parentBlock.GetComponent<BlockScript>();
            gridSquare.bNumber = gridSquare.parentBlock.bNumber;
            gridSquare.GetComponent<SpriteRenderer>().color = this.parentBlock.GetComponent<BlockScript>().bColor;
            gridSquare.GetComponent<SpriteRenderer>().sortingLayerName = "block";
            parentBlock.GetComponent<BlockScript>().bPlaced = true;
            parentBlock.GetComponent<BlockScript>().bPos = destiny;

            square.transform.position = new Vector3(Mathf.RoundToInt(square.transform.position.x), Mathf.RoundToInt(square.transform.position.y), 0f);
        }
    }

    //Remove todos os blocos do grid com o bNumber 
    void RemoveBlockGrid(int blockNumber)
    {
        for (int x = 0; x < GridScript.Instance.gridSize; x++)
        {
            for (int y = 0; y < GridScript.Instance.gridSize; y++)
            {
                if (gridGO[x, y].GetComponent<SquareScript>().bNumber == this.bNumber)
                {
                    gridGO[x, y].GetComponent<SpriteRenderer>().sortingLayerName = "grid";
                    gridGO[x, y].GetComponent<SpriteRenderer>().color = GridScript.Instance.gColor;
                    gridGO[x, y].transform.parent = GameObject.Find("Grid").transform;
                    gridGO[x, y].name = ("grid pos " + x + "," + y);
                    SquareScript baseSquareSS = gridGO[x, y].GetComponent<SquareScript>();
                    baseSquareSS.sType = SquareType.GridEmpty;
                    baseSquareSS.squareGridPos = new Vector2(x, y);
                    baseSquareSS.parentBlock = null;
                    baseSquareSS.bNumber = 0;
                }

            }
        }
    }

    void ClearGridColor()
    {
        for (int x = 0; x < GridScript.Instance.gridSize; x++)
        {
            for (int y = 0; y < GridScript.Instance.gridSize; y++)
            {
                gridGO[x, y].GetComponent<SpriteRenderer>().color = Color.grey;
            }
        }
    }

    void PaintGridPreview()
    {
        foreach (SquareScript square in parentBlock.sList)
        {
            Vector2 closestGridLoc = new Vector2(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y));
            Vector2 squarePos = closestGridLoc - this.relativePos + square.relativePos;
            if (squarePos.x >= 0 && squarePos.x < GridScript.Instance.gridSize && squarePos.y >= 0 && squarePos.y < GridScript.Instance.gridSize)
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
