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

    public List<GameObject> spawnLocations;
    public List<GameObject>[] blocksList;
    public int activeBlocksNumber;
    public float blockScale = 0.6f;
    

    void Awake ()
    {
        instance = this;
        blocksList = new List<GameObject>[6];
        for (int a = 0; a < blocksList.Length; a++)
        {
            blocksList[a] = new List<GameObject>();
        }
        getBlocks();
        int numberOfBlocks =  9 /*GameObject.FindObjectsOfType<BlockScript>().Length*/;

        for (int n = 0; n < numberOfBlocks; n++)
        {
            GameObject spawn = Instantiate(Resources.Load("Prefabs/SpawnHolder"), Vector3.zero, Quaternion.identity) as GameObject;
            spawn.transform.SetParent(GameObject.Find("Spawn Locations").transform);
            spawn.name = "Spawn " + (n + 1);
            spawn.tag = "Spawn";
            spawn.transform.position = new Vector3(blockSize - 2f * (n % blockSize), gridSize + ((n / blockSize) * 2), 0);

            BoxCollider2D bc2d = spawn.GetComponent<BoxCollider2D>();          
            bc2d.offset = new Vector2((blockScale / 2) * (blockSize - 1), (blockScale / 2) * (blockSize - 1));
            bc2d.size = new Vector2(blockScale * blockSize, blockScale * blockSize);

            spawn.GetComponent<RotationScript>().bNumber = n;
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
}
