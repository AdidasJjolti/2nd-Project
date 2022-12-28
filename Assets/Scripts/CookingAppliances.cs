using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

public enum eApplianceType
{
    STOVE = 0,
    OVEN,
    DOUGH,
    FRYING_PAN
}

public enum eIngredientType
{
    LETTUCE = 0,
    MEAT,
    TOMATO,
    EGG,
    FLOUR,
    POTATO
}

public class CookingAppliances : MonoBehaviour
{


    List<int> recipeList;

    void Awake()
    {

    }

    void Update()
    {
        
    }
}
