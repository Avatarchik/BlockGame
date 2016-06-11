using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameObject pauseMenuCanvas; 
	
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TogglePauseMenu()
    {
        if (!pauseMenuCanvas.activeSelf)
        {
            Time.timeScale = 0f;
            pauseMenuCanvas.SetActive(true);
            GridScript.Instance.paused = true;
        }

        else
        {
            Time.timeScale = 1f;
            pauseMenuCanvas.SetActive(false);
            GridScript.Instance.paused = false;
        }
    }

    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void GoToLevelSelector()
    {
        SceneManager.LoadScene("Level Selector");
    }
}
