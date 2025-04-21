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

    public float maxWeight = 30f; // ������������ ���
    public float currentWeight = 0f; // ������� ���
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
        Debug.Log("AddItem ������");
        if (item == null)
        {
            Debug.LogError("������� �������� ������ ������� (null) � ���������!");
            return false;
        }


        if (currentWeight + item.weight > maxWeight)
        {
            Debug.Log("��������� ����������!");
            return false;
        }

        Vector2Int position = FindFreePosition(item);
        if (position.x == -1)
        {
            Debug.Log("��� ����� ��� ��������: " + item.itemName);
            return false;
        }

        InventoryItem inventoryItem = new InventoryItem(item, position);
        items.Add(inventoryItem);
        currentWeight += item.weight;
        MarkPositionAsOccupied(position.x, position.y, item.width, item.height);

        Debug.Log("�������� �������: " + item.itemName);

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

        return new Vector2Int(-1, -1); // ����� ���
    }
    //������� �� public
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
    //������� �� puvlic
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




    

    //RemoveItem ��� ����� ��� ������������ ���������
    public void RemoveItem(InventoryItem item)
    {
        //items.Remove(item); // items � ��� List<InventoryItem>
        if (items.Contains(item))
        {
            items.Remove(item);
            currentWeight -= item.item.weight;
            currentWeight = Mathf.Max(currentWeight, 0); // ������ �� ������������� ��������            
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


