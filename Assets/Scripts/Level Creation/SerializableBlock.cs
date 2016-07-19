using UnityEngine;
using System.Collections;

[System.Serializable]
public class SerializableBlock {

    public string blockName;
    public int tilesNumber;
    public int solvedIndex;

    public int solvedPosX;
    public int solvedPosY;

    public SerializableBlock(string _name, int _tiles, int _solvedIndex,int _solvedPosX, int _solvedPosY)
    {
        blockName = _name;
        tilesNumber = _tiles;
        solvedIndex = _solvedIndex;
        solvedPosX = _solvedPosX;
        solvedPosY = _solvedPosY;
    }
}
