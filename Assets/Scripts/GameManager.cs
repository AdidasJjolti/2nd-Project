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
    Dictionary<int, (int, int)> dicRecipeWiki = new Dictionary<int, (int, int)>();            // Key : �丮�� ���id, Value : (�����id, �丮 �ⱸid)   
                                                                                              // dicOrder���� �޾ƿ� Ű ������ �����, �丮 �ⱸ�� �ֹ� UI�� ǥ��
    Dictionary<(int, int, int), int> dicCompleteFood;
    Dictionary<int, (int, int, int)> dicOrder = new Dictionary<int, (int, int, int)>();       // Key : �ϼ� ����id, Value : (�丮�� ���1id, �丮�� ���2id, �丮�� ���3id)
                                                                                              // dicCompleteFood�� value�� �ֹ� UI�� ���� ��������Ʈ �ε����� ���

    int maxOrder = 3;                                            // �ִ� �ֹ� ����
    float orderInterval = 5f;                                    // ���� �ֹ� ������ �ð� ����
    [SerializeField] private Sprite[] foodSprites;               // �ֹ� UI�� ǥ���� ���� ��������Ʈ �迭
    [SerializeField] private Sprite[] ingredientSprites;         // �ֹ� UI�� ǥ���� ����� ��������Ʈ �迭
    [SerializeField] private Sprite[] applianceSprites;          // �ֹ� UI�� ǥ���� �丮 �ⱸ ��������Ʈ �迭
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
            dicOrder[item.Value] = item.Key;           // '��ųʸ���[Ű] = ���'�� dicOrder ��ųʸ� ����
        }

        foreach (var item in dicRecipe)
        {
            dicRecipeWiki[item.Value] = item.Key;      // '��ųʸ���[Ű] = ���'�� dicRecipeWiki ��ųʸ� ����
        }

        // �̽�ī��Ʈ ���
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
    /// ���id�� �丮 �ⱸid�� ������ ��� ��ȯ
    /// </summary>
    /// <param name="ingredientID"></param>
    /// <param name="applianceID"></param>
    /// <returns></returns>
    public int GetAvailableID(int ingredientID, int applianceID)
    {
        // ���չ��� �ִ� Ű ���� �������� ������ -1 �� ��ȯ
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
        // �ֹ��� ������ ���¸� �� �̻� �ֹ� ���� ����
        if (orderList.Count >= maxOrder)
        {
            return;
        }

        // 1006 ~ 1008 ���̿��� �ֹ� ���� ���� �ε��� ����
        int foodIndex = (Random.Range((int)eIngredientType.START, (int)eIngredientType.MAX) % 100) - 6;

        // foodSprites���� �ֹ�UI�� ǥ���� ���� ��������Ʈ ����
        Sprite foodSprite = foodSprites[foodIndex];

        // �ֹ� ���� UI ���ӿ�����Ʈ ���� �� �θ� ������Ʈ�� �ڽ����� ����
        GameObject orderObject = Instantiate(objOrder, GameObject.Find("OrderUI").transform.position, Quaternion.identity, GameObject.Find("OrderUI").transform);
        orderObject.transform.localScale = new Vector3(1, 1, 1);
        orderObject.transform.parent = GameObject.Find("OrderUI").transform;
        orderList.Add(orderObject);

        // ������ �ֹ� UI ���ӿ�����Ʈ�� ���� ��������Ʈ ����
        orderObject.GetComponent<UIOrder>().SetIFoodImages(foodSprite);

        // foodIndex�� �� �丮 ����� �����id, �丮�ⱸid ���
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
    /// 1. cookedIngre value �߿��� -1�� �ִ��� üũ (��ᰡ ����)
    /// 2. cookedIngre value�� -1�� �ƴ� �� �߿��� �丮 �ⱸ�� -1���� üũ   =>  recipe���� ã�� �� �ִ� key�� ���� (������ ����)
    /// 3. cookedIngre value�� -1�� �ƴ� �� �߿��� �丮 �ⱸ�� -1�� �ƴ��� üũ (������ ��ᰡ ����)
    /// </summary>
    /// <param name="cookedIngreCheck"></param>
    /// <param name="orderObj"></param>
    void CheckRecipeWiki(int cookedIngreCheck, int orderIndex, GameObject orderObj)
    {
        // 1. cookedIngre value �߿��� -1�� �ִ��� üũ
        if(cookedIngreCheck == -1)
        {
            orderObj.GetComponent<UIOrder>().SetIIngredientImages(null, orderIndex);
        }
        // 2. cookedIngre value�� -1�� �ƴ� �� �߿��� �丮 �ⱸ�� -1���� üũ  =>  recipe���� ã�� �� �ִ� key�� ���� (������ ����)
        else if (dicRecipeWiki.ContainsKey(cookedIngreCheck) == false)
        {
            Debug.Log(cookedIngreCheck);
            Sprite ingredientSprite = ingredientSprites[cookedIngreCheck > 1000 ? cookedIngreCheck%100 - 1 : cookedIngreCheck];
            orderObj.GetComponent<UIOrder>().SetIIngredientImages(ingredientSprite, orderIndex);
        }
        // 3. cookedIngre value�� -1�� �ƴ� �� �߿��� �丮 �ⱸ�� -1�� �ƴ��� üũ (������ ��ᰡ ����)
        else
        {
            Sprite ingredientSprite = ingredientSprites[dicRecipeWiki[cookedIngreCheck].Item1];
            Sprite applianceSprite = applianceSprites[dicRecipeWiki[cookedIngreCheck].Item2];
            orderObj.GetComponent<UIOrder>().SetIIngredientImages(ingredientSprite, orderIndex);
            orderObj.GetComponent<UIOrder>().SetIApplianceImages(applianceSprite, orderIndex);
        }
    }
}