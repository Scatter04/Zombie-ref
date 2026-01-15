using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    
    public GameObject Inventory;
    private bool menuActivated;

    public WeaponSlot[] weaponSlot;
    public keySlot[] keySlot;

    public bool inventoryAccess = false;

    // Update is called once per frame
    void Update()
    {
        if(inventoryToggle(Input.GetButtonDown("Inventory"), menuActivated, inventoryAccess))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Inventory.SetActive(false);
            menuActivated = false;
            DeselectAllSlots();
            weaponSlot[0].DeselectDescription();
        }
        else if (inventoryToggle(Input.GetButtonDown("Inventory"), !menuActivated, inventoryAccess))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Inventory.SetActive(true);
            menuActivated = true;
        }
    }

    public void AddWeapon(string weaponName, Sprite weaponSprite, string weaponDescription)
    {
        for (int i = 0; i < weaponSlot.Length; i++)
        {
            if (!weaponSlot[i].isFull)
            {
                weaponSlot[i].AddWeaponSlot(weaponName, weaponSprite, weaponDescription);
                return;
            }
        }
    }
    
    public void AddKey(string keyName, Sprite keySprite, string keyDescription)
    {
        for (int i = 0; i < keySlot.Length; i++)
        {
            if (!keySlot[i].isFull)
            {
                keySlot[i].AddKeySlot(keyName, keySprite, keyDescription);
                return;
            }
        }
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < weaponSlot.Length; i++)
        {
            weaponSlot[i].selectedShader.SetActive(false);
        }

        for (int i = 0; i < keySlot.Length; i++)
        {
            keySlot[i].selectedShader.SetActive(false);
        }
    }

    public bool inventoryToggle(bool pressed, bool active, bool access)
    {
        return pressed && active && access;
    }
}
