using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject SettingsPopup;
    private float stageTime = 200;
    [SerializeField] private GameObject StageTimer;

    void Awake()
    {
        SettingsPopup.SetActive(false);
        stageTime = 200;
    }

    void Update()
    {
        OpenSettingsPopup();
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
        stageTime -= Time.deltaTime;
        StageTimer.GetComponent<TextMeshProUGUI>().text = ((int)stageTime).ToString();
    }
}
