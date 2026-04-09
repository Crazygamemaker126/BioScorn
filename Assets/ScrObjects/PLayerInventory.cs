// PlayerInventory.cs
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerInventory")]
public class PlayerInventory : ScriptableObject
{
    [Header("Stats")]
    public int maxHealth = 100;
    public int maxArmor = 100;

    // Runtime state (not serialized between plays)
    [NonSerialized] public int CurrentHealth;
    [NonSerialized] public WingsData Wings;
    [NonSerialized] public List<KeyData> Keys = new();
    [NonSerialized] public List<WeaponData> Weapons = new();
    [NonSerialized] public Dictionary<WeaponData, int> Ammo = new();

    // ── Events ───────────────────────────────────────────────────────────
    public event Action<int, int> OnHealthChanged;   // current, max
    public event Action<WingsData> OnWingsCollected;
    public event Action<WeaponData, int> OnWeaponAdded;     // weapon, ammo
    public event Action<WeaponData, int> OnAmmoChanged;     // weapon, ammo
    public event Action<KeyData> OnKeyAdded;
    public event Action<ItemBase, string> OnItemCollected;  // item, UI message

    // ── Initialise on scene load ─────────────────────────────────────────
    public void Initialise()
    {
        CurrentHealth = maxHealth;
        
        Keys.Clear();
        Weapons.Clear();
        Ammo.Clear();
    }

    // ── Public API (called by ItemData subclasses) ────────────────────────
    public void HealPlayer(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        OnItemCollected?.Invoke(null, $"+{amount} HP  →  {CurrentHealth}/{maxHealth}");
    }

    public void AddWings(WingsData wings)
    {
        Wings = wings;
        OnWingsCollected?.Invoke(wings);
        OnItemCollected?.Invoke(wings,
            $"Wings equipped!  Hover +{wings.hoverDurationBonus}s  |  Speed +{wings.moveSpeedBonus}");
    }

    public void AddWeapon(WeaponData weapon, int startAmmo)
    {
        if (!Weapons.Contains(weapon))
        {
            Weapons.Add(weapon);
            Ammo[weapon] = startAmmo;
            OnWeaponAdded?.Invoke(weapon, startAmmo);
            OnItemCollected?.Invoke(weapon, $"Picked up {weapon.itemName}! {weapon.GetAmmoDisplay(startAmmo)}");
        }
        else
        {
            // Already have it — add ammo instead
            Ammo[weapon] = Mathf.Min(Ammo[weapon] + startAmmo, weapon.maxAmmo);
            OnAmmoChanged?.Invoke(weapon, Ammo[weapon]);
            OnItemCollected?.Invoke(weapon, $"{weapon.itemName} ammo: {weapon.GetAmmoDisplay(Ammo[weapon])}");
        }
    }

    public void AddKey(KeyData key)
    {
        if (!Keys.Contains(key))
        {
            Keys.Add(key);
            OnKeyAdded?.Invoke(key);
            OnItemCollected?.Invoke(key, $"Key collected: {key.itemName}  (door: {key.doorID})");
        }
    }

    public void SpendAmmo(WeaponData weapon, int amount)
    {
        if (!Ammo.ContainsKey(weapon)) return;
        Ammo[weapon] = Mathf.Max(0, Ammo[weapon] - amount);
        OnAmmoChanged?.Invoke(weapon, Ammo[weapon]);
    }
}