using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private Transform spawnPoint;

    private void Start()
    {
        spawnPoint = GameObject.Find("SpawnPoint").transform;
        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint not found in the scene.");
        }
    }

    public void ReloadScene()
    {
        StartCoroutine(ReloadSceneCoroutine());
    }

    private IEnumerator ReloadSceneCoroutine()
    {
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return new WaitForEndOfFrame();

        Player player = FindObjectOfType<Player>();
        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.position;
            Debug.Log($"Player position after scene reload: {player.transform.position}");
        }
    }
}
