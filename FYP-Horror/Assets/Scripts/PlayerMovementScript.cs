using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public CharacterController characterController;
    private float horizontalInput;
    private float verticalInput;

    public float speed;

    Vector3 moveDirection;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        movePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void movePlayer()
    {
        moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;

        characterController.Move(moveDirection * speed * Time.deltaTime);
    }
}
