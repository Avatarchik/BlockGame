using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockScript : MonoBehaviour
{
    [SerializeField]
    int[] bLocations;
    int maxBlockSize = 3;

    public Color bColor;
    public int[,] block;
    public List<SquareScript> sList;
    public int bNumber;  
    public bool bPlaced;
    public Vector2 bPos;

    void Start()
    {
        sList = new List<SquareScript>();;
        block = new int[maxBlockSize, maxBlockSize];
        bNumber = SpawnScript.Instance.blocksList.Count + 1;
        CreateBlock();                    
    }

    void CreateBlock()
    {
        foreach (int loc in bLocations)
        {
            switch (loc)
            {
                case 1: block[0, 0] = bNumber; break;
                case 2: block[1, 0] = bNumber; break;
                case 3: block[2, 0] = bNumber; break;
                case 4: block[0, 1] = bNumber; break;
                case 5: block[1, 1] = bNumber; break;
                case 6: block[2, 1] = bNumber; break;
                case 7: block[0, 2] = bNumber; break;
                case 8: block[1, 2] = bNumber; break;
                case 9: block[2, 2] = bNumber; break;
                default:
                    Debug.Log("Número inválido para criação de bloco" + loc);
                    break;
            }
        }
        CreateBlockSprite();
    }

    public void CreateBlockSprite()
    {
        for (int x = 0; x < block.GetLength(0); x++)
        {
            for (int y = 0; y < block.GetLength(1); y++)
            {
                if (block[x, y] == bNumber)
                {              
                    GameObject baseSquare = Instantiate(Resources.Load("Prefabs/Base Square"), Vector3.zero, Quaternion.identity) as GameObject;
                    baseSquare.transform.localPosition = new Vector3(x, y, 0);
                    baseSquare.GetComponent<SpriteRenderer>().color = bColor;
                    baseSquare.GetComponent<SpriteRenderer>().sortingLayerName = "blocks";
                    baseSquare.transform.parent = transform;

                    SquareScript sScript = baseSquare.GetComponent<SquareScript>();
                    sScript.relativePos = new Vector2(x, y);
                    sScript.sType = SquareType.Block;
                    sScript.parentBlock = this;
                    sScript.bNumber = bNumber;
                    baseSquare.name = this.gameObject.name + sScript.relativePos.ToString();
                    sList.Add(sScript);
                }
            }
        }
        transform.position = SpawnScript.Instance.spawnLocations[bNumber - 1].transform.position;
        this.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
        SpawnScript.Instance.blocksList.Add(this.gameObject);        
    }
}