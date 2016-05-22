using UnityEngine;
using System.Collections.Generic;

public class SpawnScript : MonoBehaviour {

    #region Singleton Pattern
    private static SpawnScript instance = null;

    public static SpawnScript Instance
    {
        get { return instance; }
    }
    #endregion

    public int blockSize = 3;
    public int gridSize = 5;

    int spawnColumns;
    float spawnSpacingX;
    float spawnSpacingY;
    Vector3 spawnPos;

    public List<GameObject> spawnLocations;
    public List<GameObject>[] blocksList;
    public int activeBlocksNumber;
    public float blockScale = 0.6f;
    

    void Awake ()
    {
        instance = this;
        FixCamera();
        blocksList = new List<GameObject>[6];
        for (int a = 0; a < blocksList.Length; a++)
        {
            blocksList[a] = new List<GameObject>();
        }
        getBlocks();
        int numberOfBlocks = 2 * gridSize;

        for (int n = 0; n < numberOfBlocks; n++)
        {
            GameObject spawn = Instantiate(Resources.Load("Prefabs/SpawnHolder"), Vector3.zero, Quaternion.identity) as GameObject;
            spawn.transform.SetParent(GameObject.Find("Spawn Locations").transform);
            spawn.name = "Spawn " + (n + 1);
            spawn.tag = "Spawn";
            Vector3 pos = new Vector3(-spawnSpacingX * (n % spawnColumns), ((n / spawnColumns) * spawnSpacingY), 0);
            spawn.transform.position = pos + spawnPos;

            BoxCollider2D bc2d = spawn.GetComponent<BoxCollider2D>();          
            bc2d.offset = new Vector2((blockScale / 2) * (blockSize - 1), (blockScale / 2) * (blockSize - 1));
            bc2d.size = new Vector2(blockScale * blockSize, blockScale * blockSize);

            spawn.GetComponent<RotationScript>().bNumber = n + 1;
            spawnLocations.Add(spawn);
        }       
	}

    void getBlocks()
    {
        for (int i = 2; i < 6; i++)
        {
            Object[] blockFormats = (Object[])Resources.LoadAll(string.Format("Prefabs/Blocks/Block {0} squares", i));
            foreach(GameObject block in blockFormats)
            {
                blocksList[i].Add(block as GameObject);
            }
        }
    }

    void FixCamera()
    {
        Camera cam = GameObject.FindObjectOfType<Camera>();

        switch (gridSize)
        {
            case 4:
                cam.transform.position = new Vector3(1.5f, 3.5f, -10);
                cam.orthographicSize = 5f;

                spawnColumns = 3;
                spawnSpacingX = 1.75f;
                spawnSpacingY = 1.75f;
                spawnPos = new Vector3(2.75f, 4f);
                blockScale = 0.5f;
                break;
            case 5:
                cam.transform.position = new Vector3(2f, 5f, -10);
                cam.orthographicSize = 6.5f;

                spawnColumns = 3;
                spawnSpacingX = 2f;
                spawnSpacingY = 2f;
                spawnPos = new Vector3(3.5f, 5f);
                blockScale = 0.55f;
                break;
            case 6:
                cam.transform.position = new Vector3(2.5f, 5f, -10);
                cam.orthographicSize = 6.5f;

                spawnColumns = 4;
                spawnSpacingX = 1.75f;
                spawnSpacingY = 1.75f;
                spawnPos = new Vector3(4.6f, 6f);
                blockScale = 0.5f;
                break;
        }
    }
}
