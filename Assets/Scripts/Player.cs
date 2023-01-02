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

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        var dicResult = CSVReader.Read("recipe");
        Debug.Log(dicResult[(1,0)]);
        Debug.Log(dicResult[(1,1)]);
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

        Debug.DrawRay(transform.position + new Vector3(0, 5, 0), transform.forward * 10, new Color(1, 0, 0));

        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position + new Vector3(0, 5, 0), transform.forward * 10, out hitInfo, 10))
        {
            //Debug.Log(hitInfo.transform.parent.name);

            //GameObject target = null;
            //for(int i = 0; i < hitInfo.transform.parent.childCount; i++)
            //{
            //    if(hitInfo.transform.GetChild(i).GetComponent<Lettuce>() && Input.GetKey(KeyCode.Space))
            //    {
            //        target = transform.GetChild(i).gameObject;
            //        break;
            //    }
            //}


            if(hitInfo.transform.root.GetComponentInChildren<Lettuce>() && Input.GetKey(KeyCode.Space))
            {
                if (isHavingIngredient)
                {
                    Debug.Log("Already Got one");
                    return;
                }

                Debug.Log("Got a piece of lettuce");

                GameObject obj = hitInfo.transform.root.GetComponentInChildren<Lettuce>().gameObject;     // �������� ���� ���� ���� �ʱ�ȭ�Ͽ� Instantiate�� ���� ������Ʈ�� ���
                possesingIngredient = Instantiate(obj, IngredientPoint.transform.position, IngredientPoint.transform.rotation);


                Transform parent = possesingIngredient.transform.parent;                  // parent ���� ������ IngredientPoint�� ����

                possesingIngredient.transform.parent = null;                              // IngredientPoint�� ��Ʈ ������Ʈ�� ����
                possesingIngredient.transform.localScale = new Vector3(50f, 50f, 50f);    // possesingIngredient�� ���� �������� 50���� ���� = �۷ι� ������
                possesingIngredient.transform.parent = parent;                            // �ٽ� IngredientPoint�� IngredientPoint�� �ʱ�ȭ, Player�� �ڽ� ������Ʈ�� ����

                possesingIngredient.transform.parent = IngredientPoint.transform;
                possesingIngredient.transform.localPosition = Vector3.zero;
                isHavingIngredient = true;
            }
            else if (hitInfo.transform.root.GetComponentInChildren<Pork>() && Input.GetKey(KeyCode.Space))
            {
                if (isHavingIngredient)
                {
                    Debug.Log("Already Got one");
                    return;
                }

                Debug.Log("Got a piece of pork");

                GameObject obj = hitInfo.transform.root.GetComponentInChildren<Pork>().gameObject;     // �������� ���� ���� ���� �ʱ�ȭ�Ͽ� Instantiate�� ���� ������Ʈ�� ���
                possesingIngredient = Instantiate(obj, IngredientPoint.transform.position, IngredientPoint.transform.rotation);


                Transform parent = possesingIngredient.transform.parent;                  // parent ���� ������ IngredientPoint�� ����

                possesingIngredient.transform.parent = null;                              // IngredientPoint�� ��Ʈ ������Ʈ�� ����
                possesingIngredient.transform.localScale = new Vector3(50f, 50f, 50f);    // possesingIngredient�� ���� �������� 50���� ���� = �۷ι� ������
                possesingIngredient.transform.parent = parent;                            // �ٽ� IngredientPoint�� IngredientPoint�� �ʱ�ȭ, Player�� �ڽ� ������Ʈ�� ����

                possesingIngredient.transform.parent = IngredientPoint.transform;
                possesingIngredient.transform.localPosition = Vector3.zero;
                isHavingIngredient = true;
            }
            else if (hitInfo.transform.root.GetComponentInChildren<Tomato>() && Input.GetKey(KeyCode.Space))
            {
                if (isHavingIngredient)
                {
                    Debug.Log("Already Got one");
                    return;
                }

                Debug.Log("Got a tomato");

                GameObject obj = hitInfo.transform.root.GetComponentInChildren<Tomato>().gameObject;     // �������� ���� ���� ���� �ʱ�ȭ�Ͽ� Instantiate�� ���� ������Ʈ�� ���
                possesingIngredient = Instantiate(obj, IngredientPoint.transform.position, IngredientPoint.transform.rotation);


                Transform parent = possesingIngredient.transform.parent;                  // parent ���� ������ IngredientPoint�� ����

                possesingIngredient.transform.parent = null;                              // IngredientPoint�� ��Ʈ ������Ʈ�� ����
                possesingIngredient.transform.localScale = new Vector3(50f, 50f, 50f);    // possesingIngredient�� ���� �������� 50���� ���� = �۷ι� ������
                possesingIngredient.transform.parent = parent;                            // �ٽ� IngredientPoint�� IngredientPoint�� �ʱ�ȭ, Player�� �ڽ� ������Ʈ�� ����

                possesingIngredient.transform.parent = IngredientPoint.transform;
                possesingIngredient.transform.localPosition = Vector3.zero;
                isHavingIngredient = true;
            }
            else if (hitInfo.transform.root.GetComponentInChildren<Egg>() && Input.GetKey(KeyCode.Space))
            {
                if (isHavingIngredient)
                {
                    Debug.Log("Already Got one");
                    return;
                }

                Debug.Log("Got an egg");

                GameObject obj = hitInfo.transform.root.GetComponentInChildren<Egg>().gameObject;     // �������� ���� ���� ���� �ʱ�ȭ�Ͽ� Instantiate�� ���� ������Ʈ�� ���
                possesingIngredient = Instantiate(obj, IngredientPoint.transform.position, IngredientPoint.transform.rotation);


                Transform parent = possesingIngredient.transform.parent;                  // parent ���� ������ IngredientPoint�� ����

                possesingIngredient.transform.parent = null;                              // IngredientPoint�� ��Ʈ ������Ʈ�� ����
                possesingIngredient.transform.localScale = new Vector3(3f, 3f, 3f);       // possesingIngredient�� ���� �������� 50���� ���� = �۷ι� ������
                possesingIngredient.transform.parent = parent;                            // �ٽ� IngredientPoint�� IngredientPoint�� �ʱ�ȭ, Player�� �ڽ� ������Ʈ�� ����

                possesingIngredient.transform.parent = IngredientPoint.transform;
                possesingIngredient.transform.localPosition = Vector3.zero;
                isHavingIngredient = true;
            }
            else if (hitInfo.transform.root.GetComponentInChildren<Flour>() && Input.GetKey(KeyCode.Space))
            {
                if (isHavingIngredient)
                {
                    Debug.Log("Already Got one");
                    return;
                }

                Debug.Log("Got a bag of flour");

                GameObject obj = hitInfo.transform.root.GetComponentInChildren<Flour>().gameObject;     // �������� ���� ���� ���� �ʱ�ȭ�Ͽ� Instantiate�� ���� ������Ʈ�� ���
                possesingIngredient = Instantiate(obj, IngredientPoint.transform.position, IngredientPoint.transform.rotation);


                Transform parent = possesingIngredient.transform.parent;                  // parent ���� ������ IngredientPoint�� ����

                possesingIngredient.transform.parent = null;                              // IngredientPoint�� ��Ʈ ������Ʈ�� ����
                possesingIngredient.transform.localScale = new Vector3(3f, 3f, 3f);       // possesingIngredient�� ���� �������� 50���� ���� = �۷ι� ������
                possesingIngredient.transform.parent = parent;                            // �ٽ� IngredientPoint�� IngredientPoint�� �ʱ�ȭ, Player�� �ڽ� ������Ʈ�� ����

                possesingIngredient.transform.parent = IngredientPoint.transform;
                possesingIngredient.transform.localPosition = Vector3.zero;
                isHavingIngredient = true;
            }
            else if (hitInfo.transform.root.GetComponentInChildren<Potato>() && Input.GetKey(KeyCode.Space))
            {
                if (isHavingIngredient)
                {
                    Debug.Log("Already Got one");
                    return;
                }

                Debug.Log("Got a potato");

                GameObject obj = hitInfo.transform.root.GetComponentInChildren<Potato>().gameObject;     // �������� ���� ���� ���� �ʱ�ȭ�Ͽ� Instantiate�� ���� ������Ʈ�� ���
                possesingIngredient = Instantiate(obj, IngredientPoint.transform.position, IngredientPoint.transform.rotation);


                Transform parent = possesingIngredient.transform.parent;                  // parent ���� ������ IngredientPoint�� ����

                possesingIngredient.transform.parent = null;                              // IngredientPoint�� ��Ʈ ������Ʈ�� ����
                possesingIngredient.transform.localScale = new Vector3(3f, 3f, 3f);       // possesingIngredient�� ���� �������� 50���� ���� = �۷ι� ������
                possesingIngredient.transform.parent = parent;                            // �ٽ� IngredientPoint�� IngredientPoint�� �ʱ�ȭ, Player�� �ڽ� ������Ʈ�� ����

                possesingIngredient.transform.parent = IngredientPoint.transform;
                possesingIngredient.transform.localPosition = Vector3.zero;
                isHavingIngredient = true;
            }

        }
    }

}
