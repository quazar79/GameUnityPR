using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName; // �������� ��������
    public Sprite icon; // ������ ��������
    public string description; // ��������
    public float weight; // ��� ��������
    public int width; // ������ �������� � ������
    public int height; // ������ �������� � ������

    public GameObject worldPrefab; //  ������ ��� ������������
}
