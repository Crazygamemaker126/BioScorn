// UIManager.cs
using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("SO Reference")]
    [SerializeField] PlayerInventory inventory;

    [Header("Stats bars")]
    [SerializeField] Slider healthBar;
    [SerializeField] Slider armorBar;
    [SerializeField] TMP_Text healthLabel;
    [SerializeField] TMP_Text armorLabel;

    [Header("Pickup toast")]
    [SerializeField] GameObject toastPanel;
    [SerializeField] TMP_Text toastText;
    [SerializeField] Image toastIcon;
    [SerializeField] float toastDuration = 3f;

    [Header("Inventory list")]
    [SerializeField] Transform weaponListParent;
    [SerializeField] GameObject weaponRowPrefab; // has Icon, NameLabel, AmmoLabel
    [SerializeField] Transform keyListParent;
    [SerializeField] GameObject keyIconPrefab;

    // Track spawned rows so we can update ammo in-place
    readonly Dictionary<WeaponData, WeaponRow> _weaponRows = new();

    // ── Lifecycle ─────────────────────────────────────────────────────────
    void OnEnable()
    {
        inventory.Initialise();
        Subscribe();
        RefreshAllUI();
    }

    void OnDisable() => Unsubscribe();

    void Subscribe()
    {
        inventory.OnHealthChanged += HandleHealthChanged;
        //inventory.OnWingsChanged += HandleArmorChanged;
        inventory.OnWeaponAdded += HandleWeaponAdded;
        inventory.OnAmmoChanged += HandleAmmoChanged;
        inventory.OnKeyAdded += HandleKeyAdded;
        inventory.OnItemCollected += ShowToast;
    }

    void Unsubscribe()
    {
        inventory.OnHealthChanged -= HandleHealthChanged;
        //inventory.OnArmorChanged -= HandleArmorChanged;
        inventory.OnWeaponAdded -= HandleWeaponAdded;
        inventory.OnAmmoChanged -= HandleAmmoChanged;
        inventory.OnKeyAdded -= HandleKeyAdded;
        inventory.OnItemCollected -= ShowToast;
    }

    // ── Event handlers ────────────────────────────────────────────────────
    void HandleHealthChanged(int current, int max)
    {
        healthBar.value = (float)current / max;
        healthLabel.text = $"{current} / {max}";
    }

    void HandleArmorChanged(int current, int max)
    {
        armorBar.value = (float)current / max;
        armorLabel.text = $"{current} / {max}";
    }

    void HandleWeaponAdded(WeaponData weapon, int ammo)
    {
        var row = Instantiate(weaponRowPrefab, weaponListParent)
                      .GetComponent<WeaponRow>();
        row.Bind(weapon, ammo);
        _weaponRows[weapon] = row;
    }

    void HandleAmmoChanged(WeaponData weapon, int ammo)
    {
        if (_weaponRows.TryGetValue(weapon, out var row))
            row.UpdateAmmo(ammo);
    }

    void HandleKeyAdded(KeyData key)
    {
        var icon = Instantiate(keyIconPrefab, keyListParent);
        //icon.GetComponent<Image>().sprite = key.icon;
        icon.GetComponentInChildren<TMP_Text>().text = key.itemName;
    }

    // ── Toast / pickup notification ───────────────────────────────────────
    Coroutine _toastRoutine;

    void ShowToast(ItemBase item, string message)
    {
        if (_toastRoutine != null) StopCoroutine(_toastRoutine);
        //toastIcon.sprite = item != null ? item.icon : null;
        toastIcon.enabled = item != null;
        toastText.text = message;
        _toastRoutine = StartCoroutine(ToastTimer());
    }

    IEnumerator ToastTimer()
    {
        toastPanel.SetActive(true);
        yield return new WaitForSeconds(toastDuration);
        toastPanel.SetActive(false);
    }

    // ── Full refresh (called once on enable) ──────────────────────────────
    void RefreshAllUI()
    {
        HandleHealthChanged(inventory.CurrentHealth, inventory.maxHealth);
        //HandleArmorChanged(inventory.CurrentArmor, inventory.maxArmor);
    }
}