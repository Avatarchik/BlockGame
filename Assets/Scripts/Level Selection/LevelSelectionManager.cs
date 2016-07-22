using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    public int gridSize;
    public int page;

    int lastPage;
    bool changingPage = false;

    GameObject LevelsPanel;
    GameObject LevelsPanel2;
    GameObject[] buttonsPanel = new GameObject[20];
    GameObject[] buttonsPanel2 = new GameObject[20];

    Vector2 spacing = new Vector2(20, 30);
    Vector2 basePos = new Vector2(50, -100);

    void Awake()
    {
        SaveLoad.LoadProgress();
        SaveLoad.LoadMaps();
        gridSize = StateMachine.currentGridSize;
        lastPage = SaveLoad.savedMaps[gridSize].Count / 20;

        int n = 0;

        for (int y = 0; y < 4; y++)
            for (int x = 0; x < 5; x++)
            {
                GameObject button = Instantiate(Resources.Load("UI/LevelSelectionButton")) as GameObject;
                buttonsPanel[n] = button;
                LevelsPanel = GameObject.Find("Levels Panel 1");
                button.transform.SetParent(LevelsPanel.transform);
                button.name = "Button " + gridSize + "x" + n;

                RectTransform rt = button.GetComponent<RectTransform>();
                button.transform.localPosition = new Vector3(basePos.x + x * (rt.sizeDelta.x + spacing.x), basePos.y - y * (rt.sizeDelta.y + spacing.y));
                button.transform.localScale = Vector3.one;

                button.GetComponent<LevelSelectorButton>().levelToLoad = n;
                button.GetComponentInChildren<Text>().text = (n + 1).ToString();
                n++;
            }

        page++;
        n = 0;
        for (int y = 0; y < 4; y++)
            for (int x = 0; x < 5; x++)
            {
                GameObject button = Instantiate(Resources.Load("UI/LevelSelectionButton")) as GameObject;
                buttonsPanel2[n] = button;
                LevelsPanel2 = GameObject.Find("Levels Panel 2");
                button.transform.SetParent(LevelsPanel2.transform);
                button.name = "Button " + gridSize + "x" + n;

                RectTransform rt = button.GetComponent<RectTransform>();
                button.transform.localPosition = new Vector3(basePos.x + x * (rt.sizeDelta.x + spacing.x), basePos.y - y * (rt.sizeDelta.y + spacing.y));
                button.transform.localScale = Vector3.one;
                n++;
            }
        page = 0;

        FixTitle();
    }

    public void GoToGridSelector()
    {
        Debug.LogWarning("Going to Grid Selector");

        StateMachine.state = GameState.GridSelector;
        SceneManager.LoadScene("Grid Selection", LoadSceneMode.Single);
    }

    public void NextPageButton()
    {
        if (page == lastPage)
            return;

        page++;

        for (int n = 0; n < buttonsPanel2.Length; n++)
        {
            buttonsPanel2[n].GetComponent<LevelSelectorButton>().levelToLoad = (page * 20 + n);
            buttonsPanel2[n].GetComponentInChildren<Text>().text = ((page * 20 + n) + 1).ToString();
            buttonsPanel2[n].GetComponent<LevelSelectorButton>().CheckMapExists();
        }

        GameObject levelsPanelGroup = LevelsPanel.transform.parent.gameObject;
        RectTransform rt2 = levelsPanelGroup.GetComponent<RectTransform>();
        RectTransform rtLP = LevelsPanel.GetComponent<RectTransform>();

        LevelsPanel2.GetComponent<RectTransform>().anchoredPosition = new Vector2(rtLP.anchoredPosition.x + rtLP.GetWidth(), rtLP.anchoredPosition.y);

        StartCoroutine(MoveUI(rt2, new Vector2(-rt2.GetWidth() / 2, 0), 30));

        GameObject aux = LevelsPanel;
        LevelsPanel = LevelsPanel2;
        LevelsPanel2 = aux;

        var aux2 = buttonsPanel;
        buttonsPanel = buttonsPanel2;
        buttonsPanel2 = aux2;
    }

    public void LastPageButton()
    {
        if (page == 0 || changingPage)
            return;

        page--;

        for (int n = 0; n < buttonsPanel2.Length; n++)
        {
            buttonsPanel2[n].GetComponent<LevelSelectorButton>().levelToLoad = (page * 20 + n);
            buttonsPanel2[n].GetComponentInChildren<Text>().text = ((page * 20 + n) + 1).ToString();
            buttonsPanel2[n].GetComponent<LevelSelectorButton>().CheckMapExists();
        }

        GameObject levelsPanelGroup = LevelsPanel.transform.parent.gameObject;
        RectTransform rt2 = levelsPanelGroup.GetComponent<RectTransform>();
        RectTransform rtLP = LevelsPanel.GetComponent<RectTransform>();

        LevelsPanel2.GetComponent<RectTransform>().anchoredPosition = new Vector2(rtLP.anchoredPosition.x - rtLP.GetWidth(), rtLP.anchoredPosition.y);

        StartCoroutine(MoveUI(rt2, new Vector2(+rt2.GetWidth() / 2, 0), 30));

        GameObject aux = LevelsPanel;
        LevelsPanel = LevelsPanel2;
        LevelsPanel2 = aux;

        var aux2 = buttonsPanel;
        buttonsPanel = buttonsPanel2;
        buttonsPanel2 = aux2;
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
