using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventoryItem inventoryItem;

    private GameObject dragIcon;
    private RectTransform dragIconRect;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private InventoryUI inventoryUI; // ссылка на UI
    public ContextMenuUI contextMenuUI;
    private Vector2Int originalPosition;


    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        inventoryUI = GetComponentInParent<InventoryUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        originalPosition = inventoryItem.position;
        

        if (inventoryItem == null || inventoryItem.item == null)
            return;

        
        var tooltipTrigger = GetComponent<ItemTooltipTrigger>();
        if (tooltipTrigger != null)
            tooltipTrigger.CancelTooltip();

        // Сделаем оригинал полупрозрачным
        if (canvasGroup != null)
            canvasGroup.alpha = 0.3f;

        // Создаем иконку для перетаскивания
        dragIcon = new GameObject("DraggingIcon");
        dragIcon.transform.SetParent(transform.root, false);
        dragIcon.transform.SetAsLastSibling();


        Image iconImage = dragIcon.AddComponent<Image>();
        iconImage.sprite = inventoryItem.item.icon;
        iconImage.raycastTarget = false;

        RectTransform rectTransform = dragIcon.GetComponent<RectTransform>();
        rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;

        dragIconRect = transform.root as RectTransform;

        UpdateDragPosition(eventData);
        originalPosition = inventoryItem.position;
        inventoryUI.SetDraggingState(true);
        inventoryUI.currentDragHandler = this;
        if (contextMenuUI == null)
            contextMenuUI = FindAnyObjectByType<ContextMenuUI>();

        if (contextMenuUI != null)
            contextMenuUI.Hide();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag...");
        if (dragIcon != null)
        {
            UpdateDragPosition(eventData);
        }
        if (dragIconRect != null)
            dragIconRect.position = Input.mousePosition;
        // Добавим проверку и подсветку
        /*Vector2Int hoverPosition = inventoryUI.GetGridPositionUnderMouse(eventData, inventoryItem);
        bool canPlace = inventoryUI.inventory.CanPlaceItemAt(
            hoverPosition.x,
            hoverPosition.y,
            inventoryItem.item.width,
            inventoryItem.item.height
        );*/

        //inventoryUI.ClearHighlights();
        //inventoryUI.HighlightSlots(hoverPosition, inventoryItem.item.width, inventoryItem.item.height, canPlace);

        //ниже нов подсветка слотов 
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag"); 
        if (dragIcon != null)
        {
            Destroy(dragIcon);
        }
        
        // Вернуть прозрачность обратно
        if (canvasGroup != null)
            canvasGroup.alpha = 1f;

        Vector2Int dropPosition = inventoryUI.GetGridPositionUnderMouse(eventData, inventoryItem);
        Inventory inventory = inventoryUI.inventory;

        // Удалим предмет с предыдущего места
        Vector2Int originalPosition = inventoryItem.position;
        //inventory.ClearOccupiedSpace(inventoryItem);
        inventory.ClearOccupiedSpace(originalPosition, inventoryItem.item.width, inventoryItem.item.height);
        bool isInsideGrid =
        dropPosition.x >= 0 &&
        dropPosition.y >= 0 &&
        dropPosition.x + inventoryItem.item.width <= inventory.gridWidth &&
        dropPosition.y + inventoryItem.item.height <= inventory.gridHeight;


        if (isInsideGrid && inventory.CanPlaceItemAt(dropPosition.x, dropPosition.y, inventoryItem.item.width, inventoryItem.item.height))
        {
            
            inventoryItem.position = dropPosition;
            inventory.MarkPositionAsOccupied(dropPosition.x, dropPosition.y, inventoryItem.item.width, inventoryItem.item.height);

        }
        else
        {
            // Вернём обратно
            inventoryItem.position = originalPosition;
            inventory.MarkPositionAsOccupied(originalPosition.x, originalPosition.y, inventoryItem.item.width, inventoryItem.item.height);
        }

        // Обнови интерфейс
        inventoryUI.RefreshUI();
        inventoryUI.SetDraggingState(false);
        

    }
    public void CancelDrag()
    {
        if (dragIcon != null)
        {
            Destroy(dragIcon);
            dragIcon = null;

            // Вернём предмет на исходную позицию
            inventoryItem.position = originalPosition;
            inventoryUI.inventory.MarkPositionAsOccupied(
                originalPosition.x,
                originalPosition.y,
                inventoryItem.item.width,
                inventoryItem.item.height
            );

            inventoryUI.RefreshUI();
            
        }
    }
    private void UpdateDragPosition(PointerEventData eventData)
    {
        RectTransform rt = dragIcon.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(dragIconRect, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = dragIconRect.rotation;
        }

    }

}
