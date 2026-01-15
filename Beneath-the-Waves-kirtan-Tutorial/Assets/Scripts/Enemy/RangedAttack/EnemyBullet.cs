using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private void Start()
    {
        // Ignore collision with the enemy
        Collider[] enemyColliders = GameObject.FindGameObjectWithTag("Enemy").GetComponentsInChildren<Collider>();
        foreach (Collider collider in enemyColliders)
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collider);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;
        if (hitTransform.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            hitTransform.GetComponent<Player>().takeDamage(10);
        }
        Destroy(gameObject);
    }
}
