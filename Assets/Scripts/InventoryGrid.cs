using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGrid : MonoBehaviour
{
    public int gridWidth = 8;
    public int gridHeight = 7;
    private bool[,] grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = new bool[gridWidth, gridHeight];
    }

    public bool CanPlaceItem(Item item, out Vector2Int position)
    {
        for (int x = 0; x <= gridWidth - item.width; x++)
        {
            for (int y = 0; y <= gridHeight - item.height; y++)
            {
                if (IsSpaceFree(x, y, item.width, item.height))
                {
                    position = new Vector2Int(x, y);
                    return true;
                }
            }
        }

        position = Vector2Int.zero;
        return false;
    }

    public void PlaceItem(Item item, Vector2Int pos)
    {
        for (int x = 0; x < item.width; x++)
        {
            for (int y = 0; y < item.height; y++)
            {
                grid[pos.x + x, pos.y + y] = true;
            }
        }

        // Здесь можно добавить создание UI-объекта слота
    }

    private bool IsSpaceFree(int startX, int startY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[startX + x, startY + y])
                    return false;
            }
        }
        return true;
    }
}
