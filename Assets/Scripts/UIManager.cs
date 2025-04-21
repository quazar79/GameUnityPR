using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject inventoryPanel;   // ������ ���������
    public GameObject pauseMenuPanel;   // ������ �����

    private bool isInventoryOpen = false;
    private bool isPauseOpen = false;
    public InventoryUI inventoryUI;
    public ContextMenuUI contextMenuUI;
    //public InventoryItemDragHandler currentDragHandler; // ������� ������, ������� �������

    // Update is called once per frame
    void Update()
    {
        // ��������� ������� Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ���� ��������� ������ � ��������� ���
            if (isInventoryOpen)
            {
                CloseInventory();
                TooltipUI.Instance.HideTooltip();
                contextMenuUI.Hide();
                if (inventoryUI.currentDragHandler != null)
                {
                    inventoryUI.currentDragHandler.CancelDrag();
                    inventoryUI.currentDragHandler = null;
                }
            }
            // �����, ���� ����� ������� � ��������� �����
            else if (isPauseOpen)
            {
                ResumeGame();
            }
            // ���� �� ���������, �� ����� �� ������� � ��������� ���� �����
            else
            {
                PauseGame();
            }
        }
        // ��������� ������� Tab ��� ���������
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // ���� ����� �� ������� � ����������� ���������
            if (!isPauseOpen)
            {
                ToggleInventory();
            }
        }       
    }
    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);
        UpdateCursorAndTime(isInventoryOpen);

        if (isInventoryOpen)
        {
            inventoryUI.RefreshUI(); // ������� ����� ����� ��������
        }
        else
        {
            TooltipUI.Instance.HideTooltip();
            contextMenuUI.Hide();
            if (inventoryUI.currentDragHandler != null)
            {
                inventoryUI.currentDragHandler.CancelDrag();
                inventoryUI.currentDragHandler = null;
            }
        }
    }

    public void CloseInventory()
    {
        isInventoryOpen = false;
        inventoryPanel.SetActive(false);
        UpdateCursorAndTime(false);

        
        

    }

    void PauseGame()
    {
        isPauseOpen = true;
        pauseMenuPanel.SetActive(true);
        UpdateCursorAndTime(true);
    }

    public void ResumeGame()
    {
        isPauseOpen = false;
        pauseMenuPanel.SetActive(false);
        UpdateCursorAndTime(false);
    }
    void UpdateCursorAndTime(bool uiOpen)
    {
        if (uiOpen)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
