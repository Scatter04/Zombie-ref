using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMissle : MonoBehaviour
{
    public float forwardSpeed = 0.025f;
    private GameObject Target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            transform.LookAt(Target.transform.position);
            transform.position += transform.forward * forwardSpeed;
        }
    }

    void setTarget(GameObject player)
    {
        Target = player;
    }
}
