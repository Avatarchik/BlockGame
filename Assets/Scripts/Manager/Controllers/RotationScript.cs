using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour
{
    public int spawnNumber;
    public GameObject block;
    public bool rotating;

    public const string RotateBlock = "RotationScript.RotateBlock";

    void Start()
    {
        if (StateMachine.state == GameState.InGame)
            block = LogicManager.Instance.unplacedBlocks[spawnNumber];
    }

    void OnMouseDown()
    {
        if (block && !block.GetComponent<BlockScript>().bPlaced && !rotating)
            this.PostNotification(RotateBlock, gameObject.GetComponentInParent<Transform>().gameObject);
    }
}
