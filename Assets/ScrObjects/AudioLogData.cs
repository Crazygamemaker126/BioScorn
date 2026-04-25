using UnityEngine;

[CreateAssetMenu(menuName = "Items/Collectibles/AudioLog")]
public class AudioLogData : ItemBase
{
    [Header("Audio")]
    public AudioClip audioClip;
    [Header("Subtitles")]
    [TextArea(3, 8)]
    public string subtitleText;
    
    


    public override void OnCollected(PlayerInventory inventory)
    {
        inventory.AddAudioLog(this);
    }

    public override string GetInventoryDisplay(int quantity)
    {
        return $"{itemName} x{quantity}";
    }
}
