using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController characterController;
    Vector3 moveDirection;
    public int playerSpeed;
    Animator animator;

    [SerializeField] private GameObject[] Ingredients;
    [SerializeField] private Transform IngredientPoint;

    GameObject possesingIngredient;

    bool isHavingIngredient;

    public delegate void CookingEventHandler(GameObject objIngredient, GameObject appliance);      //함수의 모양, 원형을 정의
    public static event CookingEventHandler StartCooking;                                                                          //실행할 함수를 담는 틀

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        StartCooking += SetFood;
    }

    void Start()
    {
        var dicResult = CSVReader.Read("recipe");
        Debug.Log(dicResult[(1, 0)]);
        Debug.Log(dicResult[(1, 1)]);
    }

    void Update()
    {
        if (Time.timeScale == 0)
            return;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(x, 0, z);

        if (x != 0)
        {
            moveDirection = new Vector3(x, 0, 0);
            //isMoving = true;
            //animator.SetBool("isMoving",true);
        }
        else if (z != 0)
        {
            moveDirection = new Vector3(0, 0, z);
            //isMoving = true;
            //animator.SetBool("isMoving", true);
        }


        characterController.Move(moveDirection * playerSpeed * Time.deltaTime);

        //moveDirection.y = 0;

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        else
        {
            transform.rotation = transform.rotation;
        }

        animator.SetBool("isMoving", moveDirection != Vector3.zero);

        Debug.DrawRay(transform.position + new Vector3(0, 12, 0), transform.forward * 10, new Color(1, 0, 0));

        if (Input.GetKey(KeyCode.Space))
        {
            RaycastHit hitInfo;

            if (Physics.Raycast(transform.position + new Vector3(0, 12, 0), transform.forward * 15, out hitInfo, 10))
            {
                Debug.Log(hitInfo.transform.name);
                GameObject target = null;

                if (isHavingIngredient == false)            // 스페이스바 입력할 때 재료 보유중 상태가 아니면 재료 클래스 검사
                {
                    var trTarget = hitInfo.transform;
                    eIngredientType ingredientType = eIngredientType.NONE;

                    if (trTarget.GetComponent<Ingredient>())//) || trTarget.GetComponent<Pork>() || trTarget.GetComponent<Tomato>() || trTarget.GetComponent<Egg>() || trTarget.GetComponent<Flour>() || trTarget.GetComponent<Potato>())
                    {
                        ingredientType = trTarget.GetComponent<Ingredient>().GetIngredientType();
                        target = trTarget.gameObject;
                        SoundManager.Instance.GetIngredient();
                    }
                    else
                        return;

                    possesingIngredient = Instantiate(target, IngredientPoint.transform.position, IngredientPoint.transform.rotation);
                    possesingIngredient.GetComponent<BoxCollider>().enabled = false;

                    Transform parent = possesingIngredient.transform.parent;                  // parent 지역 변수를 IngredientPoint로 설정

                    possesingIngredient.transform.parent = null;                              // IngredientPoint를 루트 오브젝트로 설정

                    if (ingredientType == eIngredientType.LETTUCE || ingredientType == eIngredientType.PORK || ingredientType == eIngredientType.TOMATO)       //possesingIngredient.GetComponent<Lettuce>() || possesingIngredient.GetComponent<Pork>() || possesingIngredient.GetComponent<Tomato>())
                    {
                        possesingIngredient.transform.localScale = new Vector3(50f, 50f, 50f);    // possesingIngredient의 로컬 스케일을 50으로 설정 = 글로벌 스케일
                    }
                    else if (ingredientType == eIngredientType.EGG || ingredientType == eIngredientType.FLOUR || ingredientType == eIngredientType.POTATO) // possesingIngredient.GetComponent<Egg>() || possesingIngredient.GetComponent<Flour>() || possesingIngredient.GetComponent<Potato>())
                    {
                        possesingIngredient.transform.localScale = new Vector3(3f, 3f, 3f);    // possesingIngredient의 로컬 스케일을 3으로 설정 = 글로벌 스케일
                    }

                    possesingIngredient.transform.parent = parent;                            // 다시 IngredientPoint를 IngredientPoint로 초기화, Player의 자식 오브젝트로 복귀

                    possesingIngredient.transform.parent = IngredientPoint.transform;
                    possesingIngredient.transform.localPosition = Vector3.zero;

                    isHavingIngredient = true;
                }
                else
                {
                    var trTarget = hitInfo.transform;

                    if (trTarget.GetComponent<TrashCan>() && possesingIngredient != null)
                    {
                        Destroy(possesingIngredient);
                        isHavingIngredient = false;
                    }
                    else if (trTarget.GetComponent<CookingAppliances>() && possesingIngredient != null)
                    {
                        StartCooking(possesingIngredient, trTarget.gameObject);          //StartCooking 내에 있는 모든 함수에게 possesingIngredient 전달
                    }
                }
            }
        }
    }
    
    void SetFood(GameObject Ingredient, GameObject settingAppliance)
    {
        this.possesingIngredient = null;
        isHavingIngredient = false;
    }

    //요리 완료된 재료를 가져오는 함수
    void RetrieveFood(GameObject Ingredient, GameObject settingAppliance, eApplianceState appState)
    {
        if (Ingredient.GetComponent<Ingredient>().isCooked = true && settingAppliance.GetComponent<CookingAppliances>())
        {

        }
    }
}
