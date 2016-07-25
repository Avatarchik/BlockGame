using UnityEngine;
using System.Collections;

public abstract class Tile : MonoBehaviour {

    public BlockScript parentBlock;
    public int bNumber;
    public Vector2 gridPos;
}
