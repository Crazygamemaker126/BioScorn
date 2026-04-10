using UnityEngine;

[CreateAssetMenu(fileName = "HealthPackData", menuName = "Items/Collectibles/HealthPack")]
public class HealthPackData : CollectibleBase
{
    public int healAmount = 30;

    public override void OnCollected(PlayerInventory inventory)
    {
        inventory.HealPlayer(healAmount);
    }
}
