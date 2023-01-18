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

    public delegate void CookingEventHandler(GameObject objIngredient, GameObject appliance);      //�Լ��� ���, ������ ����
    public static event CookingEventHandler StartCooking;                                                                          //������ �Լ��� ��� Ʋ

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

                if (isHavingIngredient == false)            // �����̽��� �Է��� �� ��� ������ ���°� �ƴϸ� ��� Ŭ���� �˻�
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

                    Transform parent = possesingIngredient.transform.parent;                  // parent ���� ������ IngredientPoint�� ����

                    possesingIngredient.transform.parent = null;                              // IngredientPoint�� ��Ʈ ������Ʈ�� ����

                    if (ingredientType == eIngredientType.LETTUCE || ingredientType == eIngredientType.PORK || ingredientType == eIngredientType.TOMATO)       //possesingIngredient.GetComponent<Lettuce>() || possesingIngredient.GetComponent<Pork>() || possesingIngredient.GetComponent<Tomato>())
                    {
                        possesingIngredient.transform.localScale = new Vector3(50f, 50f, 50f);    // possesingIngredient�� ���� �������� 50���� ���� = �۷ι� ������
                    }
                    else if (ingredientType == eIngredientType.EGG || ingredientType == eIngredientType.FLOUR || ingredientType == eIngredientType.POTATO) // possesingIngredient.GetComponent<Egg>() || possesingIngredient.GetComponent<Flour>() || possesingIngredient.GetComponent<Potato>())
                    {
                        possesingIngredient.transform.localScale = new Vector3(3f, 3f, 3f);    // possesingIngredient�� ���� �������� 3���� ���� = �۷ι� ������
                    }

                    possesingIngredient.transform.parent = parent;                            // �ٽ� IngredientPoint�� IngredientPoint�� �ʱ�ȭ, Player�� �ڽ� ������Ʈ�� ����

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
                        StartCooking(possesingIngredient, trTarget.gameObject);          //StartCooking ���� �ִ� ��� �Լ����� possesingIngredient ����
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

    //�丮 �Ϸ�� ��Ḧ �������� �Լ�
    void RetrieveFood(GameObject Ingredient, GameObject settingAppliance, eApplianceState appState)
    {
        if (Ingredient.GetComponent<Ingredient>().isCooked = true && settingAppliance.GetComponent<CookingAppliances>())
        {

        }
    }
}
