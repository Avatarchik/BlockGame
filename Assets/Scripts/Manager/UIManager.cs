using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public GameObject pauseMenuCanvas; 

    public void GiveTip()
    {
        List<GameObject> notPlacedBlocks = new List<GameObject>();
        foreach (GameObject block in GameManager.Instance.activeBlocks)
            if (!block.GetComponent<BlockScript>().bPlaced)
                notPlacedBlocks.Add(block);

        if (notPlacedBlocks.Count == 0)
            return;

        GameObject randomBlock = notPlacedBlocks[Random.Range(0, notPlacedBlocks.Count)];
        BlockScript bs = randomBlock.GetComponent<BlockScript>();
        while (bs.rotIndex != bs.solutionIndex)
            bs.RotateBlock();

        Vector2 blockSolution = bs.solutionPos;
        LogicManager.Instance.PlaceBlock(randomBlock, blockSolution);
    }

    public void TogglePauseMenu()
    {
        if (!pauseMenuCanvas.activeSelf)
        {
            Time.timeScale = 0f;
            pauseMenuCanvas.SetActive(true);
            GameManager.Instance.gamePaused = true;
        }

        else
        {
            Time.timeScale = 1f;
            pauseMenuCanvas.SetActive(false);
            GameManager.Instance.gamePaused = false;
        }
    }

    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Time.timeScale = 1f;
        pauseMenuCanvas.SetActive(false);
        GameManager.Instance.gamePaused = false;
    }

    public void GoToLevelSelector()
    {
        SceneManager.LoadScene("Level Selector", LoadSceneMode.Single);
    }
}
