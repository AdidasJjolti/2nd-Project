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

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    void CompleteFood()
    {
        string names = gameObject.GetComponentInChildren<Ingredient>().name;

        if (names == "Lettuce(Clone)" && names == "Tomato(Clone)" && names == "Egg(Clone)")
        {
            GameObject firstchild = transform.GetChild(2).GetChild(0).gameObject;
            GameObject secondchild = transform.GetChild(2).GetChild(1).gameObject;
            GameObject thirdchild = transform.GetChild(2).GetChild(2).gameObject;

            Destroy(firstchild);
            Destroy(secondchild);
            Destroy(thirdchild);

            Debug.Log("Salad is ready.");
            SoundManager.Instance.GetIngredient();
        }
    }

    public ePlateState GetCookingPlateState()
    {
        return state;
    }
}
