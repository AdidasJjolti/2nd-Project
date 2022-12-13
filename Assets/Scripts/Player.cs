using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController characterController;

    Vector3 moveDirection;
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
     }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(x, 0, z);

        characterController.Move(moveDirection * 10 * Time.deltaTime);
    }
}
