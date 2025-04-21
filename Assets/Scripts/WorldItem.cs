using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public Item itemData;

    public void Initialize(Item item)
    {
        itemData = item;
        // можно визуально настроить MeshRenderer, если нужно
    }
}
