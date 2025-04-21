using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName; // Название предмета
    public Sprite icon; // Иконка предмета
    public string description; // Описание
    public float weight; // Вес предмета
    public int width; // Ширина предмета в слотах
    public int height; // Высота предмета в слотах

    public GameObject worldPrefab; //  Префаб для выбрасывания
}
