using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour
{
    public int bNumber;
    public GameObject block;
    float clickTime;
    float holdTime = 0.5f;


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
                block.GetComponent<BlockScript>().RotateMatrix(true);
                block.transform.position = SpawnScript.Instance.spawnLocations[bNumber - 1].transform.position - Vector3.forward;
                clickTime = Time.time;
            }
        }

    }
}
