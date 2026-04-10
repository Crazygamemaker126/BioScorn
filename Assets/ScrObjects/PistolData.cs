using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "PistolData", menuName = "Items/Weapons/Pistol")]
public class PistolData : WeaponData
{
    public int magSize = 12;

    public override void OnCollected(PlayerInventory inventory)
    {
        inventory.AddWeapon(this, startingAmmo);
    }

    public override string GetAmmoDisplay(int current) => $"{current % magSize} | {current} total";




}
