using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveButton : Interactable
{
    [SerializeField]
    private GameObject steps;
    private bool stepsActive = false;

    //override interact method
    //changes animation state to open door

    protected override void interact()
    {
        stepsActive = true;
        steps.GetComponent<Animator>().SetBool("StepsActive", stepsActive);
        promptMessage = "Sounds like something has moved";
    }
}
