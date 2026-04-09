using UnityEngine;

// ─────────────────────────────────────────────────────────────────────────────
// WEAPONS (3)
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Pistol — ranged weapon, 12 rounds.</summary>
public class Pistol : EquipmentItem
{
    [Header("Weapon Stats")]
    public int currentAmmo = 12;
    public int maxAmmo     = 12;

    private void Awake()
    {
        itemName       = "Pistol";
        collectMessage = "Pistol acquired! 12 rounds loaded.";
    }

    public override void ApplyEffect(PlayerInventory inventory) { }

    public override string GetStatusText() =>
        $"{itemName} — {currentAmmo} / {maxAmmo} ammo";
}

/// <summary>Shotgun — high damage, low capacity.</summary>
public class Shotgun : EquipmentItem
{
    [Header("Weapon Stats")]
    public int currentAmmo = 6;
    public int maxAmmo     = 6;

    private void Awake()
    {
        itemName       = "Shotgun";
        collectMessage = "Shotgun picked up! 6 shells loaded.";
    }

    public override void ApplyEffect(PlayerInventory inventory) { }

    public override string GetStatusText() =>
        $"{itemName} — {currentAmmo} / {maxAmmo} shells";
}

/// <summary>Knife — melee weapon, no ammo.</summary>
public class Knife : EquipmentItem
{
    private void Awake()
    {
        itemName       = "Knife";
        collectMessage = "Knife equipped! Silent and deadly.";
    }

    public override void ApplyEffect(PlayerInventory inventory) { }

    public override string GetStatusText() => $"{itemName} — Melee";
}

// ─────────────────────────────────────────────────────────────────────────────
// PICKUPS (3)
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Health Pack — restores 25 HP.</summary>
public class HealthPack : EquipmentItem
{
    public int healAmount = 25;

    private void Awake()
    {
        itemName       = "Health Pack";
        collectMessage = $"Health restored! +{healAmount} HP";
    }

    public override void ApplyEffect(PlayerInventory inventory)
    {
        if (inventory.HealthTracking != null)
            inventory.HealthTracking.OnHealthIncreased(healAmount);
    }

    public override string GetStatusText() => $"{itemName} — Restored {healAmount} HP";
}

//Ammo Crate
public class AmmoCrate : EquipmentItem
{
    public int ammoAmount = 50;

    private void Awake()
    {
        itemName       = "Ammo Crate";
        collectMessage = $"Ammo crate found! +{ammoAmount} ammo.";
    }

    public override void ApplyEffect(PlayerInventory inventory)
    {
        inventory.AddAmmo(ammoAmount);
    }

    public override string GetStatusText() => $"{itemName} — {ammoAmount} pts";
}



