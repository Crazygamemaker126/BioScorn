using UnityEngine;


public abstract class EquipmentItem : Equipment_Base
{
    [Header("UI Fields")]
    [Tooltip("Message shown in the notification popup when collected.")]
    public string collectMessage = "";

    
    public virtual string GetStatusText()
    {
        return string.IsNullOrEmpty(collectMessage) ? itemName : $"{itemName}";
    }

    
    public override void Pickup()
    {
        PlayerInventory inventory = FindAnyObjectByType<PlayerInventory>();
        if (inventory != null)
            ApplyEffect(inventory);

        base.Pickup();
    }

    
    public abstract void ApplyEffect(PlayerInventory inventory);
}
