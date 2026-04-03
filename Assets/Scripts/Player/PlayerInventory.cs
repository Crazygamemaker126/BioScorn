using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{

    public Dictionary<Item_Base, int> inventory = new Dictionary<Item_Base, int>();
    public TextMeshProUGUI inventoryTextbox;
    
    public void AddItem(Item_Base itemName, int amount)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;
        }
        else
        {
            inventory.Add(itemName, amount);
        }

        UpdateInventoryList();
    }

    public void RemoveItem(Item_Base itemName, int amount) 
    {
        if (inventory.ContainsKey(itemName)) 
        {
            inventory[itemName] -= amount;
        }

        if (inventory[itemName] <= 0) 
        {
            inventory.Remove(itemName);
        }

        UpdateInventoryList();
    }

    private void UpdateInventoryList()
    {
        inventoryTextbox.text = "Inventory\n";

        foreach(KeyValuePair<Item_Base, int> item in inventory) 
        {
            inventoryTextbox.text += $"{item.Key.itemName} : {item.Value}\n";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Item_Base>())
        {
            AddItem(other.GetComponent<Item_Base>(),1);
            other.GetComponent<Item_Base>().Pickup();
        }
    }
}
