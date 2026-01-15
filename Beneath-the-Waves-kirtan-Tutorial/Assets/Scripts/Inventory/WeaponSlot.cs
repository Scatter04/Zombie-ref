using System.Collections;
using System.Collections.Generic;
using TMPro;
// using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour, IPointerClickHandler
{
    //Weapon data
    public string weaponName;
    public Sprite weaponSprite;
    public bool isFull;
    public Color color;
    public string weaponDescription;

    //Weapon slot
    [SerializeField]
    private Image weaponImage;

    public GameObject selectedShader;
    public bool thisWeaponSelected;
    private InventoryManager inventoryManager;

    //weapon description
    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionName;
    public TMP_Text ItemDescriptionText;

    public void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public void AddWeaponSlot(string weaponName, Sprite weaponSprite, string weaponDescription)
    {
        this.weaponName = weaponName;
        this.weaponSprite = weaponSprite;
        this.weaponDescription = weaponDescription;
        isFull = true;

        weaponImage.sprite = weaponSprite;
        color = weaponImage.color;
        color.a = 100;
        weaponImage.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
    }

    public void OnLeftClick()
    {
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisWeaponSelected = true;
        ItemDescriptionName.text = weaponName;
        ItemDescriptionText.text = weaponDescription;
        itemDescriptionImage.sprite = weaponSprite;
        if (itemDescriptionImage.sprite == null)
        {
            itemDescriptionImage.gameObject.SetActive(false);
        }
        else
        {
            itemDescriptionImage.gameObject.SetActive(true);
        }
    }

    public void DeselectDescription()
    {
        ItemDescriptionName.text = null;
        ItemDescriptionText.text = null;
        itemDescriptionImage.gameObject.SetActive(false);
    }
}
