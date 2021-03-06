﻿using UnityEngine;
using System.Collections.Generic;

public class SpawnScript : MonoBehaviour {

    #region Singleton Pattern
    private static SpawnScript instance = null;

    public static SpawnScript Instance
    {
        get { return instance; }
    }
    #endregion

    public int gridSize;
    public int blockSize = 3;

    int spawnColumns;
    float spawnSpacingX;
    float spawnSpacingY;
    Vector3 spawnPos;

    public List<GameObject> spawnLocations;
    public float blockScale = 0.6f;
    

    void Awake ()
    {
        instance = this;

        gridSize = GameManager.Instance.gridSize;
        FixCamera();
        
        int numberOfSpawns = 15;

        for (int n = 0; n < numberOfSpawns; n++)
        {
            GameObject spawn = Instantiate(Resources.Load("Prefabs/SpawnHolder"), Vector3.zero, Quaternion.identity) as GameObject;
            spawn.transform.SetParent(GameObject.Find("Spawn Locations").transform);
            spawn.name = "Spawn " + n;
            spawn.tag = "Spawn";
            Vector3 pos = new Vector3(-spawnSpacingX * (n % spawnColumns), ((n / spawnColumns) * spawnSpacingY), 0);
            spawn.transform.position = pos + spawnPos;

            Transform background = spawn.transform.Find("Spawn Background");
            background.localScale = Vector3.one * blockScale * blockSize;
            background.localPosition = Vector3.one * blockScale;

            Transform rotButton = spawn.transform.Find("Rotation Button");
            rotButton.localScale = Vector3.one * blockScale * 0.8f;
            rotButton.localPosition = new Vector3(2.5f * blockScale, -rotButton.localScale.x / 2, -2);
            rotButton.GetComponent<RotationScript>().spawnNumber = n;

            spawnLocations.Add(spawn);
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

    public void DeleteSpawns()
    {
        for (int n = GameManager.Instance.activeBlocks.Count; n < spawnLocations.Count; n++)
        {
            Destroy(spawnLocations[n]);
        }
    }
}
