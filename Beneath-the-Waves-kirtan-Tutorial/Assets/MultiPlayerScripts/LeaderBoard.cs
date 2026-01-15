using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // For LINQ queries
using Photon.Pun; // For Photon PUN (Photon Unity Networking)
using Photon.Pun.UtilityScripts; // For Photon PUN utility scripts, including player score handling
using TMPro; // For TextMeshPro

public class LeaderBoard : MonoBehaviour
{
    // Public variables accessible from the Unity Inspector
    public GameObject playersHolder; // Holder for the players UI

    [Header("Options")]
    public float refreshRate = 1f; // Refresh rate for the leaderboard in seconds

    [Header("UI")]
    public GameObject[] slots; // Array of UI slots for displaying players
    [Space]
    public TextMeshProUGUI[] scoreTexts; // Array of TextMeshPro UI elements for displaying player scores
    public TextMeshProUGUI[] nameTexts; // Array of TextMeshPro UI elements for displaying player names

    private void Start()
    {
        // Invoke the Refresh method repeatedly at the specified refresh rate
        InvokeRepeating(nameof(Refresh), 1f, refreshRate);
    }

    // Method to refresh the leaderboard
    public void Refresh()
    {
        // Deactivate all slots initially
        foreach (var slot in slots)
        {
            slot.SetActive(false);
        }

        // Get a sorted list of players by score in descending order
        var sortedPlayerList =
            (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();

        int i = 0;
        // Update each slot with player information
        foreach (var player in sortedPlayerList)
        {
            if (i >= slots.Length) break; // Exit if there are more players than slots

            slots[i].SetActive(true); // Activate the slot

            // Set default name if the player's nickname is empty
            if (string.IsNullOrEmpty(player.NickName))
            {
                player.NickName = "unnamed";
            }

            // Update the UI elements with player name and score
            nameTexts[i].text = player.NickName;
            scoreTexts[i].text = player.GetScore().ToString();

            i++;
        }
    }

    private void Update()
    {
        // Show or hide the players holder UI based on the Tab key input
        playersHolder.SetActive(Input.GetKey(KeyCode.Tab));
    }
}
