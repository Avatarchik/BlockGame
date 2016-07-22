using UnityEngine;
using System.Collections;

public class BlockPicker : MonoBehaviour {

    [SerializeField]
    int blockSize;

    public void BlockSizeButton()
    {
        GameObject.Find("BlockSize Panel").SetActive(false);
        GameObject.Find("Block Panel").SetActive(true);
    }

    void CreateBlocks(int bSize)
    {

    }
}