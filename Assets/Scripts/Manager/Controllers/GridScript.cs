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
        gridSize = StateMachine.currentGridSize;
        gridGO = new GameObject[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                gridGO[x, y] = Instantiate(Resources.Load("Prefabs/Base Grid Square"), new Vector3(x, y, 1f), Quaternion.identity) as GameObject;
                gridGO[x, y].transform.parent = GameObject.Find("Grid").transform;
                gridGO[x, y].name = ("grid pos " + x + "," + y);
                gridGO[x, y].GetComponent<GridTile>().gridPos = new Vector2(x, y);
                if (StateMachine.state == GameState.LevelCreator)
                    gridGO[x, y].GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    public void FillGrid()
    {
        foreach (Vector2 filledPos in filledListPos)
        {
            int x = (int)filledPos.x;
            int y = (int)filledPos.y;
            gridGO[x, y].GetComponent<SpriteRenderer>().color = Color.clear;
            gridGO[x, y].GetComponent<GridTile>().gType = GridType.Filled;
        }      
    }

}
