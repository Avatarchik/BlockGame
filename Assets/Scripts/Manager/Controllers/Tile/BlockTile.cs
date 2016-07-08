using UnityEngine;
using System.Collections;

public class BlockTile : Tile
{
    public Vector2 relativePos;

    Vector3 screenPoint;
    Vector3 offset;
    Vector3 originalPos;
    GameObject[,] gridGO { get { return GridScript.Instance.gridGO; } set { GridScript.Instance.gridGO = value; } }

    public const string RotateBlock = "RotationScript.RotateBlock";

    void OnMouseDown()
    {
        if (!GameManager.Instance.gamePaused)
        {
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            originalPos = gameObject.transform.position;
            LogicManager.Instance.RemoveBlockGrid(parentBlock.bNumber);
        }
    }

    void OnMouseDrag()
    {
        if (!GameManager.Instance.gamePaused && !LogicManager.Instance.rotatingBlock)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            parentBlock.transform.localScale = Vector3.one;
            ClearGridColor();

            //Traz o bloco para frente na camera
            Vector3 blockPosition = new Vector3(curPosition.x - (transform.localPosition.x * parentBlock.transform.localScale.x), curPosition.y - (transform.localPosition.y * parentBlock.transform.localScale.x), -1f);
            parentBlock.transform.position = blockPosition;

            foreach (BlockTile bTile in this.parentBlock.tileList)
                bTile.GetComponent<SpriteRenderer>().sortingOrder = 2;

            PaintGridPreview();

        }
    }

    void OnMouseUp()
    {
        if (!GameManager.Instance.gamePaused && !LogicManager.Instance.rotatingBlock)
        {
            Vector2 closestGridLoc = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            Vector2 destiny = closestGridLoc - (relativePos - parentBlock.rotPos[parentBlock.rotIndex]);

            if (LogicManager.Instance.CheckPosition(parentBlock.gameObject, destiny))
            {
                LogicManager.Instance.PlaceBlock(parentBlock.gameObject, destiny);
            }
            else
            {
                LogicManager.Instance.RemoveBlockGrid(parentBlock.bNumber);
                parentBlock.transform.position = SpawnScript.Instance.spawnLocations[bNumber].transform.position - Vector3.forward;
                parentBlock.gameObject.transform.localScale = new Vector3(SpawnScript.Instance.blockScale, SpawnScript.Instance.blockScale, 1f);
                foreach (BlockTile tile in parentBlock.tileList)
                    tile.transform.localPosition = tile.relativePos;
            }
        }
        ClearGridColor();
        foreach (BlockTile bTile in this.parentBlock.tileList)
            bTile.GetComponent<SpriteRenderer>().sortingOrder = 0;
    }


    #region Private
    void ClearGridColor()
    {
        for (int x = 0; x < SpawnScript.Instance.gridSize; x++)
        {
            for (int y = 0; y < SpawnScript.Instance.gridSize; y++)
            {
                gridGO[x, y].GetComponent<SpriteRenderer>().color = Color.grey;
            }
        }
        GridScript.Instance.FillGrid();
    }

    void PaintGridPreview()
    {
        Vector2 closestGridLoc = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        Vector2 destiny = closestGridLoc - (relativePos - parentBlock.rotPos[parentBlock.rotIndex]);
        BlockScript bs = parentBlock.GetComponent<BlockScript>();

        foreach (BlockTile bTile in parentBlock.tileList)
        {
            Color gridPreviewColor;
            Vector2 tilePos = destiny - (bs.rotPos[bs.rotIndex] - bTile.relativePos);
            if (tilePos.x >= 0 && tilePos.x < SpawnScript.Instance.gridSize && tilePos.y >= 0 && tilePos.y < SpawnScript.Instance.gridSize)
            {
                if (LogicManager.Instance.CheckPosition(parentBlock.gameObject, destiny))
                    gridPreviewColor = Color.green;
                else
                    gridPreviewColor = Color.red;

                gridGO[Mathf.RoundToInt(tilePos.x), Mathf.RoundToInt(tilePos.y)].GetComponent<SpriteRenderer>().color = gridPreviewColor;
            }
        }
    }
    #endregion
}
