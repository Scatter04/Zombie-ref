using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // Namespace for Photon PUN (Photon Unity Networking)
using TMPro; // Namespace for TextMeshPro

public class Health : MonoBehaviour
{
    // Public variables accessible from the Unity Inspector
    public int health; // The player's health
    public bool isLocalPlayer; // Flag to check if this is the local player

    [Header("UI")]
    public TextMeshProUGUI healthText; // Reference to the TextMeshPro UI element that displays health

    // Photon PUN RPC (Remote Procedure Call) method to take damage
    [PunRPC]
    public void TakeDamage(int _damage)
    {
        // Decrease health by the damage amount
        health -= _damage;

        // Update the health UI text
        healthText.text = health.ToString();

        // Check if health is less than or equal to 0
        if (health <= 0)
        {
            // If this is the local player, respawn
            if (isLocalPlayer)
            {
                RoomManager.instance.SpawnPlayer(); // Call to respawn the player
            }
            // Destroy the game object
            Destroy(gameObject);
        }
    }
}
