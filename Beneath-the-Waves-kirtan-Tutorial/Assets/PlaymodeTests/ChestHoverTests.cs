using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class ChestHoverTests
{
    private GameObject player;
    private GameObject chest;
    private GameObject canvas;
    private TextMeshProUGUI hoverLabel;
    private ChestHover chestHover;

    [SetUp]
    public void SetUp()
    {
        // Create and set up the player object
        player = new GameObject("Player");
        player.tag = "Player";
        player.transform.position = Vector3.zero;

        // Create and set up the chest object
        chest = new GameObject("Chest");
        chest.AddComponent<BoxCollider>();
        chestHover = chest.AddComponent<ChestHover>();
        chest.transform.position = new Vector3(0, 0, 2); // 2 units in front of the player

        // Create and set up the canvas
        canvas = new GameObject("Canvas");
        Canvas canvasComponent = canvas.AddComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.WorldSpace;

        // Create and set up the hover label
        GameObject labelObject = new GameObject("HoverLabel");
        hoverLabel = labelObject.AddComponent<TextMeshProUGUI>();
        labelObject.transform.SetParent(canvas.transform);
        hoverLabel.text = "Press 'E' to open";
        hoverLabel.gameObject.SetActive(false);

        // Link the hover label to the ChestHover script
        chestHover.hoverLabel = hoverLabel;
        chestHover.hoverDistance = 3f;

        // Ensure playerTransform is set
        chestHover.GetType().GetField("playerTransform", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(chestHover, player.transform);

        // Create a camera and set it as the main camera
        GameObject cameraObject = new GameObject("Main Camera");
        Camera camera = cameraObject.AddComponent<Camera>();
        cameraObject.tag = "MainCamera"; // Tag the camera object as MainCamera
        chestHover.GetType().GetField("mainCamera", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(chestHover, camera);
    }

    [UnityTest]
    public IEnumerator HoverLabelAppearsWhenCloseAndLookingAtChest()
    {
        // Move the player close to the chest
        player.transform.position = new Vector3(0, 0, 1.5f);

        // Wait for one frame to ensure the Start method is called
        yield return null;

        // Simulate the Update loop
        chestHover.Invoke("Update", 0f);

        // Assert that the hover label is active
        Assert.IsTrue(hoverLabel.gameObject.activeSelf);

        yield return null;
    }

    [UnityTest]
    public IEnumerator HoverLabelDisappearsWhenFarFromChest()
    {
        // Move the player far from the chest
        player.transform.position = new Vector3(0, 0, 5f);

        // Wait for one frame to ensure the Start method is called
        yield return null;

        // Simulate the Update loop
        chestHover.Invoke("Update", 0f);

        // Assert that the hover label is inactive
        Assert.IsFalse(hoverLabel.gameObject.activeSelf);

        yield return null;
    }
}
