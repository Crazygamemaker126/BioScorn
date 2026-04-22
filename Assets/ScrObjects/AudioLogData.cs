using UnityEngine;

[CreateAssetMenu(menuName = "Items/Collectibles/AudioLog")]
public class AudioLogData : ItemBase
{
    public AudioClip audioClip;
    public AudioSource audioSource;

    public override void OnCollected(PlayerInventory inventory)
    {
        return;
        //Needs to fire the audio clip on collection.
    }

    public override string GetInventoryDisplay(int quantity)
    {
        return $"{itemName} x{quantity}";
    }
}
