using UnityEngine;
using System.Collections.Generic;

public class SpawnScript : MonoBehaviour
{
    #region Singleton Pattern
    private static SpawnScript instance = null;

    public static SpawnScript Instance
    {
        get { return instance; }
    }
    #endregion

    public int gridSize;
    public float blockScale;
    public int spawnColumns = 3;
    public List<GameObject> spawnLocations;
    
    float spawnSize;
    Vector2 spacing;
    Vector3 baseSpawn;

    void Awake()
    {
        instance = this;
        gridSize = StateMachine.currentGridSize;

        Camera cam = FindObjectOfType<Camera>();
        cam.transform.position = new Vector3(0.5f * (gridSize - 1), (gridSize - 0.5f), -10);
        cam.orthographicSize = gridSize;

        //Generate Spawns
        for (int n = 0; n < 15; n++)
        {
            GameObject spawn = Instantiate(Resources.Load("Prefabs/SpawnHolder"), Vector3.zero, Quaternion.identity) as GameObject;
            spawn.transform.SetParent(GameObject.Find("Spawn Locations").transform);
            spawn.name = "Spawn " + n;

            Transform background = spawn.transform.Find("Spawn Background");
            background.localScale = Vector3.one * blockScale * 3;
            background.localPosition = Vector3.one * blockScale;

            Transform rotButton = spawn.transform.Find("Rotation Button");
            rotButton.localScale = Vector3.one * blockScale * 0.8f;
            rotButton.localPosition = new Vector3(2.5f * blockScale, -rotButton.localScale.x / 2, -1.5f);
            rotButton.GetComponent<RotationScript>().spawnNumber = n;

            spawnLocations.Add(spawn);
        }
    }

    public void FixSpawnsPosition()
    {
        blockScale = gridSize / (4 * spawnColumns - 1f);
        spawnSize = 3 * blockScale;
        spacing = Vector2.one * 0.8f * blockScale;
        baseSpawn = new Vector3((spawnColumns - 1) * (spawnSize + spacing.x) + 0.5f * blockScale - 0.5f, GridScript.Instance.gridGO[0,gridSize - 1].transform.position.y + 1, 0);

        for (int n = 0; n < GameManager.Instance.activeBlocks.Count; n++)
        {
            Vector3 pos = new Vector3(-(n % spawnColumns) * (spacing.x + spawnSize), ((n / spawnColumns) * (spacing.y + spawnSize)), 0);
            spawnLocations[n].transform.position = pos + baseSpawn;

            Transform background = spawnLocations[n].transform.Find("Spawn Background");
            background.localScale = Vector3.one * blockScale * 3;
            background.localPosition = Vector3.one * blockScale;

            Transform rotButton = spawnLocations[n].transform.Find("Rotation Button");
            rotButton.localScale = Vector3.one * blockScale * 0.8f;
            rotButton.localPosition = new Vector3(2.5f * blockScale, -rotButton.localScale.x / 2, -1.5f);
        }

        foreach (GameObject block in LogicManager.Instance.unplacedBlocks)
            block.transform.localScale = Vector3.one * blockScale;

        if (StateMachine.state == GameState.InGame)
        {
            for (int i = 0; i < LogicManager.Instance.unplacedBlocks.Count; i++)
            {
                LogicManager.Instance.unplacedBlocks[i].transform.position =  spawnLocations[i].transform.position;
                LogicManager.Instance.unplacedBlocks[i].GetComponent<BlockScript>().spawnNumber = i;
                spawnLocations[i].GetComponentInChildren<RotationScript>().block = LogicManager.Instance.unplacedBlocks[i];
            }
        }
    }

    public void DeleteExtraSpawns()
    {
        for (int n = spawnLocations.Count - 1; n >= GameManager.Instance.activeBlocks.Count; n--)
        {
            Destroy(spawnLocations[n]);
            spawnLocations.RemoveAt(n);
        }
    }
}
