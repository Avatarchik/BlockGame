using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour
{
    public int bNumber;
    float clickTime;
    float holdTime = 0.75f;


    void OnMouseDown()
    {
        clickTime = Time.time;
    }

    void OnMouseDrag()
    {
        if ((Time.time - clickTime > holdTime))
        {
            SpawnScript.Instance.activeBlocksList[bNumber].GetComponent<BlockScript>().RotateMatrix(true);
            clickTime = Time.time;
        }
    }
}
