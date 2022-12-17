using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void OnClickESC()           // 테스트용 효과음 재생 함수
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            AudioClip escapeSound = audioClip[0];
            sfxPlayer.PlayOneShot(escapeSound);
        }
    }

    void Start()
    {
        if(bgmPlayer != null)
        {
            bgmPlayer.Play();
        }
    }

    void Update()
    {
        OnClickESC();
    }
}
