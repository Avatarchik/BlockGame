using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum GridType { Empty, Used, Filled }

public class GridTile : Tile
{
    public GridType gType;


    void OnMouseDown()
    {
        switch (gType)
        {
            case GridType.Empty:
                gType = GridType.Filled;
                GridScript.Instance.filledListPos.Add(gridPos);
                GetComponent<SpriteRenderer>().color = Color.clear;
                LogicManager.Instance.totalTiles--;
                LogicManager.Instance.tilesLeft--;
                break;
            case GridType.Filled:
                gType = GridType.Empty;
                GridScript.Instance.filledListPos.Remove(gridPos);
                GetComponent<SpriteRenderer>().color = Color.grey;
                LogicManager.Instance.totalTiles++;
                LogicManager.Instance.tilesLeft++;
                break;
        }

        GameObject.Find("Tiles Text").GetComponent<Text>().text = LogicManager.Instance.tilesLeft + "/" + LogicManager.Instance.totalTiles;
    }
}
