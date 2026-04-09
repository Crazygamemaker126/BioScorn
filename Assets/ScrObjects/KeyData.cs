using UnityEngine;

[CreateAssetMenu(fileName = "KeyData", menuName = "Items/Collectibles/Key")]
public class KeyData : ItemBase
{
    public string doorID;

    public override void OnCollected(PlayerInventory inventory)
    {
        inventory.AddKey(this);
    }
}
