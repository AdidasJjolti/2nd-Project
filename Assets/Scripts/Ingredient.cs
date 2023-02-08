using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eIngredientType
{
    NONE = -1,
    LETTUCE = 0,
    PORK,
    TOMATO,
    EGG,
    FLOUR,
    POTATO,
    
    GRILLED_MEAT = 100,                  // ���� ���
    GRILLED_TOMATO,                      // ���� �丶��
    STEAMED_EGG,                         // ���� ���
    FRIED_EGG,                           // ��� �Ķ���
    PIZZA_DOUGH,                         // ���� ����
    STEAMED_POTATO,                      // ���� ����
    FRIED_POTATO = 1006,                 // ���� Ƣ��
    SALAD,                               // ������
    STEAK_MEAL,                          // ������ũ ��Ʈ
    MAX,

    START = FRIED_POTATO,
    END = STEAK_MEAL
}


public class Ingredient : MonoBehaviour
{
    [SerializeField] eIngredientType ingreType;

    public bool isCooked;
    int cookedIngredient;

    void Start()
    {
        cookedIngredient = (int)ingreType;
    }


    void Update()
    {
        
    }

    public eIngredientType GetIngredientType()
    {
        return ingreType;
    }


    // �丮�� ����� ingredientID�� ������ �Լ�, � �丮�� ��ᰡ ��ȯ�Ǿ����� üũ�� �� ���
    public void SetCookingResultID(int cookedID)
    {
        cookedIngredient = cookedID;
    }

    public int GetCookedID()
    {
        return cookedIngredient;
    }
}
