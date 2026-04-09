using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerInventory : MonoBehaviour
{
    
    public Dictionary<Item_Base, int> inventory = new Dictionary<Item_Base, int>();
    public TextMeshProUGUI inventoryTextbox;

   
    public HealthTracking HealthTracking { get; private set; }
    private PlayerController _playerController;

    //Player inventory stats
    public int KeyCount { get; private set; } = 0;
    public int AmmoCount { get; private set; } = 0;

    //UI implementation
    public event Action<Item_Base> OnItemCollected;
    public event Action<int> OnKeyCountChanged;
    public event Action<int> OnArmorChanged;
  

    //Caches references
    private void Awake()
    {
        HealthTracking = GetComponent<HealthTracking>();
        _playerController = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Item_Base>())
        {
            AddItem(other.GetComponent<Item_Base>(), 1);
            other.GetComponent<Item_Base>().Pickup();

            // Added fire event so UI reacts instantly
            OnItemCollected?.Invoke(other.GetComponent<Item_Base>());
        }
    }

    
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

        foreach (KeyValuePair<Item_Base, int> item in inventory)
        {
            inventoryTextbox.text += $"{item.Key.itemName} : {item.Value}\n";
        }
    }

    

    public void AddKey()
    {
        KeyCount++;
        OnKeyCountChanged?.Invoke(KeyCount);
    }

    public void AddAmmo(int amount)
    {
        AmmoCount += amount;
        OnArmorChanged?.Invoke(AmmoCount);
    }

    
}
