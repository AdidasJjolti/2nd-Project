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
    //[SerializeField] GameObject[] foods;        // 임시 코드


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
        // 플레이어가 접촉한 plate와 자기 자신이 같은지 체크, 다른 플레이트에서는 이벤트 동작하지 않음
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

        else if (state == ePlateState.READY && possesingIngredient == null)
        {
            var ingredients = gameObject.GetComponentsInChildren<Ingredient>();

            int ingredient1ID = ingredients.Length > 0 && ingredients[0] != null ? ingredients[0].GetCookedID() : -1;
            int ingredient2ID = ingredients.Length > 1 && ingredients[1] != null ? ingredients[1].GetCookedID() : -1;
            int ingredient3ID = ingredients.Length > 2 && ingredients[2] != null ? ingredients[2].GetCookedID() : -1;

            int completeFoodID = GameManager.Instance.GetCompleteFoodID(ingredient1ID, ingredient2ID, ingredient3ID);

            foreach (var item in ingredients)
            {
                Destroy(item.gameObject);
            }

            // 맞는 레시피가 없으면 탈출
            if (completeFoodID == -1)
            {
                GameManager.Instance.CountMissCount();
                return false;
            }

            var newObj = Instantiate(PrefabsManager.Instance.GetCookedPrefab(completeFoodID%100), cookingPoints[0].position, cookingPoints[0].rotation);
            newObj.transform.parent = cookingPoints[0];
            SoundManager.Instance.GetIngredient();
            state = ePlateState.COMPLETE;

            return false;
        }

        return false;
    }

    bool RetrieveFood(GameObject objPlate, GameObject food)
    {
        // 플레이어가 접촉한 plate와 자기 자신이 같은지 체크, 다른 플레이트에서는 이벤트 동작하지 않음
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
                    if(foods[i] != cookingPoints[0])   // foods[i] = cookingPoints[0] : 자기 자신인 오브젝트
                    {
                        Destroy(foods[i].gameObject);
                    }
                }
            }

            Debug.Log("100 Points");
            GameManager.Instance.AddGameScore();
        }

        state = ePlateState.READY;
        return false;
    }

    public ePlateState GetCookingPlateState()
    {
        return state;
    }
}
