using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumbleMovementCalc : MonoBehaviour
{

    public float walkSpeed = 10f;
    public float runSpeed = 13f;
    public Vector3 inputVector;


    public Vector3 CalcMovement(bool isRunning, float h, float v, float ws, float rs)
    {
        walkSpeed = ws;
        runSpeed = rs;
        inputVector = new Vector3(h, 0f, v);
        inputVector.Normalize();
        if (isRunning){
            inputVector *= runSpeed;
        } else
        {
            inputVector *= walkSpeed;
        }
        return inputVector;
    }
}
