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
    
    GRILLED_MEAT = 100,                  // 구운 고기
    GRILLED_TOMATO,                      // 구운 토마토
    STEAMED_EGG,                         // 삶은 계란
    FRIED_EGG,                           // 계란 후라이
    PIZZA_DOUGH,                         // 피자 도우
    STEAMED_POTATO,                      // 삶은 감자
    FRIED_POTATO = 1006,                 // 감자 튀김
    SALAD,                               // 샐러드
    STEAK_MEAL,                          // 스테이크 세트
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
