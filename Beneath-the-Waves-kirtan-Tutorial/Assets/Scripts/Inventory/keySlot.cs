using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class keySlot : MonoBehaviour, IPointerClickHandler
{
    //Key data
    public string keyName;
    public Sprite keySprite;
    public bool isFull;
    public Color color;
    public string keyDescription;

    //Key slot
    [SerializeField]
    private Image keyImage;

    public GameObject selectedShader;
    public bool thisKeySelected;
    private InventoryManager inventoryManager;

    //key description
    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionName;
    public TMP_Text ItemDescriptionText;

    public void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public void AddKeySlot(string keyName, Sprite keySprite, string keyDescription)
    {
        this.keyName = keyName;
        this.keySprite = keySprite;
        this.keyDescription = keyDescription;
        isFull = true;

        keyImage.sprite = keySprite;
        color = keyImage.color;
        color.a = 40;
        keyImage.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
    }

    public void OnLeftClick()
    {
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisKeySelected = true;
        ItemDescriptionName.text = keyName;
        ItemDescriptionText.text = keyDescription;
        itemDescriptionImage.sprite = keySprite;
        if (itemDescriptionImage.sprite == null)
        {
            itemDescriptionImage.gameObject.SetActive(false);
        }
        else
        {
            itemDescriptionImage.gameObject.SetActive(true);
        }

    }
}
