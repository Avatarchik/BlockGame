using UnityEngine;

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
    public int gridSize;
    public Color gColor;
    public int maxBlockSize = 3;

    void Start()
    {
        gridGO = new GameObject[gridSize, gridSize];
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
