using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    public int gridSize;
    public int page;
    int animationTime = 20;

    int totalPages;
    bool changingPage = false;

    GameObject LevelsPanel;
    GameObject LevelsPanel2;
    GameObject[] buttonsPanel = new GameObject[20];
    GameObject[] buttonsPanel2 = new GameObject[20];

    #region UI
    public void GoToGridSelector()
    {
        Debug.LogWarning("Going to Grid Selector");

        StateMachine.state = GameState.GridSelector;
        SceneManager.LoadScene("Grid Selection", LoadSceneMode.Single);
    }

    public void NextPageButton()
    {
        if (!changingPage)
            MovePanel(true);
    }

    public void LastPageButton()
    {
        if (!changingPage)
            MovePanel(false);
    }
    #endregion

    void Awake()
    {
        SaveLoad.LoadProgress();
        SaveLoad.LoadMaps();
        gridSize = StateMachine.currentGridSize;
        totalPages = /*SaveLoad.savedMaps[gridSize].Count / 20*/5;

        LevelsPanel = GameObject.Find("Levels Panel 1");
        LevelsPanel2 = GameObject.Find("Levels Panel 2");

        page = 0;
        for (int n = 0; n < 20; n++)
        {
            GameObject button = Instantiate(Resources.Load("UI/LevelSelectionButton")) as GameObject;
            buttonsPanel[n] = button;
            button.transform.SetParent(LevelsPanel.transform);
            button.name = "Button " + gridSize + "x" + n;
            button.transform.localScale = Vector3.one;

            button.GetComponent<LevelSelectorButton>().levelToLoad = n;
            button.GetComponentInChildren<Text>().text = (n + 1).ToString();
        }

        page++;
        for (int n = 0; n < 20; n++)
        {
            GameObject button = Instantiate(Resources.Load("UI/LevelSelectionButton")) as GameObject;
            buttonsPanel2[n] = button;
            button.transform.SetParent(LevelsPanel2.transform);
            button.name = "Button " + gridSize + "x" + n;
            button.transform.localScale = Vector3.one;
        }
        page = 0;

        FixTitle();
    }

    void FixTitle()
    {
        Text titleText = GameObject.Find("Title Text").GetComponent<Text>();
        switch (gridSize)
        {
            case 4: titleText.text = "FÁCIL"; break;
            case 5: titleText.text = "MÉDIO"; break;
            case 6: titleText.text = "DIFÍCIL"; break;
            default: titleText.text = ""; break;
        }
        titleText.text += string.Format(" ({0}x{0})", gridSize);
    }

    void MovePanel(bool increasePage)
    {
        int side = 0;
        if (increasePage)
        {
            if (page == totalPages)
                return;
            page++;
            side = -1;
        }
        else
        {
            if (page == 0 || changingPage)
                return;
            page--;
            side = +1;
        }

        for (int n = 0; n < buttonsPanel2.Length; n++)
        {
            buttonsPanel2[n].GetComponent<LevelSelectorButton>().levelToLoad = (page * 20 + n);
            buttonsPanel2[n].GetComponent<LevelSelectorButton>().CheckMapExists();
            buttonsPanel2[n].GetComponentInChildren<Text>().text = ((page * 20 + n) + 1).ToString();
        }

        RectTransform rtLP = LevelsPanel.GetComponent<RectTransform>();
        LevelsPanel2.GetComponent<RectTransform>().anchoredPosition = new Vector2(rtLP.anchoredPosition.x - side * rtLP.GetWidth(), rtLP.anchoredPosition.y);

        GameObject levelsPanelGroup = LevelsPanel.transform.parent.gameObject;
        RectTransform rt = levelsPanelGroup.GetComponent<RectTransform>();
        StartCoroutine(MoveUI(rt, new Vector2(side * rt.GetWidth() / 2, 0), animationTime));

        GameObject aux = LevelsPanel;
        LevelsPanel = LevelsPanel2;
        LevelsPanel2 = aux;

        GameObject[] aux2 = buttonsPanel;
        buttonsPanel = buttonsPanel2;
        buttonsPanel2 = aux2;
    }

    IEnumerator MoveUI(RectTransform rt, Vector2 distance, int frames)
    {
        Vector2 startPos = rt.anchoredPosition;
        Vector2 step = distance / frames;

        changingPage = true;
        for (int n = 0; n < frames; n++)
        {
            rt.anchoredPosition = rt.anchoredPosition + step;
            yield return null;
        }
        rt.anchoredPosition = startPos + distance;
        changingPage = false;
    }
}
