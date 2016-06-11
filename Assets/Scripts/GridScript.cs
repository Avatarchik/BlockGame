using UnityEngine;
using System.Collections.Generic;

public class GridScript : MonoBehaviour {

    #region Singleton Pattern
    private static GridScript instance = null;

    public static GridScript Instance
    {
        get { return instance; }
    }

    void Awake() { instance = this; }
    #endregion

    public GameObject[,] gridGO;
    public Color gColor;
    public Color filledColor;
    public Color[] blocksColor = new Color[10];
    public List<Vector2> filledListPos = new List<Vector2>();
    public bool paused;

    int gridSize;

    void Start()
    {
        gridSize = SpawnScript.Instance.gridSize;
        gridGO = new GameObject[gridSize + 2, gridSize + 2];
        gColor = Color.grey;

        //Create the grid
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

        foreach (Vector2 filledPos in filledListPos)
        {
            FillGrid(filledPos);
        }
    }

    public void FillGrid(Vector2 location)
    {
        int x = (int)location.x;
        int y = (int)location.y;
        gridGO[x, y].GetComponent<SpriteRenderer>().color = filledColor;
        gridGO[x, y].GetComponent<GridTile>().gType = GridType.Filled;
    }

    public bool CheckWin()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (gridGO[x, y].GetComponent<GridTile>().bNumber == 0)
                    return false;
            }
        }

        return true;
    }

    public void WinEvent()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level Selector");
        
    }
}
