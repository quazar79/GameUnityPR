using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;       // ������ ����� 
    public GameObject itemPrefab; // ������ ����������� �������� (ItemUI)
    
    public RectTransform slotContainer;     // ������������ ������, ���� ����� ����������� �����
    public RectTransform itemContainer;     //������ ���� ����������� ItemUI
    
    public float cellSize = 100f; // ������ �����
    public float spacing = 4f; //���������� ����� �������
    public int gridWidth = 8; //������ ���������� � ������
    public int gridHeight = 7; //������ ��������� � ������
    
    public InventoryItemDragHandler currentDragHandler;
    public bool IsDraggingItem { get; private set; }

    public ContextMenuUI contextMenuUI;//��� ������������ ���������


    public Inventory inventory;

    void Start()
    {      
        if (inventory == null)
        {
            inventory = GetComponent<Inventory>(); // ����������� ����� �������������
            if (inventory == null)
            {
                Debug.LogError("Inventory �� �������� � InventoryUI!");
                return;
            }
        }
        GenerateSlotGrid();
        //RefreshUI();

    }
    void GenerateSlotGrid()
    {
        
        // ������ ����� ����� ������
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
        Debug.Log("RefreshUI ������");
        // ������� ������ �������� (� �� �����)
        foreach (Transform child in itemContainer)
        {
            if (child.CompareTag("ItemUI")) // �� ������ ��� ������ ���������
            {
                Destroy(child.gameObject);
            }

        }

        if (inventory == null)
        {
            Debug.LogError("Inventory ���� � RefreshUI");
            return;
        }

        foreach (var inventoryItem in inventory.GetItems())
        {
            CreateItemUI(inventoryItem);
        }

        if (itemPrefab == null)
        {
            Debug.LogError("ItemPrefab �� �������� � InventoryUI!");
        }
    }
    void CreateItemUI(InventoryItem inventoryItem)
    {
        Debug.Log("�������� �������: " + inventoryItem.item.itemName + ", ������: " + inventoryItem.item.icon);
        Debug.Log("������ UI ���: " + inventoryItem.item.itemName); // << ����

        if (itemPrefab == null)
        {
            Debug.LogError("Item Prefab �� ��������!");
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

        Debug.Log("�������� ItemTooltipTrigger ��� " + inventoryItem.item.itemName);

        var tooltipTrigger = go.AddComponent<ItemTooltipTrigger>();
        tooltipTrigger.inventoryItem = inventoryItem;


        
        var itemUI = go.GetComponent<InventoryItemUI>();
        if (itemUI != null)
        {
            itemUI.inventoryItem = inventoryItem;

            //����� ��� ��� ������������ ���������
            itemUI.inventoryUI = this;
            itemUI.contextMenuUI = contextMenuUI; // �������
            
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

        // ���� ������� ������ 1x1, ����� �������� ����������, ����� ������ ������� �� � ����, � � ����� ��������
        /*if (draggingItem != null)
        {
            gridX -= draggingItem.item.width / 2;
            gridY -= draggingItem.item.height / 2;
        }*/
        if (draggingItem != null)
        {
            int w = draggingItem.item.width;
            int h = draggingItem.item.height;

            // �������� ������� ����� ����� ��� ����� � ����� (��������� ������)
            float offsetX = x - gridX;
            float offsetY = y - gridY;

            // ���� ������ � ������ ����� ����� ����� �����, ���� ��������� �� �������
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

    //��� ��� ������ ����� ��� ��������� ������
    

}
