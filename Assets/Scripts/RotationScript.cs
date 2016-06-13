using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour
{
    public int bNumber;
    public GameObject block;
    float clickTime;
    float holdTime = 0.5f;

    void Start()
    {
        if (block)
            bNumber = block.GetComponent<BlockScript>().bNumber;
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
            if ((Time.time - clickTime > holdTime))
            {
                block.GetComponent<BlockScript>().RotateBlock();
                block.transform.position = SpawnScript.Instance.spawnLocations[bNumber].transform.position - Vector3.forward;
                clickTime = Time.time;
            }
        }

    }
}
