using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject inventoryPanel;   // Панель инвентаря
    public GameObject pauseMenuPanel;   // Панель паузы

    private bool isInventoryOpen = false;
    private bool isPauseOpen = false;
    public InventoryUI inventoryUI;
    public ContextMenuUI contextMenuUI;
    //public InventoryItemDragHandler currentDragHandler; // текущий объект, который тащится

    // Update is called once per frame
    void Update()
    {
        // Обработка нажатия Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Если инвентарь открыт — закрываем его
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
            // Иначе, если пауза открыта — закрываем паузу
            else if (isPauseOpen)
            {
                ResumeGame();
            }
            // Если ни инвентарь, ни пауза не открыты — открываем меню паузы
            else
            {
                PauseGame();
            }
        }
        // Обработка нажатия Tab для инвентаря
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Если пауза не открыта — переключаем инвентарь
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
            inventoryUI.RefreshUI(); // Жесткий вызов после открытия
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
