using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject SettingsPopup;

    void Awake()
    {
        SettingsPopup.SetActive(false);
    }

    void Update()
    {
        OpenSettingsPopup();
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
}
