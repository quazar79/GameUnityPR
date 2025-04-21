using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item; // Ссылка на сам предмет (ассет)

    public void Interact() // Метод, который вызывается при нажатии "E"
    {
        Debug.Log("Interact вызван");
        
        Inventory inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        if (inventory != null)
        {
            bool added = inventory.AddItem(item);
            if (added)
            {
                Debug.Log("Предмет добавлен в инвентарь: " + item.itemName);
                Destroy(gameObject);
            }
            
            else
            {
                Debug.LogWarning("Не удалось добавить предмет в инвентарь: " + item.itemName);
            }
           
        }
        else
        {
            Debug.LogError("Инвентарь игрока не найден!");
        }
    }
}
