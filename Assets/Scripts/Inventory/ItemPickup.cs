using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item; // ������ �� ��� ������� (�����)

    public void Interact() // �����, ������� ���������� ��� ������� "E"
    {
        Debug.Log("Interact ������");
        
        Inventory inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        if (inventory != null)
        {
            bool added = inventory.AddItem(item);
            if (added)
            {
                Debug.Log("������� �������� � ���������: " + item.itemName);
                Destroy(gameObject);
            }
            
            else
            {
                Debug.LogWarning("�� ������� �������� ������� � ���������: " + item.itemName);
            }
           
        }
        else
        {
            Debug.LogError("��������� ������ �� ������!");
        }
    }
}
