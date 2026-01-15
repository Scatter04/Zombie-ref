using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 1.5f;
    public float smoothing = 1.5f;


    private float xMousePos;
    private float smoothedMousePosX;
    private float yMousePos;
    private float smoothedMousePosY;

    public float lookSpeed = 2f;
    public float lookYLimit = 45f;

    private float currentLookPosX;
    private float currentLookPosY;
    Quaternion originalRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        GetInput();
        ModifyInput();
        MovePlayer();
    }

    void GetInput()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.position = new Vector3(0, -0.5f);
        }
        else
        {
            transform.position = new Vector3(0, 0);
        }
        yMousePos = Input.GetAxisRaw("Mouse Y");
        xMousePos = Input.GetAxisRaw("Mouse X");
    }

    void ModifyInput()
    {
        yMousePos *= sensitivity * smoothing;
        smoothedMousePosY = Mathf.Lerp(smoothedMousePosY, yMousePos, 1f / smoothing);
        xMousePos *= sensitivity * smoothing;
        smoothedMousePosX = Mathf.Lerp(smoothedMousePosX, xMousePos, 1f / smoothing);
    }

    void MovePlayer()
    {
        currentLookPosX += smoothedMousePosX;
        currentLookPosY += smoothedMousePosY;
        currentLookPosY = Mathf.Clamp(currentLookPosY, -lookYLimit, lookYLimit);
        Quaternion yQuaternion = Quaternion.AngleAxis(currentLookPosY, Vector3.left);
        Quaternion xQuaternion = Quaternion.AngleAxis(currentLookPosX, Vector3.up);

        //Rotate
        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
    }
}
