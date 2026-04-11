using UnityEngine;

/// <summary>
/// Intermediate base class for all non-weapon collectible items
/// (health packs, keys, wings, etc.).
/// Extend this instead of ItemBase directly for any pickup that
/// isn't a weapon.
/// </summary>
public abstract class CollectibleBase : ItemBase
{
    // Collectibles default to showing name and quantity.
    // Subclasses can override GetInventoryDisplay for more specific info.
    public override string GetInventoryDisplay(int quantity)
    {
        return $"{itemName} x{quantity}";
    }
}
