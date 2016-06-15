using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour
{
    public int bNumber;
    public GameObject parentBlock;
    public float rotationSpeed = 3;

    float clickTime;
    float holdTime = 0.5f;
    bool rotating;

    public const string RotateBlock = "BlockTile.RotateBlock";

    void Start()
    {
        if (parentBlock)
            bNumber = parentBlock.GetComponent<BlockScript>().bNumber;
    }

    void OnMouseDown()
    {
        if (!GridScript.Instance.paused)
            clickTime = Time.time;
    }

    void OnMouseDrag()
    {
        if (!GridScript.Instance.paused)
        {
            if (Time.time - clickTime > holdTime)
            {
                this.PostNotification(RotateBlock, parentBlock);
                clickTime = Time.time;
            }
        }

    }
}
