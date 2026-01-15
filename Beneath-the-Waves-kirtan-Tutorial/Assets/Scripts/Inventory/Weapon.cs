using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //bullet shooting delay
    public bool isShooting, readytoShoot;
    bool allowReset = true;
    public float shootingDelay = 1f;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 25;
    public float bulletLife = 3f;
    public int weaponDamage;

    //variables for gun animation

    private BoxCollider gunTrigger;
    public float range = 20f;
    public float spread = 1f;

    public ParticleSystem muzzleFlash;
    public Animator anims;

    //Audio vars
    public AudioSource src;
    public AudioClip fireSound;


    public void Awake()
    {
        readytoShoot = true;

    }
    // Start is called before the first frame update
    void Start()
    {
        gunTrigger = GetComponent<BoxCollider>();
        gunTrigger.size = new Vector3(spread, spread, range);
        gunTrigger.center = new Vector3(0, 0, 0.5f * range);
    }

    // Update is called once per frame
    void Update()
    {
        isShooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (readytoShoot && isShooting)
        {
            FireWeapon();
        }

    }

    private void FireWeapon()
    {
        readytoShoot = false;

        src.clip = fireSound;
        src.Play();

        anims.ResetTrigger("Firing");
        anims.SetTrigger("Firing");
        muzzleFlash.Play();


        //Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        //setting the damage
        Bullet bull = bullet.GetComponent<Bullet>();
        bull.damage = weaponDamage;
        //shoot
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletSpeed, ForceMode.Impulse);

        //destroy the bullet aftyer bulletlife
        StartCoroutine(DestroyBullet(bullet, bulletLife));

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }
    }

    private void ResetShot()
    {
        readytoShoot = true;
        allowReset = true;
    }

    private IEnumerator DestroyBullet(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
