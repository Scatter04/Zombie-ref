using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    // Variable Declaration
    public Quest chestQuest;
    public GameObject chestLid;
    public float openAngle = -60f;
    public float closeAngle = 0f;
    public float animationDuration = 1f;
    private bool isOpen = false;
    private bool isQuestAccepted = false;
    private bool isPlayerInRange = false;

    // Start Quest
    void Start()
    {
        // Create a new quest with a description and reward
        chestQuest = new Quest("Defeat the enemy", 50);
    }

    
    void Update()
    {
        // Check for player input and if the player is in range
        if (Input.GetKeyDown(KeyCode.E) && IsPlayerInRange())
        {
            if (isOpen)
            {
                // If the chest is open, close it
                CloseChest();
            }
            else
            {
                // If the chest is closed, open it
                OpenChest();
                if (!isQuestAccepted)
                {
                    // Start the quest if it hasn't been accepted yet
                    QuestManager.Instance.StartQuest(chestQuest);
                    isQuestAccepted = true;
                }
            }
        }
    }

    // Chest Opening
    void OpenChest()
    {
        // Start the animation to open the chest
        StartCoroutine(AnimateChest(openAngle));
        isOpen = true;
        // Show the quest window in the UI
        UIManager.Instance.ShowQuestWindow(chestQuest);
    }

    // Chest Closing
    void CloseChest()
    {
        // Start the animation to close the chest
        StartCoroutine(AnimateChest(closeAngle));
        isOpen = false;
        // Hide the quest window in the UI
        UIManager.Instance.HideQuestWindow();
    }

    // Manages chest animated e.g. chest lid
    IEnumerator AnimateChest(float targetAngle)
    {
        // Animate the chest lid to the target angle over the specified duration
        float currentAngle = chestLid.transform.localEulerAngles.x;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float angle = Mathf.LerpAngle(currentAngle, targetAngle, elapsedTime / animationDuration);
            chestLid.transform.localEulerAngles = new Vector3(angle, chestLid.transform.localEulerAngles.y, chestLid.transform.localEulerAngles.z);
            yield return null;
        }
        // Ensure the lid reaches the target angle
        chestLid.transform.localEulerAngles = new Vector3(targetAngle, chestLid.transform.localEulerAngles.y, chestLid.transform.localEulerAngles.z);
    }

    // Checks if player within range
    bool IsPlayerInRange()
    {
        return isPlayerInRange;
    }

    void OnTriggerEnter(Collider other)
    {
        // Set player in range if the player enters the trigger area
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Set player out of range if the player exits the trigger area
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
