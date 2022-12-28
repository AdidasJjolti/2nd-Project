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

    GameObject meat;

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
            Debug.Log(hitInfo.transform.parent.name);

            if(hitInfo.transform.parent.name == "LettuceBox")
            {
                Debug.Log("Got a lettuce");
            }
            else if (hitInfo.transform.parent.name == "PorkBox")
            {
                if (isHavingIngredient)
                {
                    Debug.Log("Already Got one");
                    return;
                }

                Debug.Log("Got a piece of meat");

                meat = Instantiate(Ingredients[0], IngredientPoint.transform.position, IngredientPoint.transform.rotation);
                meat.transform.parent = IngredientPoint.transform;
                meat.transform.localPosition = Vector3.zero;
                isHavingIngredient = true;
            }
            else if (hitInfo.transform.parent.name == "TomatoBox")
            {
                Debug.Log("Got a tomato");
            }
            else if (hitInfo.transform.parent.name == "EggBox")
            {
                Debug.Log("Got an egg");
            }
            else if (hitInfo.transform.parent.name == "FlourBox")
            {
                Debug.Log("Got a bag of flour");
            }
            else if (hitInfo.transform.parent.name == "PotatoBox")
            {
                Debug.Log("Got a potato");
            }

        }
    }

}
