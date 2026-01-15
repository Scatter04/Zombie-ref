using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MainMenuTest
{
    private GameObject mainMenuGameObject;
    private MainMenu mainMenu;

    [SetUp]
    public void Setup()
    {
        // Create a new GameObject and add the MainMenu component to it
        mainMenuGameObject = new GameObject();
        mainMenu = mainMenuGameObject.AddComponent<MainMenu>();
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up after each test
        Object.Destroy(mainMenuGameObject);
    }

    [UnityTest]
    public IEnumerator PlayGame_LoadsNextScene()
    {
        // Arrange: Ensure the test starts with the first scene
        SceneManager.LoadScene(0);
        yield return new WaitForSeconds(1);  // Allow time for the scene to load

        Debug.Log($"Current scene before PlayGame: {SceneManager.GetActiveScene().buildIndex}");

        // Act: Call the PlayGame method
        mainMenu.PlayGame();
        yield return new WaitForSeconds(1);  // Allow time for the scene transition

        Debug.Log($"Current scene after PlayGame: {SceneManager.GetActiveScene().buildIndex}");

        // Assert: Check if the next scene is loaded
        Assert.AreEqual(1, SceneManager.GetActiveScene().buildIndex, $"Expected scene index: 1, but got: {SceneManager.GetActiveScene().buildIndex}");
    }

    [UnityTest]
    public IEnumerator MultiPlayer_LoadsMultiplayerScene()
    {
        // Arrange: Ensure the test starts with the first scene
        SceneManager.LoadScene(0);
        yield return new WaitForSeconds(1);  // Allow time for the scene to load

        Debug.Log($"Current scene before MultiPlayer: {SceneManager.GetActiveScene().buildIndex}");

        // Act: Call the MultiPlayer method
        mainMenu.MultiPlayer();
        yield return new WaitForSeconds(1);  // Allow time for the scene transition

        Debug.Log($"Current scene after MultiPlayer: {SceneManager.GetActiveScene().buildIndex}");

        // Assert: Check if the multiplayer scene is loaded
        Assert.AreEqual(2, SceneManager.GetActiveScene().buildIndex, $"Expected scene index: 2, but got: {SceneManager.GetActiveScene().buildIndex}");
    }

    [Test]
    public void QuitGame_LogsQuitMessage()
    {
        // Arrange: Use a LogAssert to capture logs
        LogAssert.Expect(LogType.Log, "Quit");

        // Act: Call the QuitGame method
        mainMenu.QuitGame();

        // Assert: Check if the quit message was logged
        LogAssert.NoUnexpectedReceived();
    }
}
