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

    void Awake () {
        instance = this;
        int numberOfBlocks = GameObject.FindObjectsOfType<BlockScript>().Length;
        for (int n = 0; n < numberOfBlocks; n++)
        {
            GameObject spawn = new GameObject();
            //spawn.transform.SetParent(GameObject.Find("Spawn Locations").transform);
            spawn.transform.position = new Vector3(2f * (n % 3) - 1, 10 - ((n / 3) * 2), 0);
            spawnLocations.Add(spawn);
        }
        
	}
}
