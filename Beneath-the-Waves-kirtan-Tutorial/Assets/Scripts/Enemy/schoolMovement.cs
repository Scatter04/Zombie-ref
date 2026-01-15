using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class schoolMovement : MonoBehaviour
{
    public float turnSpeed = 45.0f;
    public float forwardSpeed = 0.1f;
    public float smoothing = 0.8f;
    private float previousTurn = 0.0f;
    private int health = 5;

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 50f; // Make the projectile faster
    public float attackInterval = 2f;

    private Transform player;
    private float attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        attackTimer = attackInterval;
    }

    // Update is called once per frame
    void Update()
    {
        // Lock onto the player
        if (player != null)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        }

        previousTurn = previousTurn * smoothing + turnSpeed * (Random.value * 2.0f - 1.0f) * (1 - smoothing);
        transform.Rotate(0.0f, previousTurn, 0.0f, Space.Self);
        transform.position += transform.forward * forwardSpeed;

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            ShootProjectile();
            attackTimer = attackInterval;
        }
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        transform.localScale = transform.localScale * (1f - ((5f - health) * 0.2f));
        Debug.Log(health);
        Debug.Log((1f - ((5f - health) * 0.2f)));
        if (health == 1)
        {
            Destroy(gameObject);
        }
    }

    private void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null && player != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 direction = (player.position - firePoint.position).normalized;
                rb.linearVelocity = direction * projectileSpeed;

                // Ignore collision with the enemy's own colliders
                Collider[] enemyColliders = GetComponentsInChildren<Collider>();
                Collider projectileCollider = projectile.GetComponent<Collider>();
                foreach (Collider collider in enemyColliders)
                {
                    Physics.IgnoreCollision(projectileCollider, collider);
                }
            }
            else
            {
                Debug.LogError("Projectile prefab is missing Rigidbody component.");
            }
        }
    }
}
