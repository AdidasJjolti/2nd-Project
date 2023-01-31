using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePlateState
{
    READY = 0,
    COMPLETE
}

public class CookingPlate : MonoBehaviour
{
    [SerializeField] ePlateState state;
    [SerializeField] Transform[] cookingPoints;
    [SerializeField] GameObject[] foods;

    void Start()
    {
        Player.CompleteFood += CompleteFood;
        Player.RetrieveFood += RetrieveFood;
    }


    void Update()
    {

    }

    bool CompleteFood(GameObject objPlate, GameObject possesingIngredient)
    {
        // �÷��̾ ������ plate�� �ڱ� �ڽ��� ������ üũ, �ٸ� �÷���Ʈ������ �̺�Ʈ �������� ����
        if (objPlate != gameObject)
        {
            return false;
        }

        if (state == ePlateState.READY && possesingIngredient != null)
        {
            if (cookingPoints[0].childCount == 0)
            {
                possesingIngredient.transform.parent = cookingPoints[0];
                possesingIngredient.transform.localPosition = Vector3.zero;
            }
            else if (cookingPoints[1].childCount == 0)
            {
                possesingIngredient.transform.parent = cookingPoints[1];
                possesingIngredient.transform.localPosition = Vector3.zero;
            }
            else if (cookingPoints[2].childCount == 0)
            {
                possesingIngredient.transform.parent = cookingPoints[2];
                possesingIngredient.transform.localPosition = Vector3.zero;
            }
            SoundManager.Instance.GetIngredient();
            return true;
        }

        // ToDo : ������ üũ�ؼ� �����ǿ� �´� ������� Ȯ���ϰ� �´� �������� ��ȯ�ϴ� �ڵ� �߰� �ʿ�, �´� �����ǰ� ������ �÷��� ��Ḧ ��� �ı��ϰ� �̽� ī��Ʈ +1
        else if (state == ePlateState.READY && possesingIngredient == null)
        {
            var ingredients = gameObject.GetComponentsInChildren<Ingredient>();

            foreach (var item in ingredients)
            {
                Destroy(item.gameObject);
            }

            // ToDo : �ӽ÷� ���� ������ ������Ʈ ������ ��� ����
            var newObj = Instantiate(foods[0], cookingPoints[0].position, cookingPoints[0].rotation);
            newObj.transform.parent = cookingPoints[0];
            SoundManager.Instance.GetIngredient();
            state = ePlateState.COMPLETE;

            return false;
        }

        return false;
    }

    bool RetrieveFood(GameObject objPlate, GameObject food)
    {
        // �÷��̾ ������ plate�� �ڱ� �ڽ��� ������ üũ, �ٸ� �÷���Ʈ������ �̺�Ʈ �������� ����
        if (objPlate != gameObject)
        {
            return false;
        }

        if(cookingPoints[0].childCount != 0)
        {
            Transform[] foods = cookingPoints[0].GetComponentsInChildren<Transform>();

            if(foods != null)
            {
                for(int i = 1; i < foods.Length; i++)
                {
                    if(foods[i] != cookingPoints[0])   // foods[i] = cookingPoints[0] : �ڱ� �ڽ��� ������Ʈ
                    {
                        Destroy(foods[i].gameObject);
                    }
                }
            }
            // ToDo : ���� �Ŵ��� or �������� �Ŵ������� ���� �ø���
            Debug.Log("100 Points");
        }

        state = ePlateState.READY;
        return false;
    }

    public ePlateState GetCookingPlateState()
    {
        return state;
    }
}
