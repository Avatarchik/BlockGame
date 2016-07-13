using UnityEngine;
using System.Collections.Generic;

public class GridScript : MonoBehaviour {

    #region Singleton Pattern
    private static GridScript instance = null;

    public static GridScript Instance
    {
        get { return instance; }
    }
    #endregion

    public GameObject[,] gridGO;

    public Color filledColor;
    public Color[] blocksColor = new Color[10];
    public List<Vector2> filledListPos = new List<Vector2>();

    int gridSize;

    void Awake()
    {
        instance = this;
        CreateGrid();
    }

    void CreateGrid()
    {
        gridSize = GameManager.Instance.gridSize;
        gridGO = new GameObject[gridSize + 2, gridSize + 2];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                gridGO[x, y] = Instantiate(Resources.Load("Prefabs/Base Grid Square"), new Vector3(x, y, 1f), Quaternion.identity) as GameObject;
                gridGO[x, y].transform.parent = GameObject.Find("Grid").transform;
                gridGO[x, y].name = ("grid pos " + x + "," + y);
                gridGO[x, y].GetComponent<GridTile>().gridPos = new Vector2(x, y);
            }
        }

        FillGrid();
    }

    public void FillGrid()
    {
        foreach (Vector2 filledPos in filledListPos)
        {
            int x = (int)filledPos.x;
            int y = (int)filledPos.y;
            gridGO[x, y].GetComponent<SpriteRenderer>().color = filledColor;
            gridGO[x, y].GetComponent<GridTile>().gType = GridType.Filled;
        }      
    }

}
