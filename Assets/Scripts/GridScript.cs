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
    public List<Vector2> filledListPos = new List<Vector2>();

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
                gridGO[x, y] = Instantiate(Resources.Load("Prefabs/Base Square"), new Vector3(x, y, 1f), Quaternion.identity) as GameObject;
                gridGO[x, y].GetComponent<SpriteRenderer>().sortingLayerName = "grid";
                gridGO[x, y].GetComponent<SpriteRenderer>().color = gColor;
                gridGO[x, y].transform.parent = GameObject.Find("Grid").transform;
                gridGO[x, y].name = ("grid pos " + x + "," + y);
                SquareScript baseSquareSS = gridGO[x, y].GetComponent<SquareScript>();
                baseSquareSS.sType = SquareType.GridEmpty;
                baseSquareSS.squareGridPos = new Vector2(x, y);
                baseSquareSS.parentBlock = null;
                baseSquareSS.bNumber = 0;
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
        gridGO[x, y].GetComponent<SquareScript>().bNumber = -1;
        gridGO[x, y].GetComponent<SquareScript>().sType = SquareType.GridFilled;
    }

    public bool CheckWin()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (gridGO[x, y].GetComponent<SquareScript>().bNumber == 0)
                    return false;
            }
        }

        return true;
    }

    public void WinEvent()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("scene 2");
        
    }
}
