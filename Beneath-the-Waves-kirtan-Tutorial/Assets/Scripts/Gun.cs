using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [SerializeField]
    private string weaponName;

    [SerializeField]
    private Sprite sprite;

    [TextArea]
    [SerializeField]
    private string weaponDescription;

    private InventoryManager inventoryManager;
    private bool destroyed = false;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && destroyed == false)
        {
            Destroy(gameObject);
            destroyed = true;
            inventoryManager.AddWeapon(weaponName, sprite, weaponDescription);
        }
    }
}
