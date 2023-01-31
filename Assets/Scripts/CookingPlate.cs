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

        // ToDo : 레시피 체크해서 레시피에 맞는 재료인지 확인하고 맞는 음식으로 반환하는 코드 추가 필요, 맞는 레시피가 없으면 올려둔 재료를 모두 파괴하고 미스 카운트 +1
        else if (state == ePlateState.READY && possesingIngredient == null)
        {
            var ingredients = gameObject.GetComponentsInChildren<Ingredient>();

            foreach (var item in ingredients)
            {
                Destroy(item.gameObject);
            }

            // ToDo : 임시로 넣은 샐러드 오브젝트 저장할 장소 구현
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
            // ToDo : 게임 매니저 or 스테이지 매니저에서 점수 올리기
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
