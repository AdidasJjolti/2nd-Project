using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    int gameScene;

    public static SoundManager Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<SoundManager>();

                if(instance == null)
                {
                    Debug.LogError("SoundManager doesn't exist.");
                    return null;
                }
            }

            return instance;
        }
    }


    [SerializeField] private AudioClip BGMClip;            // 브금 오디오 소스
    [SerializeField] private AudioClip[] audioClip;        // 효과음 오디오 소스

    [SerializeField] private Slider BGMSlider;             // 브금 볼륨 조절 슬라이더
    private float bgmVolume = 1;                           // 기본 볼륨값 1로 지정

    [SerializeField] private Slider SFXSlider;             // 효과음 볼륨 조절 슬라이더
    private float sfxVolume = 1;                           // 기본 볼륨값 1로 지정

    Dictionary<string, AudioClip> audioClipsDic;
    AudioSource bgmPlayer;
    AudioSource sfxPlayer;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        gameScene = SceneManager.GetActiveScene().buildIndex;


        sfxPlayer = GetComponent<AudioSource>();
        SetupBGM();

        audioClipsDic = new Dictionary<string, AudioClip>();
        foreach (AudioClip a in audioClip)
        {
            audioClipsDic.Add(a.name, a);
        }
    }

    //void AwakeAfter()
    //{
    //    sfxPlayer = GetComponent<AudioSource>();
    //    SetupBGM();

    //    audioClipsDic = new Dictionary<string, AudioClip>();
    //    foreach (AudioClip a in audioClip)
    //    {
    //        audioClipsDic.Add(a.name, a);
    //    }
    //}

    

    void Start()
    {
        if(bgmPlayer != null)
        {
            bgmPlayer.Play();
        }

        bgmPlayer.volume = BGMSlider.value;
        bgmVolume = BGMSlider.value;
        PlayerPrefs.SetFloat("bgmVolume", bgmVolume);

        sfxPlayer.volume = SFXSlider.value;
        sfxVolume = SFXSlider.value;
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }

    void Update()
    {
        OnClickESC();
        SetBGMSlider();
        SetSFXSlider();
    }

    void SetupBGM()
    {
        if (BGMClip == null)
            return;

        GameObject child = new GameObject("BGM");
        child.transform.SetParent(transform);
        bgmPlayer = child.AddComponent<AudioSource>();
        bgmPlayer.clip = BGMClip;
        bgmPlayer.volume = 1;
    }

    void OnClickESC()           // 게임 세팅 팝업 오픈 효과음 재생
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioClip escapeSound = audioClip[0];
            sfxPlayer.PlayOneShot(escapeSound);
        }
    }

    public void GetIngredient()
    {
        AudioClip getIngredientSound = audioClip[1];
        sfxPlayer.PlayOneShot(getIngredientSound);
    }

    void SetBGMSlider()
    {
        bgmPlayer.volume = BGMSlider.value;

        bgmVolume = BGMSlider.value;
        PlayerPrefs.SetFloat("bgmVolume", bgmVolume);
    }
    void SetSFXSlider()
    {
        sfxPlayer.volume = SFXSlider.value;

        sfxVolume = SFXSlider.value;
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }

}
