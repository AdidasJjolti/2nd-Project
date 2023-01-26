using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

public enum eApplianceType
{
    MICROWAVE = 0,
    OVEN,
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
            GameManager.Instance.missCount += 1;
            Debug.Log("Miss Count +1");
            Destroy(objIngredient);
            yield break;
        }

        // (�ӽ�) ���� �ⱸ�� �丮���� �ƴ� ��, �̹� �丮�� ��Ḧ �ٽ� ������ ���� ��� �ı��ϰ� �̽� ī��Ʈ +1
        if (objIngredient.GetComponent<Ingredient>().isCooked)
        {
            GameManager.Instance.missCount += 1;
            Debug.Log("Miss Count +1");
            Destroy(objIngredient);
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
    }
}
