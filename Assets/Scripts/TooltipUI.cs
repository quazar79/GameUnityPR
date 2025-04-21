using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class TooltipUI : MonoBehaviour
{
    

    public static TooltipUI Instance;

    //public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;
    //public TextMeshProUGUI itemNameText;
    //public TextMeshProUGUI descriptionText;
    //public TextMeshProUGUI weightText;
    public RectTransform backgroundRect;
    public CanvasGroup canvasGroup;
    private float fadeSpeed = 6f;
    private bool isShowing = false;

    private void Awake()
    {
        Instance = this;
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        
    }
    void Update()
    {
        
        Vector2 mousePosition = Input.mousePosition;
        transform.position = mousePosition;
        float offsetX = 10f;
        float offsetY = -10f;

        canvasGroup.transform.position = mousePosition + new Vector2(offsetX, offsetY);
        float targetAlpha = isShowing ? 1f : 0f;
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.unscaledDeltaTime * fadeSpeed);
    }

    public void ShowTooltip(InventoryItem item)
    {
        tooltipText.text = $"{item.item.itemName}\n\n{item.item.description}\n\nВес: {item.item.weight}";
        

        // Автоматически подгоняет фон
        LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundRect);

        isShowing = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void HideTooltip()
    {
        
        canvasGroup.alpha = 0f;
        isShowing = false;
        canvasGroup.blocksRaycasts = false;
    }

    
}
