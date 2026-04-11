using UnityEngine;

[CreateAssetMenu(fileName = "HealthPackData", menuName = "Items/Collectibles/HealthPack")]
public class HealthPackData : CollectibleBase
{
    public int healAmount = 30;

    public override void OnCollected(PlayerInventory inventory)
    {
        inventory.HealPlayer(healAmount);
    }

    /// <summary>
    /// Shows how much health each pack restores and how many are held.
    /// </summary>
    public override string GetInventoryDisplay(int quantity)
    {
        return $"{itemName} x{quantity} — +{healAmount} HP each";
    }
}
