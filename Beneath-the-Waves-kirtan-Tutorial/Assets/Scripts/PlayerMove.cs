using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController myCC;
    public float walkSpeed = 10f;
    public float runSpeed = 13f;
    public float jumpPower = 10f;

    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    private Vector3 inputVector;
    private Vector3 movementVector;
    [SerializeField]
    private float myGravity = 15f;

    private bool isRunning;

    public HumbleMovementCalc calculator;

    void Start()
    {
        myCC = GetComponent<CharacterController>();
        calculator = GetComponent<HumbleMovementCalc>();
        Debug.Log("PlayerMove script initialized.");
    }

    void Update()
    {
        GetInput();
        MovePlayer();
        Debug.Log($"PlayerMove Update: Position {transform.position}");
    }

    void GetInput()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift) && myCC.isGrounded || isRunning && !myCC.isGrounded;
        inputVector = transform.TransformDirection(calculator.CalcMovement(isRunning, Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), walkSpeed, runSpeed));

        Debug.Log($"GetInput: inputVector {inputVector}");

        float movementDirectionY = movementVector.y;
        if (!myCC.isGrounded)
        {
            if (isRunning)
            {
                movementVector = 0.6f * movementVector + 0.4f * inputVector;
            }
            else
            {
                movementVector = 0.6f * inputVector - 0.6f * (Vector3.up * -myGravity);
            }
        }
        else
        {
            movementVector = inputVector + (Vector3.up * -myGravity);
        }

        if (Input.GetButton("Jump") && myCC.isGrounded)
        {
            movementVector.y = jumpPower;
        }
        else
        {
            movementVector.y = movementDirectionY;
        }

        if (!myCC.isGrounded)
        {
            movementVector.y -= myGravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            myCC.height = crouchHeight;
            runSpeed = crouchSpeed;
            walkSpeed = crouchSpeed;
        }
        else
        {
            myCC.height = defaultHeight;
            walkSpeed = 10f;
            runSpeed = 13f;
        }

        Debug.Log($"GetInput: movementVector {movementVector}");
    }

    void MovePlayer()
    {
        myCC.Move(movementVector * Time.deltaTime);
        Debug.Log($"MovePlayer: Position {transform.position}");
    }
}
