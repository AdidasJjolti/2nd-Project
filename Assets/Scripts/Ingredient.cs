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
    POTATO
}


public class Ingredient : MonoBehaviour
{
    [SerializeField] eIngredientType ingredientType;

    public bool isCooked;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public eIngredientType GetIngredientType()
    {
        return ingredientType;
    }
}
