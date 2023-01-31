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

    public int missCount;
    [SerializeField] private TextMeshProUGUI missCountText;

    public bool isTimeCheat;

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
        maxstageTime = 10;
    }

    void Start()
    {
        curstageTime = maxstageTime;
    }

    void Update()
    {
        OpenSettingsPopup();
        TimeCheat();
        CountStageTime();
        CountMissCount();
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

        stageTimer.text = ((int)curstageTime).ToString();
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

    void CountMissCount()
    {
        missCountText.text = ((int)missCount).ToString();
    }
}