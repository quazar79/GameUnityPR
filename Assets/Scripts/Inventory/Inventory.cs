using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    private List<InventoryItem> items = new List<InventoryItem>();

    public float maxWeight = 30f; // Максимальный вес
    public float currentWeight = 0f; // Текущий вес
    private bool[,] occupiedGrid;
    public int gridWidth = 8;
    public int gridHeight = 7;
    
    void Awake()
    {

        occupiedGrid = new bool[gridWidth, gridHeight];
    }

    public List<InventoryItem> GetItems()
    {
        return items;
    }

    public bool AddItem(Item item)
    {
        Debug.Log("AddItem вызван");
        if (item == null)
        {
            Debug.LogError("Попытка добавить пустой предмет (null) в инвентарь!");
            return false;
        }


        if (currentWeight + item.weight > maxWeight)
        {
            Debug.Log("Инвентарь переполнен!");
            return false;
        }

        Vector2Int position = FindFreePosition(item);
        if (position.x == -1)
        {
            Debug.Log("Нет места для предмета: " + item.itemName);
            return false;
        }

        InventoryItem inventoryItem = new InventoryItem(item, position);
        items.Add(inventoryItem);
        currentWeight += item.weight;
        MarkPositionAsOccupied(position.x, position.y, item.width, item.height);

        Debug.Log("Добавлен предмет: " + item.itemName);

        return true;
    }
    
    public Vector2Int FindFreePosition(Item item)
    {
        for (int y = 0; y <= gridHeight - item.height; y++)
        {
            for (int x = 0; x <= gridWidth - item.width; x++)
            {
                if (CanPlaceItemAt(x, y, item.width, item.height))
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return new Vector2Int(-1, -1); // Места нет
    }
    //поменял на public
    public bool CanPlaceItemAt(int startX, int startY, int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (occupiedGrid[startX + x, startY + y])
                    return false;
            }
        }
        return true;
    }
    //поменял на puvlic
    public void MarkPositionAsOccupied(int startX, int startY, int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                occupiedGrid[startX + x, startY + y] = true;
            }
        }
    }




    

    //RemoveItem нов метод для выбрасывания предметов
    public void RemoveItem(InventoryItem item)
    {
        //items.Remove(item); // items — это List<InventoryItem>
        if (items.Contains(item))
        {
            items.Remove(item);
            currentWeight -= item.item.weight;
            currentWeight = Mathf.Max(currentWeight, 0); // защита от отрицательных значений            
        }
    }

    public void ClearOccupiedSpace(Vector2Int position, int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                occupiedGrid[position.x + x, position.y + y] = false;
            }
        }
    }


}


