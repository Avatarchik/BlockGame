using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour
{
    public int bNumber;
    public GameObject parentBlock;

    public const string RotateBlock = "RotationScript.RotateBlock";

    void Start()
    {
        parentBlock = GameManager.Instance.activeBlocks[bNumber];
    }

    void OnMouseDown()
    {
        if (!GameManager.Instance.gamePaused && !LogicManager.Instance.rotatingBlock && !parentBlock.GetComponent<BlockScript>().bPlaced)
            this.PostNotification(RotateBlock, parentBlock);
    }
}
