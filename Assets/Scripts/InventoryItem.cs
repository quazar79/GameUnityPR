using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public Item item;
    public Vector2Int position;

    public InventoryItem(Item item, Vector2Int position)
    {
        this.item = item;
        this.position = position;
    }
}
