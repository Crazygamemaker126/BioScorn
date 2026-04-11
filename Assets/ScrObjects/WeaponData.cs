using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ItemBase
{
    [Header("Weapon Stats")]
    public int maxAmmo;
    public int startingAmmo;
    public float damage;

    public virtual string GetAmmoDisplay(int current) => $"{current} / {maxAmmo}";

    public override void OnCollected(PlayerInventory inventory)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Weapons display their ammo status using GetAmmoDisplay,
    /// which each weapon subclass overrides with its own format.
    /// </summary>
    public override string GetInventoryDisplay(int quantity)
    {
        return $"{itemName} — {GetAmmoDisplay(quantity)}";
    }
}