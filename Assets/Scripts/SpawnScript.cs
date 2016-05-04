using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnScript : MonoBehaviour {

    #region Singleton Pattern
    private static SpawnScript instance = null;

    public static SpawnScript Instance
    {
        get { return instance; }
    }
    #endregion

    public List<GameObject> spawnLocations;
    public List<GameObject> blocksList;
    public float blockScale = 0.6f;
    public int blockSize = 3;

    void Awake () {
        instance = this;
        int numberOfBlocks = GameObject.FindObjectsOfType<BlockScript>().Length;

        for (int n = 0; n < numberOfBlocks; n++)
        {
            GameObject spawn = Instantiate(Resources.Load("Prefabs/SpawnHolder"), Vector3.zero, Quaternion.identity) as GameObject; ;
            spawn.transform.SetParent(GameObject.Find("Spawn Locations").transform);
            spawn.name = "Spawn " + (n + 1);
            spawn.tag = "Spawn";
            spawn.transform.position = new Vector3(3 - 2f * (n % 3), 5 + ((n / 3) * 2), 0.5f);

            BoxCollider2D bc2d = spawn.GetComponent<BoxCollider2D>();          
            bc2d.offset = new Vector2(blockScale, blockScale);
            bc2d.size = new Vector2(blockScale * blockSize, blockScale * blockSize);

            spawn.GetComponent<RotationScript>().bNumber = n;
            spawnLocations.Add(spawn);
        }
        
	}
}
