using UnityEngine;

[CreateAssetMenu(fileName = "KnifeData", menuName = "Items/Weapons/Knife")]
public class KnifeData : WeaponData
{
    //Knife is for melee, we reuse what can be reused.
    public override void OnCollected(PlayerInventory inventory)
    {
        inventory.AddWeapon(this, 0);
    }

    public override string GetAmmoDisplay(int current) => "Melee";
    
       
    
}
