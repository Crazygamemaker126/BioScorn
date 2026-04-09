using UnityEngine;

[CreateAssetMenu(menuName = "Items/Collectibles/Wings")]
public class WingsData : ItemBase
{
    [Header("Mobility Modifiers")]
    public float hoverDurationBonus = 8f;    // adds to maxHoverDuration
    public float moveSpeedBonus = 2f;    // adds to moveSpeed
    public float jumpForceBonus = 2f;    // adds to jumpForce

    public override void OnCollected(PlayerInventory inventory)
    {
        inventory.AddWings(this);
    }
}