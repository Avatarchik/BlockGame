using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    public static Game saveGame;

    public static void Save()
    {
        saveGame= GameObject.FindObjectOfType<Game>();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGame.gd");
        bf.Serialize(file, saveGame);
        file.Close();
    }
}