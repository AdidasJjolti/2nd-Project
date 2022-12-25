using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

public class CookingAppliances : MonoBehaviour
{
    enum eApplianceType
    {
        STOVE = 0,
        OVEN,
        DOUGH,
        FRYING_PAN
    }

    enum eIngredientType
    {
        LETTUCE = 0,
        MEAT,
        TOMATO,
        EGG,
        FLOUR,
        POTATO
    }

    List<int> recipeList;

    void Awake()
    {

    }

    void Update()
    {
        
    }
}
