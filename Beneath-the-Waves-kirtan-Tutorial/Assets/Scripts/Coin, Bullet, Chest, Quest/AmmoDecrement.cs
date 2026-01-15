using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDecrement : MonoBehaviour
{
    void Update()
    {
        // Check if the left mouse button is pressed and there are bullets left
        if (Input.GetMouseButtonDown(0) && PlayerAccount.Instance.UseBullet())
        {
            Debug.Log("Bullet shot!");
        }
    }
}

