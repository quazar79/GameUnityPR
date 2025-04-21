using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastVisualizer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Raycast ��������! ������ �� " + gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("������ ������ � " + gameObject.name);
    }
}
