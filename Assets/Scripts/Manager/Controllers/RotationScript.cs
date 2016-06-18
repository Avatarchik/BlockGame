using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour
{
    public int bNumber;
    public GameObject parentBlock;

    float clickTime;
    float holdTime = 0.5f;
    bool rotating;

    public const string RotateBlock = "RotationScript.RotateBlock";

    void Start()
    {
        if (parentBlock)
            bNumber = parentBlock.GetComponent<BlockScript>().bNumber;
    }

    void OnMouseDown()
    {
        if (!GameManager.Instance.gamePaused)
            clickTime = Time.time;
    }

    void OnMouseDrag()
    {
        if (!GameManager.Instance.gamePaused)
        {
            if (Time.time - clickTime > holdTime)
            {
                this.PostNotification(RotateBlock, parentBlock);
                clickTime = Time.time;
            }
        }

    }
}
