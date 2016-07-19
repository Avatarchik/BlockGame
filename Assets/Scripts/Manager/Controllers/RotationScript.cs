using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour
{
    public int spawnNumber;
    public GameObject block;

    public const string RotateBlock = "RotationScript.RotateBlock";

    void Start()
    {
        if (StateMachine.state == GameState.InGame || StateMachine.state == GameState.LevelGenerator)
            block = LogicManager.Instance.unplacedBlocks[spawnNumber];
    }

    void OnMouseDown()
    {
        if (block && !block.GetComponent<BlockScript>().bPlaced)
            this.PostNotification(RotateBlock, gameObject.GetComponentInParent<Transform>().gameObject);
    }
}
