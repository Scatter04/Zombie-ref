using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceObjectToCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Make the object face the main camera
        transform.LookAt(Camera.main.transform);
    }
}
