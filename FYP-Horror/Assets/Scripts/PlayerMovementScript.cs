using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public CharacterController characterController;
    private float horizontalInput;
    private float verticalInput;
    private bool canPlayerMove = true;

    public float speed;

    Vector3 moveDirection;

    Vector3 velocity;

    private float gravity = -9.81f;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canPlayerMove == true)
        {
            MyInput();
            movePlayer();

            velocity.y += gravity * Time.deltaTime;

            characterController.Move(velocity * Time.deltaTime);
        }
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

    public void changeCanPlayerMove(bool _moveBool)
    {
        canPlayerMove = _moveBool;
    }
}
