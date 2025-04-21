using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//ÍÅ ÈÑÏÎËÜÇÓÅÒÑß ÑÊĞÈÏÒ
public class SlotVisual : MonoBehaviour
{
    public Image background;

    void Awake()
    {
        background = GetComponent<Image>();
        background.color = new Color(1f, 1f, 1f, 0.15f);
    }

    public void SetColor(Color color)
    {
        if (background != null)
            background.color = color;
    }

    public void ResetColor()
    {
        //SetColor(Color.gray); // èñõîäíûé öâåò
        if (background != null)
            background.color = new Color(1f, 1f, 1f, 0.15f);
    }
}
