using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    public Vector3 spawnPosition = new Vector3(0, 1, 0); // Set desired spawn position

    void Start()
    {
        // Ensure the player starts at the correct position
        transform.position = spawnPosition;

        // Enable necessary components if they are not enabled
        GetComponent<PlayerMove>().enabled = true;
        GetComponent<MouseLook>().enabled = true;
    }
}
