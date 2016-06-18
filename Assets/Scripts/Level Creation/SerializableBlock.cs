using UnityEngine;
using System.Collections;

[System.Serializable]
public class SerializableBlock {

    public string blockName;
    public int tilesNumber;
    public int solvedIndex;

    public int solvedPosX;
    public int solvedPosY;

    public SerializableBlock(string _name, int _tiles, int _index,int _posX, int _posY)
    {
        blockName = _name;
        tilesNumber = _tiles;
        solvedIndex = _index;
        solvedPosX = _posX;
        solvedPosY = _posY;
    }
}
