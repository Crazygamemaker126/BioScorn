// WeaponRow.cs
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponRow : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TMP_Text nameLabel;
    [SerializeField] TMP_Text ammoLabel;

    WeaponData _weapon;

    public void Bind(WeaponData weapon, int ammo)
    {
        _weapon = weapon;
        //icon.sprite = weapon.icon;
        nameLabel.text = weapon.itemName;
        ammoLabel.text = weapon.GetAmmoDisplay(ammo);
    }

    public void UpdateAmmo(int ammo) =>
        ammoLabel.text = _weapon.GetAmmoDisplay(ammo);
}