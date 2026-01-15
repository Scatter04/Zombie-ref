using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision detected with: " + collision.gameObject.name);

        // Check if the collided object or its parent has the Enemy script
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy == null && collision.transform.parent != null)
        {
            enemy = collision.transform.parent.GetComponent<Enemy>();
        }

        if (enemy != null)
        {
            Debug.Log("Hit Enemy: " + collision.gameObject.name);
            enemy.takeDamage(damage);
            Destroy(gameObject);
        }

        //test
        ShootingEnemy shootingEnemy = collision.gameObject.GetComponent<ShootingEnemy>();
        if (shootingEnemy == null && collision.transform.parent != null)
        {
            shootingEnemy = collision.transform.parent.GetComponent<ShootingEnemy>();
        }

        if (shootingEnemy != null)
        {
            Debug.Log("Hit Enemy: " + collision.gameObject.name);
            shootingEnemy.takeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Target"))
        {
            // Test if the bullet hits
            Debug.Log("Hit Target: " + collision.gameObject.name);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Hit Wall: " + collision.gameObject.name);
            Destroy(gameObject);
        }
    }
}
