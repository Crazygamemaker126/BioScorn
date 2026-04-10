using UnityEngine;

/// <summary>
/// Intermediate base class for all non-weapon collectible items
/// (health packs, keys, wings, etc.).
/// Extend this instead of ItemBase directly for any pickup that
/// isn't a weapon.
/// </summary>
public abstract class CollectibleBase : ItemBase
{
    // Shared collectible-specific fields can go here in the future
    // (e.g. pickup sound, particle effect prefab, etc.)
}
