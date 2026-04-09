using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase", menuName = "ScriptableObjects/ItemBase")]
public abstract class ItemBase : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    [TextArea] public string pickupMessage;
    public Sprite Icon;

    public abstract void OnCollected(PlayerInventory inventory);
}
