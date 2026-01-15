using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDrops : MonoBehaviour
{
    public int bulletsToAdd = 5; // Number of bullets this pickup gives

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision detected with: " + other.gameObject.name);

        if (other.gameObject.tag == "PlayerModel")
        {
            // Destroy this bullet pickup object after adding bullets to inventory
            Destroy(gameObject);

            Debug.Log("Player collision detected");

            // Add bullets to the player's inventory via PlayerAccount
            PlayerAccount.Instance.AddBullets(bulletsToAdd);


        }
    }
}

