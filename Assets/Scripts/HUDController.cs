using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the persistent HUD — health bar, armor, keys, hover timer.
/// Subscribes to PlayerInventory events in Start().
/// No Update() polling — reacts only when data actually changes.
/// </summary>
public class HUDController : MonoBehaviour
{
    [Header("Health")]
    public Slider          healthBar;
    public TextMeshProUGUI healthText;

    [Header("Armor")]
    public TextMeshProUGUI armorText;

    [Header("Keys")]
    public TextMeshProUGUI keysText;

    [Header("Hover Timer")]
    public Slider          hoverTimerSlider;
    public TextMeshProUGUI hoverTimerText;

    [Header("Data Source")]
    public PlayerInventory playerInventory;

    private void Start()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("HUDController: PlayerInventory not assigned.");
            return;
        }

        // Subscribe — UI updates only when data changes
        if (playerInventory.HealthTracking != null)
            playerInventory.HealthTracking.OnHealthChanged += UpdateHealth;

        playerInventory.OnArmorChanged          += UpdateArmor;
        playerInventory.OnKeyCountChanged       += UpdateKeys;
        playerInventory.OnHoverDurationChanged  += UpdateHoverMax;

        // Initialise displays
        if (playerInventory.HealthTracking != null)
            UpdateHealth(playerInventory.HealthTracking.CurrentHealth);

        UpdateArmor(0);
        UpdateKeys(0);
    }

    private void OnDestroy()
    {
        if (playerInventory == null) return;

        if (playerInventory.HealthTracking != null)
            playerInventory.HealthTracking.OnHealthChanged -= UpdateHealth;

        playerInventory.OnArmorChanged         -= UpdateArmor;
        playerInventory.OnKeyCountChanged      -= UpdateKeys;
        playerInventory.OnHoverDurationChanged -= UpdateHoverMax;
    }

    // ── Event Handlers ────────────────────────────────────────────────────────

    private void UpdateHealth(int current)
    {
        if (healthBar  != null) healthBar.value = current;
        if (healthText != null && playerInventory.HealthTracking != null)
            healthText.text = $"HP: {current} / {playerInventory.HealthTracking.maxHealth}";
    }

    private void UpdateArmor(int armor)
    {
        if (armorText != null)
            armorText.text = armor > 0 ? $"Armor: {armor}" : "";
    }

    private void UpdateKeys(int count)
    {
        if (keysText != null)
            keysText.text = $"Keys: {count}";
    }

    private void UpdateHoverMax(float newMax)
    {
        if (hoverTimerSlider != null) hoverTimerSlider.maxValue = newMax;
        if (hoverTimerText   != null) hoverTimerText.text = $"Hover: {newMax:F1}s max";
    }
}
