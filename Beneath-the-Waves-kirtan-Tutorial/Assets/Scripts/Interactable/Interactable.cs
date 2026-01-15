using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool useEvents;

    [SerializeField]
    public string promptMessage;

    //base call for the player interact script to use
    public void baseInteract()
    {
        if (useEvents)
        {
            
        }
        interact();
    }

    //empty method for the subclasses to implement
    protected virtual void interact()
    {

    }
}
