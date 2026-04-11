using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "ScriptableObjects/ItemBase")]
public abstract class ItemBase : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    [TextArea] public string pickupMessage;
    public Sprite Icon;

    public abstract void OnCollected(PlayerInventory inventory);

    /// <summary>
    /// Returns a formatted string for the inventory panel.
    /// Each subclass overrides this to show item-relevant information.
    /// Default shows item name and quantity.
    /// </summary>
    public virtual string GetInventoryDisplay(int quantity)
    {
        return $"{itemName} x{quantity}";
    }
}
