using UnityEngine;

/// <summary>
/// Data asset for a wings/hover equipment pickup.
/// Create via: Assets > Create > Items > Collectibles > Wings
/// </summary>
[CreateAssetMenu(menuName = "Items/Collectibles/Wings", fileName = "WingsData")]
public class WingsData : CollectibleBase
{
    [Header("Stat Bonuses")]
    public float hoverDurationBonus = 2f;
    public float moveSpeedBonus = 0f;
    public float jumpForceBonus = 0f;

    public override void OnCollected(PlayerInventory inventory)
    {
        inventory.ApplyWings(this);
    }

    /// <summary>
    /// Shows the hover duration bonus applied by this pickup.
    /// </summary>
    public override string GetInventoryDisplay(int quantity)
    {
        return $"{itemName} — Hover +{hoverDurationBonus}s, Speed +{moveSpeedBonus}, Jump +{jumpForceBonus}";
    }
}