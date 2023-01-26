using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

public enum eApplianceType
{
    MICROWAVE = 0,
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

    // 조리 기구의 상태를 넘겨줄 public 함수
    public eApplianceState GetApplianceState()
    {
        return state;
    }

    public void SetApplianceReady()
    {
        state = eApplianceState.READY;
    }

    //cookingIngredient = 조리 기구에 투입할 재료, 플레이어가 가지고 있는 재료 게임오브젝트
    void Cooking(GameObject cookingIngredient, GameObject cookingAppliance)
    {
        // 플레이어가 접촉한 조리 기구와 자기 자신이 같은지 체크, 다른 조리 기구에서는 이벤트 동작하지 않음
        if (cookingAppliance != gameObject)
        {
            return;
        }

        StartCoroutine(isCooking(cookingIngredient));
    }

    //objIngredient = cookingIngredient, 동일한 게임 오브젝트 (플레이어가 넣고자 하는 게임 오브젝트)
    //state는 클래스의 멤버 변수라서 서로 다른 코루틴에서 상태를 공유
    //objIngredient는 함수의 매개 변수(지역 변수)라서 서로 다른 코루틴에서 서로 다르게 판단
    IEnumerator isCooking(GameObject objIngredient)
    {
        // 이미 요리 중이면 코루틴 실행하지 않음
        if(state == eApplianceState.COOKING)
        {
            GameManager.Instance.missCount += 1;
            Debug.Log("Miss Count +1");
            Destroy(objIngredient);
            yield break;
        }

        // (임시) 조리 기구가 요리중이 아닐 때, 이미 요리된 재료를 다시 넣으면 가진 재료 파괴하고 미스 카운트 +1
        if (objIngredient.GetComponent<Ingredient>().isCooked)
        {
            GameManager.Instance.missCount += 1;
            Debug.Log("Miss Count +1");
            Destroy(objIngredient);
            yield break;
        }


        //아래부터는 정상적인 상황, objIngredient는 요리 기구가 들고 있음

        objIngredient.transform.parent = cookingPoint;
        objIngredient.transform.localPosition = new Vector3(0, 0, 0);

        state = eApplianceState.COOKING;
        float waitTime = 0;

        while (waitTime <= 100)
        {
            yield return new WaitForSeconds(0.01f);
            waitTime += 0.01f;
        }

        state = eApplianceState.COMPLETE;
    }
}
