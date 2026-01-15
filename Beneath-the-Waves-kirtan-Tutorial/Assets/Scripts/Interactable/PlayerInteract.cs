using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;


    // Start is called before the first frame update
    void Start()
    {
        playerUI = GetComponent<PlayerUI>();

        //initailise prompt text as an empty string
        playerUI.updateText(string.Empty);
    }

    // Update is called once per frame
    void Update()
    {
        //create a ray at the center of the camera, shooting outwards
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);

        //create variable to store information of what comes into contact with ray
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if(hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.updateText(interactable.promptMessage);
                if (Input.GetButtonDown("Interact"))
                {
                    interactable.baseInteract();
                }
            }
        }
        else
        {
            playerUI.updateText(string.Empty);
        }
    }
}
