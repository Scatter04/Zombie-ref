using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestHover : MonoBehaviour
{
    public TextMeshProUGUI hoverLabel;
    public float hoverDistance = 10f;
    private Transform playerTransform;
    private Camera mainCamera;

    private void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (hoverLabel != null)
        {
            hoverLabel.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerTransform == null || mainCamera == null || hoverLabel == null)
        {
            Debug.LogError("Missing reference in ChestHover script.");
            return;
        }

        float distance = Vector3.Distance(playerTransform.position, transform.position);
        if (distance <= hoverDistance)
        {
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    Vector3 labelPosition = transform.position + Vector3.up * 0.8f;
                    hoverLabel.transform.position = labelPosition;
                    hoverLabel.transform.LookAt(mainCamera.transform);
                    hoverLabel.transform.Rotate(0, 180, 0);
                    hoverLabel.gameObject.SetActive(true);
                    return;
                }
            }
        }

        hoverLabel.gameObject.SetActive(false);
    }
}