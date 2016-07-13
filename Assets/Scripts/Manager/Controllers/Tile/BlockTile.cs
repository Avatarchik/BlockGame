using UnityEngine;
using System.Collections;

public class BlockTile : Tile
{
    public Vector2 relativePos;

    Vector3 offset;
    GameObject[,] gridGO { get { return GridScript.Instance.gridGO; } set { GridScript.Instance.gridGO = value; } }

    void OnMouseDown()
    {
        if (!GameManager.Instance.gamePaused)
        {
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            parentBlock.transform.localScale = Vector3.one * 0.9f;
            foreach (BlockTile bTile in this.parentBlock.tileList)
                bTile.GetComponent<SpriteRenderer>().sortingOrder = 2;

            if (parentBlock.bPlaced)
                this.PostNotification(LogicManager.BlockRemovedNotification, parentBlock.gameObject);

            RemoveBlockGrid(parentBlock.gameObject);
            LogicManager.Instance.RearrangeBlocks(parentBlock.gameObject);
        }
    }

    void OnMouseDrag()
    {
        if (!GameManager.Instance.gamePaused && !LogicManager.Instance.rotatingBlock)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            //Traz o bloco para frente na camera
            float blockScale = parentBlock.transform.localScale.x;
            parentBlock.transform.position = new Vector3(curPosition.x - (transform.localPosition.x * blockScale), curPosition.y - (transform.localPosition.y * blockScale), -1f);

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
                LogicManager.Instance.PlaceBlock(parentBlock.gameObject, destiny);

            else
            {
                LogicManager.Instance.RespawnBlock(parentBlock.gameObject);
            }
        }
        ClearGridColor();
        foreach (BlockTile bTile in parentBlock.tileList)
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
        ClearGridColor();
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
                    gridPreviewColor = new Color(0, 1, 0, 0.7f);
                else
                    gridPreviewColor = new Color(1, 0, 0, 0.7f);

                gridGO[Mathf.RoundToInt(tilePos.x), Mathf.RoundToInt(tilePos.y)].GetComponent<SpriteRenderer>().color = gridPreviewColor;
            }
        }
        GridScript.Instance.FillGrid();
    }

    void RemoveBlockGrid(GameObject block)
    {
        BlockScript bs = block.GetComponent<BlockScript>();
        for (int x = 0; x < GameManager.Instance.gridSize; x++)
        {
            for (int y = 0; y < GameManager.Instance.gridSize; y++)
            {
                if (GridScript.Instance.gridGO[x, y].GetComponent<GridTile>().bNumber == bs.bNumber)
                {
                    GridTile gTile = GridScript.Instance.gridGO[x, y].GetComponent<GridTile>();
                    gTile.gType = GridType.Empty;
                    gTile.parentBlock = null;
                    gTile.bNumber = -1;
                }
            }
        }
        bs.bPlaced = false;
    }
    #endregion
}
