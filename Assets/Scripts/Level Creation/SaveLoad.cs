using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    public static List<Game>[] savedMaps;
    public static bool[,] mapsCompleted;

    #region Maps
    public static void SaveMap()
    {
        string mapsPath = Path.Combine(Application.streamingAssetsPath, "savedMaps.gd");

        if (File.Exists(mapsPath))                          //if the file exists deserialize
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(mapsPath, FileMode.Open);
            savedMaps = (List<Game>[])bf.Deserialize(file);
            file.Close();
        }
        else
        {
            savedMaps = new List<Game>[10];
            for (int i = 0; i < 10; i++)
                savedMaps[i] = new List<Game>();
        }

        Game currentGame = new Game();
        int gridSize = currentGame.gridSize;
        savedMaps[gridSize].Add(currentGame);

        BinaryFormatter bf2 = new BinaryFormatter();
        FileStream file2 = File.Create(mapsPath);
        bf2.Serialize(file2, savedMaps);
        file2.Close();
        Debug.LogWarning("Total de mapas de tamanho " + gridSize + ": " + savedMaps[gridSize].Count);       
    }

    public static void ChangeMap(int level)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string mapsPath = Path.Combine(Application.streamingAssetsPath, "savedMaps.gd"); ;
        if (File.Exists(mapsPath))//if the file exists deserialize
        {
            BinaryFormatter bf2 = new BinaryFormatter();
            FileStream file2 = File.Open(mapsPath, FileMode.Open);
            savedMaps = (List<Game>[])bf2.Deserialize(file2);
            file2.Close();
        }
        else
        {
            Debug.LogWarning("Não existe o save!");
            return;
        }

        Game currentGame = new Game();
        int gridSize = currentGame.gridSize;
        savedMaps[gridSize][level] = currentGame;

        FileStream file = File.Create(mapsPath);
        bf.Serialize(file, savedMaps);
        file.Close();

        Debug.LogWarning("Mapa " + gridSize + "x" + level + " foi alterado!");
    }

    public static void LoadMaps()
    {
        // Put your file to "YOUR_UNITY_PROJ/Assets/StreamingAssets"
        // example: "YOUR_UNITY_PROJ/Assets/StreamingAssets/db.bytes"

        string mapsPath = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            // Android
            string oriPath = System.IO.Path.Combine(Application.streamingAssetsPath, "savedMaps.gd");

            // Android only use WWW to read file
            WWW reader = new WWW(oriPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + "/savedMaps.gd";
            System.IO.File.WriteAllBytes(realPath, reader.bytes);

            mapsPath = realPath;
        }
        else
        {
            // iOS
            mapsPath = System.IO.Path.Combine(Application.streamingAssetsPath, "savedMaps.gd");
        }


        if (File.Exists(mapsPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(mapsPath, FileMode.Open);
            savedMaps = (List<Game>[])bf.Deserialize(file);
            file.Close();
        }
        else
            Debug.LogWarning("File não encontrado");
    }
    #endregion

    #region Player Progression
    public static void SaveProgress()
    {
        string mapsCompletedPath = Path.Combine(Application.streamingAssetsPath, "mapsCompleted.gd");

        //Open file
        if (File.Exists(mapsCompletedPath))                         
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(mapsCompletedPath, FileMode.Open);
            mapsCompleted = (bool[,])bf.Deserialize(file);
            file.Close();
        }
        else
            mapsCompleted = new bool[10, 100];
  
        int gridSize = GameManager.Instance.gridSize;
        int level = GameManager.Instance.level;
        mapsCompleted[gridSize, level] = true;

        BinaryFormatter bf2 = new BinaryFormatter();
        FileStream file2 = File.Create(mapsCompletedPath);
        bf2.Serialize(file2, mapsCompleted);
        file2.Close();
        Debug.LogWarning("Progresso no mapa " + gridSize + "x" + level + " foi salvo!");
    }

    public static void LoadProgress()
    {
        string mapsCompletedPath = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            // Android
            string oriPath = System.IO.Path.Combine(Application.streamingAssetsPath, "mapsCompleted.gd");

            // Android only use WWW to read file
            WWW reader = new WWW(oriPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + "/mapsCompleted.gd";
            System.IO.File.WriteAllBytes(realPath, reader.bytes);

            mapsCompletedPath = realPath;
        }
        else
        {
            // iOS
            mapsCompletedPath = System.IO.Path.Combine(Application.streamingAssetsPath, "mapsCompleted.gd");
        }


        if (File.Exists(mapsCompletedPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(mapsCompletedPath, FileMode.Open);
            mapsCompleted = (bool[,])bf.Deserialize(file);
            file.Close();
        }
        else
        {
            mapsCompleted = new bool[10, 100];
            BinaryFormatter bf2 = new BinaryFormatter();
            FileStream file2 = File.Create(mapsCompletedPath);
            bf2.Serialize(file2, mapsCompleted);
            file2.Close();
            Debug.LogWarning("Save criado!");
        }    
    }
    #endregion
}