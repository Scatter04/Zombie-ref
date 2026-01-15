using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    // References to other components
    public Movement movement; // Reference to the Movement component
    public GameObject camera; // Reference to the player's camera GameObject

    // Player's nickname
    public string nickname;

    // Text display for the nickname
    public TextMeshPro nicknameText;

    // Called when the player is considered the local player
    public void IsLocalPlayer()
    {
        movement.enabled = true; // Enable the Movement component
        camera.SetActive(true); // Activate the camera GameObject
    }

    // Remote procedure call (RPC) to set the player's nickname
    [PunRPC]
    public void SetNickname(string _name)
    {
        nickname = _name; // Set the nickname
        nicknameText.text = nickname; // Update the displayed nickname
    }
}
