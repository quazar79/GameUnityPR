using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ContextMenuUI : MonoBehaviour
{
    public GameObject menuPanel;
    public Button useButton;
    public Button dropButton;
    
    private InventoryItemUI currentItemUI;
    
    [SerializeField] private Transform dropPoint; //  ��������, �������� ����� �������

    

    public void Show(InventoryItemUI itemUI, Vector2 position)
    {
        currentItemUI = itemUI;
        menuPanel.SetActive(true);
        //menuPanel.transform.position = position;
        Vector2 offset = new Vector2(10f, -10f); // ���� ������ � ����
        Vector2 adjustedPosition = position + offset;
        

        // ����������� � ������� ��� RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            menuPanel.transform.parent as RectTransform,
            adjustedPosition,
            null, // null ���� canvas � ������ Screen Space - Overlay
            out Vector2 localPoint
        );

        menuPanel.GetComponent<RectTransform>().anchoredPosition = localPoint;

        // ��������� ��������
        useButton.onClick.RemoveAllListeners();
        useButton.onClick.AddListener(() => UseItem());

        dropButton.onClick.RemoveAllListeners();
        dropButton.onClick.AddListener(() => OnDropButton());
    }
    private void Update()
    {
        
        if (gameObject.activeSelf && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            if (!IsPointerOverUI())
            {
                Hide();
            }
        }
    }

    private bool IsPointerOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject == gameObject || result.gameObject.transform.IsChildOf(transform))
            {
                return true;
            }
        }

        return false;
    }

    public void Hide()
    {
        currentItemUI = null;
        menuPanel.SetActive(false);
        
    }
    void UseItem()
    {
        Debug.Log("������������ �������: " + currentItemUI.inventoryItem.item.itemName);
        Hide();
    }

    
    public void OnDropButton()
    {

        if (currentItemUI == null) return;

        Item itemToDrop = currentItemUI.inventoryItem.item;

        // ������� �� ���������
        currentItemUI.inventoryUI.inventory.ClearOccupiedSpace(currentItemUI.inventoryItem.position, itemToDrop.width, itemToDrop.height);
        currentItemUI.inventoryUI.inventory.RemoveItem(currentItemUI.inventoryItem);
        Destroy(currentItemUI.gameObject);

        // ����� � ��� ����� �������
        /*if (itemToDrop.worldPrefab != null)
        {
            if (itemToDrop == null || itemToDrop.worldPrefab == null)
            {
                Debug.LogWarning("��� worldPrefab � �������� " + (itemToDrop != null ? itemToDrop.itemName : "null"));
                return;
            }
            // ������� ����� ������� (�������� �� 2 ����� �����)
            Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 2f;
            Quaternion spawnRotation = Quaternion.identity;

            

            GameObject dropped = Instantiate(itemToDrop.worldPrefab, spawnPosition, spawnRotation);

            WorldItem worldItem = dropped.GetComponent<WorldItem>();
            if (worldItem != null)
            {
                worldItem.Initialize(itemToDrop);
            }
        }*/
        // ����� � ��� ��� �������
        if (itemToDrop != null && itemToDrop.worldPrefab != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("Player � ����� 'Player' �� ������!");
                return;
            }

            Vector3 spawnPosition = player.transform.position + Vector3.up * 0.5f;
            Quaternion spawnRotation = Quaternion.identity;

            GameObject dropped = Instantiate(itemToDrop.worldPrefab, spawnPosition, spawnRotation);

            WorldItem worldItem = dropped.GetComponent<WorldItem>();
            if (worldItem != null)
            {
                worldItem.Initialize(itemToDrop);
            }
        }

        Hide(); // ������� ����
    }
}
