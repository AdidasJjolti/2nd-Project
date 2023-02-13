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
    Dictionary<int, (int, int)> dicRecipeWiki = new Dictionary<int, (int, int)>();            // Key : 요리된 재료id, Value : (원재료id, 요리 기구id)   
                                                                                              // dicOrder에서 받아온 키 값으로 원재료, 요리 기구를 주문 UI에 표시
    Dictionary<(int, int, int), int> dicCompleteFood;
    Dictionary<int, (int, int, int)> dicOrder = new Dictionary<int, (int, int, int)>();       // Key : 완성 음식id, Value : (요리된 재료1id, 요리된 재료2id, 요리된 재료3id)
                                                                                              // dicCompleteFood의 value를 주문 UI의 음식 스프라이트 인덱스로 사용

    int maxOrder = 3;                                            // 최대 주문 갯수
    float orderInterval = 5f;                                    // 다음 주문 들어오는 시간 간격
    [SerializeField] private Sprite[] foodSprites;               // 주문 UI에 표시할 음식 스프라이트 배열
    [SerializeField] private Sprite[] ingredientSprites;         // 주문 UI에 표시할 원재료 스프라이트 배열
    [SerializeField] private Sprite[] applianceSprites;          // 주문 UI에 표시할 요리 기구 스프라이트 배열
    List<GameObject> orderList = new List<GameObject>();
    [SerializeField] GameObject objOrder;

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

        foreach (var item in dicCompleteFood)
        {
            dicOrder[item.Value] = item.Key;           // '딕셔너리명[키] = 밸류'로 dicOrder 딕셔너리 정의
        }

        foreach (var item in dicRecipe)
        {
            dicRecipeWiki[item.Value] = item.Key;      // '딕셔너리명[키] = 밸류'로 dicRecipeWiki 딕셔너리 정의
        }

        // 미스카운트 계산
        maxmissCount = 3;
        for(int i = 0; i < maxmissCount; i++)
        {
            missCountImages[i].gameObject.SetActive(true);
        }

        InvokeRepeating("ReceiveOrder", 5f, 5f);
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

    public void ReceiveOrder()
    {
        // 주문이 가득찬 상태면 더 이상 주문 받지 않음
        if (orderList.Count >= maxOrder)
        {
            return;
        }

        // 1006 ~ 1008 사이에서 주문 받은 음식 인덱스 결정
        int foodIndex = (Random.Range((int)eIngredientType.START, (int)eIngredientType.MAX) % 100) - 6;

        // foodSprites에서 주문UI에 표시할 음식 스프라이트 저장
        Sprite foodSprite = foodSprites[foodIndex];

        // 주문 들어온 UI 게임오브젝트 생성 후 부모 오브젝트의 자식으로 설정
        GameObject orderObject = Instantiate(objOrder, GameObject.Find("OrderUI").transform.position, Quaternion.identity, GameObject.Find("OrderUI").transform);
        orderObject.transform.localScale = new Vector3(1, 1, 1);
        orderObject.transform.parent = GameObject.Find("OrderUI").transform;
        orderList.Add(orderObject);

        // 생성한 주문 UI 게임오브젝트의 음식 스프라이트 지정
        orderObject.GetComponent<UIOrder>().SetIFoodImages(foodSprite);

        // foodIndex로 각 요리 재료의 원재료id, 요리기구id 출력
        var cookedIngre = dicOrder[(foodIndex + (int)eIngredientType.START)];
        List<int> sortedIngre = new List<int>();
        sortedIngre.Add(cookedIngre.Item1);
        sortedIngre.Add(cookedIngre.Item2);
        sortedIngre.Add(cookedIngre.Item3);

        sortedIngre.Sort((a, b) =>
        {
            int result = b.CompareTo(a);
            return result;
        });

        for(int i = 0; i < sortedIngre.Count; i++)
        {
            CheckRecipeWiki(sortedIngre[i], i, orderObject);
        }
    }

    /// <summary>
    /// 1. cookedIngre value 중에서 -1이 있는지 체크 (재료가 없음)
    /// 2. cookedIngre value가 -1이 아닌 것 중에서 요리 기구가 -1인지 체크   =>  recipe에서 찾을 수 있는 key가 없음 (조리가 없음)
    /// 3. cookedIngre value가 -1이 아닌 것 중에서 요리 기구도 -1이 아닌지 체크 (조리할 재료가 있음)
    /// </summary>
    /// <param name="cookedIngreCheck"></param>
    /// <param name="orderObj"></param>
    void CheckRecipeWiki(int cookedIngreCheck, int orderIndex, GameObject orderObj)
    {
        // 1. cookedIngre value 중에서 -1이 있는지 체크
        if(cookedIngreCheck == -1)
        {
            orderObj.GetComponent<UIOrder>().SetIIngredientImages(null, orderIndex);
        }
        // 2. cookedIngre value가 -1이 아닌 것 중에서 요리 기구가 -1인지 체크  =>  recipe에서 찾을 수 있는 key가 없음 (조리가 없음)
        else if (dicRecipeWiki.ContainsKey(cookedIngreCheck) == false)
        {
            Debug.Log(cookedIngreCheck);
            Sprite ingredientSprite = ingredientSprites[cookedIngreCheck > 1000 ? cookedIngreCheck%100 - 1 : cookedIngreCheck];
            orderObj.GetComponent<UIOrder>().SetIIngredientImages(ingredientSprite, orderIndex);
        }
        // 3. cookedIngre value가 -1이 아닌 것 중에서 요리 기구도 -1이 아닌지 체크 (조리할 재료가 있음)
        else
        {
            Sprite ingredientSprite = ingredientSprites[dicRecipeWiki[cookedIngreCheck].Item1];
            Sprite applianceSprite = applianceSprites[dicRecipeWiki[cookedIngreCheck].Item2];
            orderObj.GetComponent<UIOrder>().SetIIngredientImages(ingredientSprite, orderIndex);
            orderObj.GetComponent<UIOrder>().SetIApplianceImages(applianceSprite, orderIndex);
        }
    }
}