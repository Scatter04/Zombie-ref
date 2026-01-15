using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    private int currentIndex = -1;
    private string[] instructions =
    {
        "Welcome to Beneath the Waves!",
        "Use WASD to move and mouse to look around.",
        "Press Ctrl to crouch under gaps.",
        "Press SPACEBAR to jump.",
        "Collect the coins and other items by walking into them!",
        "You can access inventory by pressing 'I'.",
        "Left click to shoot the enemies!",
        "Tutorial Complete!"
    };
    public float instructionDelay = 2f;

    private void Start()
    {
        StartCoroutine(ShowTutorialInstructions());
    }

    private IEnumerator ShowTutorialInstructions()
    {
        while (currentIndex < instructions.Length)
        {
            if (currentIndex >= 0) // Skip the delay for the welcome message
                yield return new WaitForSeconds(instructionDelay);

            currentIndex++;
            tutorialText.text = instructions[currentIndex];

            if (!IsTriggered()) // If not triggered, go back to the same instruction
                currentIndex--;
        }

        // Tutorial completed
        tutorialText.text = "Tutorial completed!";
    }
    private bool IsTriggered()
    {
        switch (currentIndex)
        {
            case -1:
                return true;
            case 0:
                return true; // Automatically proceed to next instruction
            case 1:
                return (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));
            case 2:
                return Input.GetKey(KeyCode.LeftControl);
            case 3:
                return Input.GetKey(KeyCode.Space);
            case 4:
                return true;
            case 5:
                return Input.GetKey(KeyCode.I);
            case 6:
                return Input.GetKey(KeyCode.Mouse0);
            case 7:
                return true;

            default:
                return true; // Default to automatically proceed to next instruction
        }
    }
}
