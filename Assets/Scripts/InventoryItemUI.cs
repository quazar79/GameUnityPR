using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryItemUI : MonoBehaviour, IPointerClickHandler
{
    public Image iconImage;
    public RectTransform rectTransform;
    public InventoryItem inventoryItem; 
    //новый код для выбрасывания предметов 
    public InventoryUI inventoryUI;
    public ContextMenuUI contextMenuUI;

    public void Initialize(Sprite icon, int width, int height, InventoryItem itemData, ContextMenuUI contextMenu)
    {
        iconImage.sprite = icon;
        inventoryItem = itemData;
        contextMenuUI = contextMenu;

        // Масштабируем под размер предмета в слотах
        RectTransform rt = GetComponent<RectTransform>();
        float slotSize = 100f; // или возьми из Inventory

        rt.sizeDelta = new Vector2(width * slotSize, height * slotSize);
    }
    // новый код для выбрасывания предметов
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            contextMenuUI.Show(this, eventData.position);
        }
    }
}
