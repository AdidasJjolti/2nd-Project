using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine.UI;

public enum eApplianceType
{
    STOVE = 0,
    MICROWAVE,
    DOUGH,
    FRYING_PAN
}

public enum eApplianceState
{
    READY = 0,
    COOKING,
    COMPLETE
}

public class CookingAppliances : MonoBehaviour
{
    [SerializeField] eApplianceState state;
    [SerializeField] eApplianceType applianceType;
    [SerializeField] private Transform cookingPoint;
    List<int> recipeList;

    [SerializeField] private GameObject applianceStates;
    [SerializeField] private Sprite[] stateImages;
    private Image stateIcon;

    Camera camera;

    void Awake()
    {
        state = eApplianceState.READY;
        Player.StartCooking += Cooking;
    }

    void Start()
    {
        camera = Camera.main;
        applianceStates.transform.position = camera.WorldToScreenPoint(cookingPoint.position + new Vector3(2, 1, 6));
        stateIcon = applianceStates.transform.GetChild(1).GetComponent<Image>();
    }

    void Update()
    {
        
    }

    // ���� �ⱸ�� ���¸� �Ѱ��� public �Լ�
    public eApplianceState GetApplianceState()
    {
        return state;
    }

    public void SetApplianceReady()
    {
        state = eApplianceState.READY;
        // �丮 �غ� ���¸� �丮 �ⱸ ���� �̹��� ��ü
        stateIcon.sprite = stateImages[(int)state];
    }

    //cookingIngredient = ���� �ⱸ�� ������ ���, �÷��̾ ������ �ִ� ��� ���ӿ�����Ʈ
    void Cooking(GameObject cookingIngredient, GameObject cookingAppliance)
    {
        // �÷��̾ ������ ���� �ⱸ�� �ڱ� �ڽ��� ������ üũ, �ٸ� ���� �ⱸ������ �̺�Ʈ �������� ����
        if (cookingAppliance != gameObject)
        {
            return;
        }

        StartCoroutine(isCooking(cookingIngredient));
    }

    //objIngredient = cookingIngredient, ������ ���� ������Ʈ (�÷��̾ �ְ��� �ϴ� ���� ������Ʈ)
    //state�� Ŭ������ ��� ������ ���� �ٸ� �ڷ�ƾ���� ���¸� ����
    //objIngredient�� �Լ��� �Ű� ����(���� ����)�� ���� �ٸ� �ڷ�ƾ���� ���� �ٸ��� �Ǵ�
    IEnumerator isCooking(GameObject objIngredient)
    {
        // �̹� �丮 ���̸� �ڷ�ƾ �������� ����
        if(state == eApplianceState.COOKING)
        {
            AddMissCount(objIngredient);
            yield break;
        }

        Ingredient ingre = objIngredient.GetComponent<Ingredient>();
        // (�ӽ�) ���� �ⱸ�� �丮���� �ƴ� ��, �̹� �丮�� ��Ḧ �ٽ� ������ ���� ��� �ı��ϰ� �̽� ī��Ʈ +1
        if (ingre.isCooked)
        {
            AddMissCount(objIngredient);
            yield break;
        }

        // ���չ� Ű ������ ��ȯ�� ������� ����
        int cookedIngredient = GameManager.Instance.GetAvailableID((int)ingre.GetIngredientType(), (int)applianceType);

        // ���չ� Ű ������ ��ȯ�� ������� 0���� ũ�� � ������� ������ ���� ��Ȳ 
        bool isCookable = cookedIngredient > 0;

        // ������ �� ������ ��� �ı�, �̽� ī��Ʈ +1, �ڷ�ƾ Ż��
        if(!isCookable)
        {
            AddMissCount(objIngredient);
            yield break;
        }


        //�Ʒ����ʹ� �������� ��Ȳ, objIngredient�� �丮 �ⱸ�� ��� ����

        objIngredient.transform.parent = cookingPoint;
        objIngredient.transform.localPosition = new Vector3(0, 0, 0);

        state = eApplianceState.COOKING;

        // �丮�� ���¸� �丮 �ⱸ ���� �̹��� ��ü
        stateIcon.sprite = stateImages[(int)state];

        float waitTime = 0;

        while (waitTime <= 5)
        {
            yield return new WaitForSeconds(0.01f);
            waitTime += 0.01f;
        }

        state = eApplianceState.COMPLETE;
        // �丮 �Ϸ� ���¸� �丮 �ⱸ ���� �̹��� ��ü
        stateIcon.sprite = stateImages[(int)state];


        // �丮�� ������ ����� ingredientType�� ��ȯ�ϱ� ���� ���չ� Ű ������ ��ȯ�� ������� �Ű� ������ ����
        //ingre.SetCookingResultID(cookedIngredient);

        //Todo : objIngredient �ı��ϰ� ���� ����� �´� ��� ������ ����, cookingPoint�� �ڽ� ������Ʈ�� ����
        //       ���ӸŴ��� ��ũ��Ʈ���� ������ �������� ����
        GameObject cooked = Instantiate(PrefabsManager.Instance.GetCookedPrefab(cookedIngredient % 100));
        cooked.GetComponent<Ingredient>().isCooked = true;
        cooked.transform.parent = cookingPoint;
        cooked.transform.localPosition = new Vector3(0, 0, 0);
        Destroy(objIngredient);
    }

    void AddMissCount(GameObject objIngredient)
    {
        GameManager.Instance.CountMissCount();
        Debug.Log("Miss Count +1");
        Destroy(objIngredient);
    }
}
