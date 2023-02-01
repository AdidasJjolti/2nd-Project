using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

public enum eApplianceType
{
    MICROWAVE = 0,
    STOVE,
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

    void Awake()
    {
        state = eApplianceState.READY;
        Player.StartCooking += Cooking;
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
        float waitTime = 0;

        while (waitTime <= 100)
        {
            yield return new WaitForSeconds(0.01f);
            waitTime += 0.01f;
        }

        state = eApplianceState.COMPLETE;

        // �丮�� ������ ����� ingredientType�� ��ȯ�ϱ� ���� ���չ� Ű ������ ��ȯ�� ������� �Ű� ������ ����
        ingre.SetCookingResultID(cookedIngredient);

        //Todo : objIngredient �ı��ϰ� ���� ����� �´� ��� ������ ����, cookingPoint�� �ڽ� ������Ʈ�� ����
        //       ���ӸŴ��� ��ũ��Ʈ���� ������ �������� ����
    }

    void AddMissCount(GameObject objIngredient)
    {
        GameManager.Instance.missCount += 1;
        Debug.Log("Miss Count +1");
        Destroy(objIngredient);
    }
}
