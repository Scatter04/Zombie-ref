using System.Collections;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int maxBullets = 5;         // Maximum number of bullets to spawn
    public Vector3 spawnArea;          // Area around the player where bullets can spawn
    public GameObject player;          // Reference to the player GameObject

    private int bulletsSpawned = 0;    // Counter for the number of bullets spawned

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found, check the player tag.");
            this.enabled = false;
            return;
        }

        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab not assigned in the inspector.");
            this.enabled = false;
            return;
        }

        StartCoroutine(SpawnBulletsPeriodically());
    }

    IEnumerator SpawnBulletsPeriodically()
    {
        while (bulletsSpawned < maxBullets)
        {
            SpawnBullet();
            yield return new WaitForSeconds(10); // Wait for 10 seconds before spawning the next bullet
        }
        Debug.Log("Finished spawning bullets");
        this.enabled = false; // Optionally disable this component if no more spawning is needed
    }

    void SpawnBullet()
    {
        if (bulletsSpawned < maxBullets)
        {
            Vector3 spawnPosition = player.transform.position + new Vector3(
                Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
                0,
                Random.Range(-spawnArea.z / 2, spawnArea.z / 2)
            );
            Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            bulletsSpawned++;
            Debug.Log($"Spawning bullet {bulletsSpawned}");
        }
    }

    // Method to spawn bullet triggered by player's firing action
    public void SpawnBulletOnFire()
    {
        if (bulletsSpawned < maxBullets)
        {
            SpawnBullet();
        }
    }
}
