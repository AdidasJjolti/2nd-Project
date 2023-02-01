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
    
    GRILLED_MEAT = 101,
    GRILLED_TOMATO,
    STEAMED_EGG,
    FRIED_EGG,
    PIZZA_DOUGH,
    STEAMED_POTATO,
    FRIED_POTATO
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


    // 요리된 재료의 ingredientID를 전달할 함수, 어떤 요리된 재료가 반환되었는지 체크할 때 사용
    public void SetCookingResultID(int cookedID)
    {
        cookedIngredient = cookedID;
    }

    public int GetCookedID()
    {
        return cookedIngredient;
    }
}
