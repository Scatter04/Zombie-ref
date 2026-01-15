using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // For Photon PUN (Photon Unity Networking)
using Photon.Pun.UtilityScripts; // For Photon PUN utility scripts, including player score handling
using TMPro; // For TextMeshPro

public class NewWeapons : MonoBehaviour
{
    // Public variables accessible from the Unity Inspector
    public int damage; // Damage dealt by the weapon
    public Camera camera; // Reference to the player's camera
    public float fireRate; // Rate of fire

    [Header("VFX")]
    public GameObject hitVFX; // Visual effect for hits

    private float nextFire; // Time until next shot can be fired

    [Header("Ammo")]
    public int mag = 5; // Number of magazines
    public int ammo = 30; // Current ammo in magazine
    public int magAmmo = 30; // Maximum ammo in a magazine

    [Header("UI")]
    public TextMeshProUGUI magText; // TextMeshPro element for magazine count
    public TextMeshProUGUI ammoText; // TextMeshPro element for ammo count

    [Header("Animation")]
    public Animation animation; // Animation component for reload animation
    public AnimationClip reload; // Reload animation clip

    void Start()
    {
        // Initialize UI text elements with current ammo and magazine count
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }

    void Update()
    {
        // Decrease the nextFire timer
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }

        // Check for firing input and conditions
        if (Input.GetButton("Fire1") && nextFire <= 0 && ammo > 0 && animation.isPlaying == false)
        {
            // Set nextFire timer based on fire rate
            nextFire = 1 / fireRate;

            // Decrease ammo count
            ammo--;

            // Update UI text elements
            magText.text = mag.ToString();
            ammoText.text = ammo + "/" + magAmmo;

            // Call the Fire method
            Fire();
        }

        // Check for reload input
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    void Reload()
    {
        // Play the reload animation
        animation.Play(reload.name);
        if (mag > 0)
        {
            // Decrease magazine count and refill ammo
            mag--;
            ammo = magAmmo;
        }

        // Update UI text elements
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }

    void Fire()
    {
        // Create a ray from the camera's position in the forward direction
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        RaycastHit hit;
        
        // Just for testing, add score to the local player
        PhotonNetwork.LocalPlayer.AddScore(1);

        // Check if the ray hits an object within 100 units
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            // Instantiate hit visual effect at the hit point
            PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);

            // Check if the hit object has a Health component
            if (hit.transform.gameObject.GetComponent<Health>())
            {
                // Add score for hitting the target
                PhotonNetwork.LocalPlayer.AddScore(damage);

                // Add bonus score if the hit results in a kill
                if (damage > hit.transform.gameObject.GetComponent<Health>().health)
                {
                    PhotonNetwork.LocalPlayer.AddScore(100);
                }

                // Call the TakeDamage method on the hit object's Health component
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
            }
        }
    }
}
