using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour
{
    public int bNumber;
    public GameObject block;
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
            block.GetComponent<BlockScript>().RotateMatrix(true);
            block.transform.position = SpawnScript.Instance.spawnLocations[bNumber - 1].transform.position - Vector3.forward;
            clickTime = Time.time;
        }
    }
}
