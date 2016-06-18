using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    //C:/Users/lucap/AppData/LocalLow/DefaultCompany/TesteBloco

    public static List<Game> saved4maps;
    public static List<Game> saved5maps;
    public static List<Game> saved6maps;

    public static void SaveMap(int gridSize)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string mapsPath = System.IO.Path.Combine(Application.streamingAssetsPath, "saved" + gridSize + "maps.gd"); ;
        switch (gridSize)
        {
            case 4:
                if (File.Exists(mapsPath))//if the file exists deserialize
                {
                    BinaryFormatter bf2 = new BinaryFormatter();
                    FileStream file2 = File.Open(mapsPath, FileMode.Open);
                    saved4maps = (List<Game>)bf2.Deserialize(file2);
                    file2.Close();
                }
                else                                                //else make a new list to store the maps
                    saved4maps = new List<Game>();
                
                SaveLoad.saved4maps.Add(new Game());
                FileStream file4 = File.Create(mapsPath);
                bf.Serialize(file4, SaveLoad.saved4maps);
                file4.Close();
                Debug.Log("Novo total de mapas de tamanho " + gridSize + ": " + saved4maps.Count);
                break;

            case 5:
                if (File.Exists(mapsPath))//if the file exists deserialize
                {
                    BinaryFormatter bf2 = new BinaryFormatter();
                    FileStream file2 = File.Open(mapsPath, FileMode.Open);
                    saved4maps = (List<Game>)bf2.Deserialize(file2);
                    file2.Close();
                }
                else                                                //else make a new list to store the maps
                    saved5maps = new List<Game>();

                SaveLoad.saved5maps.Add(new Game());
                FileStream file5 = File.Create(mapsPath);
                bf.Serialize(file5, SaveLoad.saved4maps);
                file5.Close();
                Debug.Log("Novo total de mapas de tamanho " + gridSize + ": " + saved5maps.Count);
                break;

            case 6:
                if (File.Exists(mapsPath))//if the file exists deserialize
                {
                    BinaryFormatter bf2 = new BinaryFormatter();
                    FileStream file2 = File.Open(mapsPath, FileMode.Open);
                    saved4maps = (List<Game>)bf2.Deserialize(file2);
                    file2.Close();
                }
                else                                                //else make a new list to store the maps
                    saved6maps = new List<Game>();

                SaveLoad.saved6maps.Add(new Game());
                FileStream file6 = File.Create(mapsPath);
                bf.Serialize(file6, SaveLoad.saved4maps);
                file6.Close();
                Debug.Log("Novo total de mapas de tamanho " + gridSize + ": " + saved6maps.Count);
                break;
        }
    }

    public static void ChangeMap(int gridSize, int level)
    {
        BinaryFormatter bf = new BinaryFormatter();
        switch (gridSize)
        {
            case 4:
                if (File.Exists(Application.persistentDataPath + "/saved4maps.gd"))//if the file exists deserialize
                {
                    BinaryFormatter bf2 = new BinaryFormatter();
                    FileStream file2 = File.Open(Application.persistentDataPath + "/saved4maps.gd", FileMode.Open);
                    saved4maps = (List<Game>)bf2.Deserialize(file2);
                    file2.Close();
                }
                else                                                //else make a new list to store the maps
                    saved4maps = new List<Game>();

                SaveLoad.saved4maps[level] = new Game();
                FileStream file4 = File.Create(Application.persistentDataPath + "/saved4maps.gd");
                bf.Serialize(file4, SaveLoad.saved4maps);
                file4.Close();
                break;

            case 5:
                if (File.Exists(Application.persistentDataPath + "/saved5maps.gd"))//if the file exists deserialize
                {
                    BinaryFormatter bf2 = new BinaryFormatter();
                    FileStream file2 = File.Open(Application.persistentDataPath + "/saved5maps.gd", FileMode.Open);
                    saved5maps = (List<Game>)bf2.Deserialize(file2);
                    file2.Close();
                }
                else                                                //else make a new list to store the maps
                    saved5maps = new List<Game>();

                SaveLoad.saved5maps[level] = new Game();
                FileStream file5 = File.Create(Application.persistentDataPath + "/saved5maps.gd");
                bf.Serialize(file5, SaveLoad.saved5maps);
                file5.Close();
                break;

            case 6:
                if (File.Exists(Application.persistentDataPath + "/saved6maps.gd"))//if the file exists deserialize
                {
                    BinaryFormatter bf2 = new BinaryFormatter();
                    FileStream file2 = File.Open(Application.persistentDataPath + "/saved6maps.gd", FileMode.Open);
                    saved6maps = (List<Game>)bf2.Deserialize(file2);
                    file2.Close();
                }
                else                                                //else make a new list to store the maps
                    saved6maps = new List<Game>();

                SaveLoad.saved6maps[level] = new Game();
                FileStream file6 = File.Create(Application.persistentDataPath + "/saved6maps.gd");
                bf.Serialize(file6, SaveLoad.saved6maps);
                file6.Close();
                break;
        }
        Debug.Log("Mapa " + gridSize + "x" + level + " foi alterado!");
    }

    public static void LoadMaps(int gridSize)
    {
        // Put your file to "YOUR_UNITY_PROJ/Assets/StreamingAssets"
        // example: "YOUR_UNITY_PROJ/Assets/StreamingAssets/db.bytes"

        string mapsPath = "";

        if (Application.platform == RuntimePlatform.Android)
        {
            // Android
            string oriPath = System.IO.Path.Combine(Application.streamingAssetsPath, "saved" + gridSize + "maps.gd");

            // Android only use WWW to read file
            WWW reader = new WWW(oriPath);
            while (!reader.isDone) { }

            string realPath = Application.persistentDataPath + "/saved" + gridSize + "maps.gd";
            System.IO.File.WriteAllBytes(realPath, reader.bytes);

            mapsPath = realPath;
        }
        else
        {
            // iOS
            mapsPath = System.IO.Path.Combine(Application.streamingAssetsPath, "saved" + gridSize + "maps.gd");
        }


        if (File.Exists(mapsPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(mapsPath, FileMode.Open);
            switch (gridSize)
            {
                case 4: SaveLoad.saved4maps = (List<Game>)bf.Deserialize(file); break;
                case 5: SaveLoad.saved5maps = (List<Game>)bf.Deserialize(file); break;
                case 6: SaveLoad.saved6maps = (List<Game>)bf.Deserialize(file); break;
            }
            
            file.Close();
        }
        else
            Debug.Log("File not found");
    }
}