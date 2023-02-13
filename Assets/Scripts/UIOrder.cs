using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOrder : MonoBehaviour
{
    [SerializeField] Image imgFood;
    [SerializeField] Image[] imgIngredient;
    [SerializeField] Image[] imgAppliance;
    float orderMaxTime = 30f;                                    // �ֹ� ���� �ִ� �ð�
    float curOrderTime;                                          // �ֹ� ���� �� ��� �ð�
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
        // ���� �̹��� ����, �̹��� ���İ� 1�� ����
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
        // ��� �̹��� ����, �̹��� ���İ� 1�� ����
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
        // �丮 �ⱸ �̹��� ����, �̹��� ���İ� 1�� ����
        imgAppliance[index].sprite = applianceSprite;
        imgAppliance[index].color = new Color(1, 1, 1, 1);
    }
}
