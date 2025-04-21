using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;       // Префаб слота 
    public GameObject itemPrefab; // Префаб визуального предмета (ItemUI)
    
    public RectTransform slotContainer;     // Родительский объект, куда будут добавляться слоты
    public RectTransform itemContainer;     //Объект куда добавляются ItemUI
    
    public float cellSize = 100f; // Размер слота
    public float spacing = 4f; //Расстояние между слотами
    public int gridWidth = 8; //Ширина ивнвентаря в слотах
    public int gridHeight = 7; //Высота инвентаря в слотах
    
    public InventoryItemDragHandler currentDragHandler;
    public bool IsDraggingItem { get; private set; }

    public ContextMenuUI contextMenuUI;//нов выбрасывание предметов


    public Inventory inventory;

    void Start()
    {      
        if (inventory == null)
        {
            inventory = GetComponent<Inventory>(); // Попробовать найти автоматически
            if (inventory == null)
            {
                Debug.LogError("Inventory не назначен в InventoryUI!");
                return;
            }
        }
        GenerateSlotGrid();
        //RefreshUI();

    }
    void GenerateSlotGrid()
    {
        
        // Создаём сетку серых слотов
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {               
                GameObject slot = Instantiate(slotPrefab, slotContainer);
                RectTransform rt = slot.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(x * (cellSize + spacing), -y * (cellSize + spacing));
                rt.sizeDelta = new Vector2(cellSize, cellSize);


                
            }
        }
    }
    public void RefreshUI()
    {
        Debug.Log("RefreshUI вызван");
        // Удаляем только предметы (а не слоты)
        foreach (Transform child in itemContainer)
        {
            if (child.CompareTag("ItemUI")) // Мы ставим тег только предметам
            {
                Destroy(child.gameObject);
            }

        }

        if (inventory == null)
        {
            Debug.LogError("Inventory пуст в RefreshUI");
            return;
        }

        foreach (var inventoryItem in inventory.GetItems())
        {
            CreateItemUI(inventoryItem);
        }

        if (itemPrefab == null)
        {
            Debug.LogError("ItemPrefab не присвоен в InventoryUI!");
        }
    }
    void CreateItemUI(InventoryItem inventoryItem)
    {
        Debug.Log("Создаётся предмет: " + inventoryItem.item.itemName + ", иконка: " + inventoryItem.item.icon);
        Debug.Log("Создаём UI для: " + inventoryItem.item.itemName); // << сюда

        if (itemPrefab == null)
        {
            Debug.LogError("Item Prefab не назначен!");
            return;
        }

        
        GameObject go = Instantiate(itemPrefab, itemContainer);
        go.tag = "ItemUI";

        RectTransform rect = go.GetComponent<RectTransform>();

        float width = inventoryItem.item.width * (cellSize + spacing) - spacing;
        float height = inventoryItem.item.height * (cellSize + spacing) - spacing;

        rect.sizeDelta = new Vector2(width, height);
        rect.anchoredPosition = new Vector2(
            inventoryItem.position.x * (cellSize + spacing),
            -inventoryItem.position.y * (cellSize + spacing)
        );

        Image image = go.GetComponent<Image>();
        if (image != null && inventoryItem.item.icon != null)
        {
            image.sprite = inventoryItem.item.icon;
        }

        Debug.Log("Добавлен ItemTooltipTrigger для " + inventoryItem.item.itemName);

        var tooltipTrigger = go.AddComponent<ItemTooltipTrigger>();
        tooltipTrigger.inventoryItem = inventoryItem;


        
        var itemUI = go.GetComponent<InventoryItemUI>();
        if (itemUI != null)
        {
            itemUI.inventoryItem = inventoryItem;

            //новый код для выбрасывания предметов
            itemUI.inventoryUI = this;
            itemUI.contextMenuUI = contextMenuUI; // передаём
            
        }
        
        var dragHandler = go.GetComponent<InventoryItemDragHandler>();
        
        if (dragHandler != null)
        {
            dragHandler.inventoryItem = inventoryItem;
        }
    }
    public Vector2 GetSlotWorldPosition(int x, int y)
    {
        float posX = x * (cellSize + spacing);
        float posY = -y * (cellSize + spacing);

        Vector2 localPos = new Vector2(posX, posY);
        return itemContainer.TransformPoint(localPos);
    }
    public Vector2Int GetGridPositionUnderMouse(PointerEventData eventData, InventoryItem draggingItem = null)
    {        
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(itemContainer, eventData.position, eventData.pressEventCamera, out localPoint);

        //Vector2 mousePosition = localPoint;

        float cellFullSize = cellSize + spacing;

        float x = (localPoint.x + (itemContainer.rect.width / 2)) / (cellSize + spacing);
        float y = (-(localPoint.y - (itemContainer.rect.height / 2))) / (cellSize + spacing);

        int gridX = Mathf.FloorToInt(x);
        int gridY = Mathf.FloorToInt(y);

        // Если предмет больше 1x1, нужно сдвинуть координату, чтобы курсор попадал не в край, а в центр предмета
        /*if (draggingItem != null)
        {
            gridX -= draggingItem.item.width / 2;
            gridY -= draggingItem.item.height / 2;
        }*/
        if (draggingItem != null)
        {
            int w = draggingItem.item.width;
            int h = draggingItem.item.height;

            // Сдвигаем позицию чтобы центр был ближе к слоту (магнитный эффект)
            float offsetX = x - gridX;
            float offsetY = y - gridY;

            // Если чётное — значит центр будет между ячеек, надо округлить по магниту
            if (w % 2 == 0)
            {
                if (offsetX > 0.5f) gridX -= w / 2 - 1;
                else gridX -= w / 2;
            }
            else
            {
                gridX -= w / 2;
            }

            if (h % 2 == 0)
            {
                if (offsetY > 0.5f) gridY -= h / 2 - 1;
                else gridY -= h / 2;
            }
            else
            {
                gridY -= h / 2;
            }
        }

        return new Vector2Int(gridX, gridY);

    }
    public void SetDraggingState(bool state)
    {
        IsDraggingItem = state;
    }

    //нов два метода снизу для подсветки слотов
    

}
