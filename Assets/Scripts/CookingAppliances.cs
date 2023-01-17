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

    void Cooking(GameObject cookingIngredient, GameObject cookingAppliance)
    {
        if(state != eApplianceState.READY || cookingAppliance != gameObject)
        {
            return;
        }
        cookingIngredient.transform.parent = cookingPoint;
        cookingIngredient.transform.localPosition = new Vector3(0,0,0);
        state = eApplianceState.COOKING;
    }
}
