using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour {

	
	void Start ()
    {
        SaveLoad.LoadProgress();
        foreach (LevelSelectorScript button in GameObject.FindObjectsOfType<LevelSelectorScript>())
        {
            if (SaveLoad.mapsCompleted[button.gridSize, button.levelToLoad] == true)
            {
                button.GetComponentInChildren<Text>().text += "*";
            }
        }
        
    }
}
