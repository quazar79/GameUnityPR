using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;


public class ItemTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    

    public InventoryItem inventoryItem;

    private float hoverTime = 0f;
    private bool isHovering = false;
    private float delay = 1f; // Задержка в секундах
    private bool tooltipShown = false;
    
    private Vector2 lastMousePosition;
    private float movementThreshold = 3f; // Минимальное движение мыши для сброса

    void Update()
    {
        if (isHovering)
        {
            float movement = Vector2.Distance(Input.mousePosition, lastMousePosition);
            lastMousePosition = Input.mousePosition;

            if (movement > movementThreshold)
            {
                // Курсор двинулся — скрываем тултип и сбрасываем всё
                TooltipUI.Instance.HideTooltip();
                hoverTime = 0f;
                tooltipShown = false;
                return;
            }

            if (!tooltipShown)
            {
                hoverTime += Time.unscaledDeltaTime;

                if (hoverTime >= delay)
                {
                    TooltipUI.Instance.ShowTooltip(inventoryItem);
                    tooltipShown = true;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Проверка: если сейчас идёт перетаскивание предмета — ничего не делаем
        InventoryUI inventoryUI = GetComponentInParent<InventoryUI>();
        if (inventoryUI != null && inventoryUI.IsDraggingItem)
            return;

        isHovering = true;
        hoverTime = 0f;
        tooltipShown = false;
        lastMousePosition = Input.mousePosition;
        

    }

    public void OnPointerExit(PointerEventData eventData)
    {

        
        isHovering = false;
        hoverTime = 0f;
        tooltipShown = false;
        TooltipUI.Instance.HideTooltip();


    }

    public void CancelTooltip()
    {
        isHovering = false;
        hoverTime = 0f;
        tooltipShown = false;
        TooltipUI.Instance.HideTooltip();
    }
}
