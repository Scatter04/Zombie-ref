using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks // Inherits from MonoBehaviourPunCallbacks for networking callbacks
{
  public static RoomManager instance; // Singleton instance for easy access

  public GameObject player; // Prefab for the player object

  [Space]
  public Transform[] spawnPoints; // Array of potential spawn locations for players

  [Space]
  public GameObject roomCam; // Reference to the room camera (likely for pre-game view)

  [Space]
  public GameObject nameUI; // UI element for entering nickname
  public GameObject connectingUI; // UI element for connection progress

  private string nickname = "Unnamed"; // Default player nickname

  void Awake()
  {
    instance = this; // Set this instance as the singleton
  }

  public void ChangeNickname(string _name) // Function to change player nickname
  {
    nickname = _name;
  }

  public void JoinRoomButtonPressed() // Function triggered when "Join Room" button is pressed
  {
    Debug.Log("Connecting..."); // Log message for debugging
    PhotonNetwork.ConnectUsingSettings(); // Initiate connection to Photon server

    nameUI.SetActive(false); // Deactivate nickname UI
    connectingUI.SetActive(true); // Activate connection progress UI
  }



  public override void OnConnectedToMaster() // Callback when connected to Master server
  {
    base.OnConnectedToMaster();
    Debug.Log("Connected to Server");
    PhotonNetwork.JoinLobby(); // Join the Photon lobby
  }

  public override void OnJoinedLobby() // Callback when joined the lobby
  {
    base.OnJoinedLobby();
    PhotonNetwork.JoinOrCreateRoom("test", null, null); // Join or create a room named "test"
    Debug.Log("We are connected and in the lobby");
  }

  public override void OnJoinedRoom() // Callback when joined a room
  {
    base.OnJoinedRoom();
    Debug.Log("We are connected and in the room");

    roomCam.SetActive(false); // Deactivate room camera (likely not needed anymore)

    SpawnPlayer(); // Spawn the player object
  }

  public void SpawnPlayer() // Function to spawn the player object
  {
    Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)]; // Choose a random spawn point

    GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity); // Instantiate the player prefab
    _player.GetComponent<PlayerSetup>().IsLocalPlayer(); // Call IsLocalPlayer() function on spawned player (likely for setting up local player properties)
    _player.GetComponent<Health>().isLocalPlayer = true; // Set health component's isLocalPlayer to true for the local player

    _player.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.AllBuffered, nickname); // Call a remote procedure call (RPC) to set the nickname for all players
    PhotonNetwork.LocalPlayer.NickName = nickname; // Set the nickname for the local player
  }
}
