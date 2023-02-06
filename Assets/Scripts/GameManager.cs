using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<GameManager>();

                if(instance == null)
                {
                    Debug.LogError("GameManager doesn't exist.");
                    return null;
                }
            }
            return instance;
        }
    }

    [SerializeField] private GameObject SettingsPopup;
    [SerializeField] private GameObject GameOverUI;

    private float maxstageTime = 10;
    private float curstageTime;
    [SerializeField] private Slider stageTimerSlider;
    [SerializeField] private TextMeshProUGUI stageTimer;

    public int maxmissCount;
    public int missCount;
    [SerializeField] private Image[] missCountImages;

    public bool isTimeCheat;
       
    Dictionary<(int, int), int> dicRecipe;
    Dictionary<(int, int, int), int> dicCompleteFood;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        SettingsPopup.SetActive(false);
        maxstageTime = 15;
    }

    void Start()
    {
        curstageTime = maxstageTime;
        dicRecipe = CSVReader.Read("recipe");
        dicCompleteFood = CSVReader.ReadCompleteFood("complete_food");

        maxmissCount = 3;
        for(int i = 0; i < maxmissCount; i++)
        {
            missCountImages[i].gameObject.SetActive(true);
        }
    }

    void Update()
    {
        OpenSettingsPopup();
        TimeCheat();
        CountStageTime();
    }

    void OpenSettingsPopup()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(SettingsPopup.activeSelf == false)
            {
                GameObject.Find("Canvas").transform.Find("GameSettingsPopup").gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                SettingsPopup.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void OnClickSettingsPopupCloseButton()
    {
        SettingsPopup.SetActive(false);
        Time.timeScale = 1;
    }

    void CountStageTime()
    {
        if(curstageTime > 0)
        {
            stageTimerSlider.transform.Find("Fill Area").gameObject.SetActive(true);
            curstageTime -= Time.deltaTime;
            stageTimerSlider.value = curstageTime / maxstageTime;
        }
        else
        {
            curstageTime = 0f;

            if (isTimeCheat)
            {
                return;
            }

            Time.timeScale = 0;
            stageTimerSlider.transform.Find("Fill Area").gameObject.SetActive(false);
            OpenGameOverUI();
        }

        if(curstageTime > 10)
        {
            stageTimer.text = ((int)curstageTime).ToString();
        }
        else if (curstageTime > 0)
        {
            stageTimer.text = $"{curstageTime:0.0}";
        }
        else
        {
            stageTimer.text = "0";
        }
    }

    void OpenGameOverUI()
    {
        if (GameOverUI.activeSelf == false)
        {
            GameObject.Find("Canvas").transform.Find("GameOverUI").gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            SettingsPopup.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void TimeCheat()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            isTimeCheat = !isTimeCheat;
        }
    }

    public void CountMissCount()
    {
        missCount++;

        for (int i = 0; i < maxmissCount; i++)
        {
            missCountImages[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < (maxmissCount - missCount); i++)
        {
            missCountImages[i].gameObject.SetActive(true);
        }

        if(missCount >= maxmissCount)
        {
            OpenGameOverUI();
        }
    }

    /// <summary>
    /// 재료id와 요리 기구id를 읍합한 결과 반환
    /// </summary>
    /// <param name="ingredientID"></param>
    /// <param name="applianceID"></param>
    /// <returns></returns>
    public int GetAvailableID(int ingredientID, int applianceID)
    {
        // 읍합법에 있는 키 값을 보유하지 않으면 -1 값 반환
        if(dicRecipe.ContainsKey((ingredientID, applianceID)) == false)
        {
            return -1;
        }

        return dicRecipe[(ingredientID, applianceID)];
    }

    public int GetCompleteFoodID(int ingredient1ID, int ingredient2ID, int ingredient3ID)
    {
        List<int> tupleList = new List<int>();
        tupleList.Add(ingredient1ID);
        tupleList.Add(ingredient2ID);
        tupleList.Add(ingredient3ID);
        tupleList.Sort();

        if (dicCompleteFood.ContainsKey((tupleList[0], tupleList[1], tupleList[2])) == false)
        {
            return -1;
        }

        return dicCompleteFood[(tupleList[0], tupleList[1], tupleList[2])];
    }
}