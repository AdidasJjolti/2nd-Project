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

    public void SetIIngredientImages()
    {

    }

    public void SetIApplianceImages()
    {

    }
}
