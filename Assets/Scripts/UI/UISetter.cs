using TMPro;
using UnityEngine;

/// <summary>
/// Drives the key count and ammo count UI text fields.
/// Subscribes to PlayerInventory events — no Update() polling at all.
///
/// Assign the PlayerInventory reference and both TMP fields in the Inspector.
/// </summary>
public class UISetter : MonoBehaviour
{
    [Header("References")]
    public PlayerInventory playerInventory;

    [Header("UI Fields")]
    public TextMeshProUGUI keysAmountText;
    public TextMeshProUGUI ammoAmountText;

    // ────────────────────────────────────────────────────────────────────
    private void Start()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("UISetter: PlayerInventory not assigned.");
            return;
        }

        // Subscribe to events
        playerInventory.OnKeyCountChanged += UpdateKeysDisplay;
        playerInventory.OnAmmoChanged += UpdateAmmoDisplay;

        // Initialise display to current values
        // (in case items are pre-loaded before Start runs)
        UpdateKeysDisplay(playerInventory.KeyCount);
        UpdateAmmoDisplay(playerInventory.AmmoCount);
    }

    private void OnDestroy()
    {
        if (playerInventory == null) return;

        playerInventory.OnKeyCountChanged -= UpdateKeysDisplay;
        playerInventory.OnAmmoChanged -= UpdateAmmoDisplay;
    }

    // ────────────────────────────────────────────────────────────────────
    private void UpdateKeysDisplay(int count)
    {
        if (keysAmountText != null)
            keysAmountText.text = $"Keys: {count}";
    }

    private void UpdateAmmoDisplay(int count)
    {
        if (ammoAmountText != null)
            ammoAmountText.text = $"Ammo: {count}";
    }
}
