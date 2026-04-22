using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Tracks everything the player is carrying.
/// All item interaction goes through ScriptableObject-based ItemBase data —
/// no MonoBehaviour item classes anywhere in this system.
///
/// World pickups should have a WorldItem component that holds an ItemBase
/// reference and calls inventory.CollectItem(data) on trigger.
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    // ── Runtime inventory store ──────────────────────────────────────────
    public Dictionary<ItemBase, int> inventory = new Dictionary<ItemBase, int>();

    // ── Cached component refs ────────────────────────────────────────────
    public HealthTracking HealthTracking { get; private set; }
    private PlayerController _playerController;

    // ── Tracked stats ────────────────────────────────────────────────────
    public int KeyCount { get; private set; } = 0;
    public int AmmoCount { get; private set; } = 0;

    // ── UI Events ────────────────────────────────────────────────────────
    public event Action<ItemBase> OnItemCollected;
    public event Action<int> OnKeyCountChanged;
    public event Action<int> OnAmmoChanged;
    public event Action<float> OnHoverDurationChanged;
    public event Action<int> OnHealthChanged;

    // ────────────────────────────────────────────────────────────────────
    private void Awake()
    {
        HealthTracking = GetComponent<HealthTracking>();
        _playerController = GetComponent<PlayerController>();
    }

    // ── Pickup entry point ───────────────────────────────────────────────
    public void CollectItem(ItemBase itemData)
    {
        Debug.Log($"CollectItem fired for: {itemData.itemName}");
        itemData.OnCollected(this);
        OnItemCollected?.Invoke(itemData);
    }           

    // ── Generic inventory helpers ────────────────────────────────────────
    public void AddToInventory(ItemBase itemData, int amount = 1)
    {
        if (inventory.ContainsKey(itemData))
            inventory[itemData] += amount;
        else
            inventory[itemData] = amount;
    }

    public void RemoveFromInventory(ItemBase itemData, int amount = 1)
    {
        if (!inventory.ContainsKey(itemData)) return;

        inventory[itemData] -= amount;

        if (inventory[itemData] <= 0)
            inventory.Remove(itemData);
    }

    public void OpenInventory(ContextCallback context)
    {

    }

    // ── Item-type-specific methods (called by SO OnCollected) ────────────

    /// <summary>Called by HealthPackData.OnCollected.</summary>
    public void HealPlayer(int amount)
    {
        if (HealthTracking == null) return;
        HealthTracking.OnHealthIncreased(amount);
        OnHealthChanged?.Invoke(HealthTracking.CurrentHealth);
    }

    /// <summary>Called by KeyData.OnCollected.</summary>
    public void AddKey(KeyData key)
    {
        AddToInventory(key);
        KeyCount++;
        OnKeyCountChanged?.Invoke(KeyCount);
    }

    /// <summary>Called by WeaponData subtypes (Pistol, Shotgun, etc.).</summary>
    public void AddWeapon(WeaponData weapon, int startingAmmo)
    {
        if (inventory.ContainsKey(weapon))
            inventory[weapon] += startingAmmo;
        else
            inventory[weapon] = startingAmmo;

        AmmoCount += startingAmmo;
        OnAmmoChanged?.Invoke(AmmoCount);
    }

    /// <summary>Called by WingsData.OnCollected.</summary>
    public void ApplyWings(WingsData wings)
    {
        if (_playerController == null) return;

        _playerController.maxHoverDuration += wings.hoverDurationBonus;
        _playerController.moveSpeed += wings.moveSpeedBonus;
        _playerController.jumpForce += wings.jumpForceBonus;

        _playerController.hoveringTimer = Mathf.Min(
            _playerController.hoveringTimer + wings.hoverDurationBonus,
            _playerController.maxHoverDuration);

        AddToInventory(wings);
        OnHoverDurationChanged?.Invoke(_playerController.maxHoverDuration);
    }

    // ── Key check helpers (used by door scripts) ─────────────────────────
    public bool HasKey(string doorID)
    {
        foreach (var kvp in inventory)
        {
            if (kvp.Key is KeyData key && key.doorID == doorID && kvp.Value > 0)
                return true;
        }
        return false;
    }

    public bool UseKey(string doorID)
    {
        foreach (var kvp in inventory)
        {
            if (kvp.Key is KeyData key && key.doorID == doorID && kvp.Value > 0)
            {
                RemoveFromInventory(key);
                KeyCount = Mathf.Max(0, KeyCount - 1);
                OnKeyCountChanged?.Invoke(KeyCount);
                return true;
            }
        }
        return false;
    }
}
