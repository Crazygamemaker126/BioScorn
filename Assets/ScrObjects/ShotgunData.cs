using UnityEngine;

[CreateAssetMenu(fileName = "ShotgunData", menuName = "Items/Weapons/Shotgun")]
public class ShotgunData : WeaponData
{
    public int pelletsPerShell = 8;

    public override void OnCollected(PlayerInventory inventory)
    {
        inventory.AddWeapon(this, startingAmmo);
    }

    public override string GetAmmoDisplay(int current) => $"{current} shells ({current * pelletsPerShell} pellets)";
    
        
    
}
