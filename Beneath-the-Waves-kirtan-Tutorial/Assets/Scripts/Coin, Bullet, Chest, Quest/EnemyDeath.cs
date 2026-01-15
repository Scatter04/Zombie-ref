using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public GameObject coinPrefab;
    // Event for when an enemy is killed
    public static event System.Action OnEnemyKilled; 
    // Flag to track if the enemy is already dead
    private bool isDead = false; 

    private void OnDestroy()
    {
        if (!isDead)
        {
            Die();
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            SpawnCoin();
            TriggerOnEnemyKilled();
        }
    }

    private void SpawnCoin()
    {
        Instantiate(coinPrefab, transform.position, Quaternion.identity);
    }

    private void TriggerOnEnemyKilled()
    {
        // Set the flag to indicate that the enemy is dead
        isDead = true; 
        OnEnemyKilled?.Invoke();
    }
}
