using UnityEngine;

[CreateAssetMenu(fileName = "KeyData", menuName = "Items/Collectibles/Key")]
public class KeyData : CollectibleBase
{
    public string doorID;

    public override void OnCollected(PlayerInventory inventory)
    {
        inventory.AddKey(this);
    }

    /// <summary>
    /// Shows which door this key belongs to.
    /// </summary>
    public override string GetInventoryDisplay(int quantity)
    {
        return $"{itemName} — Opens: {doorID}";
    }
}