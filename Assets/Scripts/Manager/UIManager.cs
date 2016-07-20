using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    #region Singleton Pattern
    private static UIManager instance = null;

    public static UIManager Instance
    {
        get { return instance; }
    }

    void Awake() { instance = this; }
    #endregion

    public GameObject pauseMenuCanvas;
    public GameObject levelCompletedCanvas;
    public Button nextLevelButton;

    public static string PauseNotification = "UIManager.Pause";

    void Start()
    {
        SaveLoad.LoadMaps();

        if (StateMachine.currentLevel + 1 >= SaveLoad.savedMaps[StateMachine.currentGridSize].Count)
            nextLevelButton.interactable = false;

        LogicManager.Instance.RearrangeBlocks();
    }

    public void GiveTip()
    {
        List<GameObject> notPlacedBlocks = new List<GameObject>();
        foreach (GameObject block in GameManager.Instance.activeBlocks)
            if (!block.GetComponent<BlockScript>().bPlaced && !block.GetComponent<BlockScript>().bTip)
                notPlacedBlocks.Add(block);

        if (notPlacedBlocks.Count == 0)
            return;

        GameObject randomBlock = notPlacedBlocks[Random.Range(0, notPlacedBlocks.Count)];
        BlockScript bs = randomBlock.GetComponent<BlockScript>();

        int originalRot = bs.rotIndex;
        while (bs.rotIndex != bs.solutionIndex)
            bs.RotateBlock();

        GameObject tipGO = new GameObject();
        tipGO.name = randomBlock.name;
        tipGO.transform.SetParent(GameObject.Find("Tips").transform);

        foreach (BlockTile bTile in bs.tileList)
        {
            Vector2 tilePos = bs.solutionPos - (bs.rotPos[bs.rotIndex] - bTile.relativePos);

            GameObject tipBlock = Instantiate(Resources.Load("Prefabs/Base Block Square"), new Vector3(tilePos.x, tilePos.y, -1f), Quaternion.identity) as GameObject;
            Destroy(tipBlock.GetComponent<BlockTile>());
            tipBlock.GetComponent<SpriteRenderer>().color = bs.bColor;
            tipBlock.GetComponent<SpriteRenderer>().sortingLayerName = "grid";
            tipBlock.GetComponent<SpriteRenderer>().sortingOrder = -1;
            tipBlock.transform.localScale = Vector3.one;
            tipBlock.transform.position = new Vector3(tilePos.x, tilePos.y, 1);

            tipBlock.transform.SetParent(tipGO.transform);
        }
        bs.bTip = true;

        while (bs.rotIndex != originalRot)
            bs.RotateBlock();
    }

    public void TogglePauseMenu()
    {
        if (!pauseMenuCanvas.activeSelf)
        {
            Time.timeScale = 0f;
            pauseMenuCanvas.SetActive(true);
            GameManager.Instance.gamePaused = true;
            this.PostNotification(PauseNotification);
        }

        else
        {
            Time.timeScale = 1f;
            pauseMenuCanvas.SetActive(false);
            GameManager.Instance.gamePaused = false;
            this.PostNotification(PauseNotification);
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

    public void LoadLevelSelector()
    {
        Debug.LogWarning("Going to Level Selector");
        StateMachine.state = GameState.LevelSelector;
        SceneManager.LoadScene("Level Selector");
    }

    public void LoadNextLevel()
    {
        StateMachine.currentLevel++;
        try
        {
            Game currentGame = SaveLoad.savedMaps[StateMachine.currentGridSize][StateMachine.currentLevel];
        }
        catch
        {
            Debug.LogWarning("Não foi possível carregar o mapa " + StateMachine.currentGridSize + "x" + StateMachine.currentLevel);
            return;
        }
        SceneManager.LoadScene("Base Map", LoadSceneMode.Single);
    }

    public void DeleteSave()
    {
        SaveLoad.ResetProgress();
    }
}
