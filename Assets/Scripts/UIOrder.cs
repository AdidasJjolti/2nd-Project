using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOrder : MonoBehaviour
{
    [SerializeField] Image imgFood;
    [SerializeField] Image[] imgIngredient;
    [SerializeField] Image[] imgAppliance;
    float orderMaxTime = 30f;                                    // 주문 유지 최대 시간
    float curOrderTime;                                          // 주문 받은 후 경과 시간
    [SerializeField] private Slider orderTimerSlider;

    void Start()
    {
        curOrderTime = orderMaxTime;
    }

    void Update()
    {
        if(curOrderTime > 0)
        {
            curOrderTime -= Time.deltaTime;
            orderTimerSlider.value = curOrderTime / orderMaxTime;
        }
    }

    public void SetIFoodImages(Sprite foodSprite)
    {
        // 음식 이미지 세팅, 이미지 알파값 1로 변경
        imgFood.sprite = foodSprite;
        imgFood.color = new Color(1, 1, 1, 1);
    }

    public void SetIIngredientImages(Sprite ingreSprite, int index)
    {
        if(ingreSprite == null)
        {
            imgIngredient[index].color = new Color(0, 0, 0, 0);
            return;
        }
        // 재료 이미지 세팅, 이미지 알파값 1로 변경
        imgIngredient[index].sprite = ingreSprite;
        imgIngredient[index].color = new Color(1, 1, 1, 1);
    }

    public void SetIApplianceImages(Sprite applianceSprite, int index)
    {
        if (applianceSprite == null)
        {
            imgAppliance[index].color = new Color(0, 0, 0, 0);
            return;
        }
        // 요리 기구 이미지 세팅, 이미지 알파값 1로 변경
        imgAppliance[index].sprite = applianceSprite;
        imgAppliance[index].color = new Color(1, 1, 1, 1);
    }
}
