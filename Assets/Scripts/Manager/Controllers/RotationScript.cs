using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour
{
    public int spawnNumber;
    public GameObject block;

    public const string RotateBlock = "RotationScript.RotateBlock";

    void Start()
    {
        if (!GameObject.FindObjectOfType<LevelGeneratorScript>())
            block = LogicManager.Instance.unplacedBlocks[spawnNumber];
    }

    void OnMouseDown()
    {
        if (block)
            this.PostNotification(RotateBlock, gameObject.GetComponentInParent<Transform>().gameObject);
    }
}
