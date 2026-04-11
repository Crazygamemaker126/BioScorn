using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the persistent HUD — health bar, keys, ammo, and hover timer.
/// Subscribes to PlayerInventory, HealthTracking, and PlayerController events.
/// No Update() polling — reacts only when data actually changes.
/// </summary>
public class HUDController : MonoBehaviour
{
    [Header("Health")]
    public Slider healthBar;
    public TextMeshProUGUI healthText;

    [Header("Ammo")]
    public TextMeshProUGUI ammoText;

    [Header("Keys")]
    public TextMeshProUGUI keysText;

    [Header("Hover Timer")]
    public Slider hoverTimerSlider;
    public TextMeshProUGUI hoverTimerText;
    public GameObject hoverTimerContainer;

    [Header("Data Sources")]
    public PlayerInventory playerInventory;
    public PlayerController playerController;

    // ────────────────────────────────────────────────────────────────────
    private void Start()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("HUDController: PlayerInventory not assigned.");
            return;
        }

        if (playerController == null)
        {
            Debug.LogWarning("HUDController: PlayerController not assigned.");
            return;
        }

        if (hoverTimerContainer != null)
            hoverTimerContainer.SetActive(false);

        // Health
        if (playerInventory.HealthTracking != null)
        {
            if (healthBar != null)
            {
                healthBar.minValue = 0;
                healthBar.maxValue = playerInventory.HealthTracking.maxHealth;
            }
            playerInventory.HealthTracking.OnHealthChanged += UpdateHealth;
        }

        // Inventory events
        playerInventory.OnKeyCountChanged += UpdateKeys;
        playerInventory.OnAmmoChanged += UpdateAmmo;
        playerInventory.OnHoverDurationChanged += UpdateHoverMax;

        // Hover timer — direct from PlayerController, fires every frame
        playerController.OnHoverTimerChanged += UpdateHoverSlider;
        

        // Initialise displays with current values
        if (playerInventory.HealthTracking != null)
            UpdateHealth(playerInventory.HealthTracking.CurrentHealth);

        UpdateKeys(playerInventory.KeyCount);
        UpdateAmmo(playerInventory.AmmoCount);
        UpdateHoverSlider(playerController.hoveringTimer / playerController.maxHoverDuration);
        UpdateHoverMax(playerController.maxHoverDuration);
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            if (playerInventory.HealthTracking != null)
                playerInventory.HealthTracking.OnHealthChanged -= UpdateHealth;

            playerInventory.OnKeyCountChanged -= UpdateKeys;
            playerInventory.OnAmmoChanged -= UpdateAmmo;
            playerInventory.OnHoverDurationChanged -= UpdateHoverMax;
        }

        if (playerController != null)
            playerController.OnHoverTimerChanged -= UpdateHoverSlider;
    }

    // ── Event Handlers ────────────────────────────────────────────────────
    private void UpdateHealth(int current)
    {
        if (healthBar != null) healthBar.value = current;
        if (healthText != null && playerInventory.HealthTracking != null)
            healthText.text = $"HP: {current} / {playerInventory.HealthTracking.maxHealth}";
    }

    private void UpdateAmmo(int count)
    {
        if (ammoText != null)
            ammoText.text = count > 0 ? $"Ammo: {count}" : "No Ammo";
    }

    private void UpdateKeys(int count)
    {
        if (keysText != null)
            keysText.text = $"Keys: {count}";
    }

    private void UpdateHoverMax(float newMax)
    {
        // Slider runs 0-1 normalised, so maxValue stays at 1
        if (hoverTimerText != null)
            hoverTimerText.text = $"Hover: {newMax:F1}s max";
    }

    private void UpdateHoverSlider(float normalised)
    {
        if (hoverTimerSlider != null)
            hoverTimerSlider.value = normalised;

        if (hoverTimerContainer != null && !hoverTimerContainer.activeSelf && normalised < 1f)
            hoverTimerContainer.SetActive(true);
    }
}
