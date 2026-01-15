using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKitScript : MonoBehaviour
{
    [SerializeField]
    private float heal = 33;

    private Player player;
    private float HP;
    private float MAXHP;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        MAXHP = player.getMAXHP();
        GetComponent<BoxCollider>().enabled = false;
    }

    private void Update()
    {
        HP = player.getHP();
        if (HP < MAXHP)
        {
            GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            player.healDamage(heal);
        }
    }
}
