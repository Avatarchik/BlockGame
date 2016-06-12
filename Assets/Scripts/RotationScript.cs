using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour
{
    int bNumber;
    public GameObject block;
    //float clickTime;
    //float holdTime = 0.5f;


    void OnMouseDown()
    {
        //if (!GridScript.Instance.paused)
        //    clickTime = Time.time;

        block.GetComponent<BlockScript>().RotateBlock();
    }

    //void OnMouseDrag()
    //{
    //    if (!GridScript.Instance.paused)
    //    {
    //        if ((Time.time - clickTime > holdTime))
    //        {
    //            block.GetComponent<BlockScript>().RotateBlock(true);
    //            block.transform.position = SpawnScript.Instance.spawnLocations[bNumber].transform.position - Vector3.forward;
    //            clickTime = Time.time;
    //        }
    //    }

    //}
}
