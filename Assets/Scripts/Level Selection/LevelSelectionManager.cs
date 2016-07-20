using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    public GameObject baseButton;
    public Text titleText;
    int gridSize;


    void Awake()
    {
        SaveLoad.LoadProgress();
        SaveLoad.LoadMaps();
        gridSize = StateMachine.currentGridSize;
        //baseButton = Resources.Load("UI/LevelSelectionButton") as GameObject;

        Vector2 spacing = new Vector2(20,30);
        Vector2 basePos = new Vector2(-490, 450);
        int n = 0;
        for (int y = 0; y < 4; y++)
            for (int x = 0; x < 5; x++)
            {
                GameObject button = Instantiate(baseButton) as GameObject;
                button.transform.SetParent(GameObject.Find("Levels Panel").transform);
                button.name = "Button " + gridSize + "x" + n;

                RectTransform rt = button.GetComponent<RectTransform>();
                button.transform.localPosition = new Vector3(basePos.x + x * (rt.sizeDelta.x + spacing.x), basePos.y - y * (rt.sizeDelta.y + spacing.y));
                button.transform.localScale = Vector3.one;

                button.GetComponent<LevelSelectorButton>().levelToLoad = n;
                button.GetComponentInChildren<Text>().text = n.ToString();
                n++;
            }
        FixTitle();
    }

    public void GoToDifficultyScene()
    {
        Debug.LogWarning("Going to Grid Selector");

        StateMachine.state = GameState.GridSelector;
        SceneManager.LoadScene("Grid Selection", LoadSceneMode.Single);
    }

    void FixTitle()
    {
        switch (gridSize)
        {
            case 4: titleText.text = "FÁCIL"; break;
            case 5: titleText.text = "MÉDIO"; break;
            case 6: titleText.text = "DIFÍCIL"; break;
            default: titleText.text = ""; break;
        }
        titleText.text += string.Format(" ({0}x{0})", gridSize);
    }
}
