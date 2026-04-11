using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Toggleable inventory panel — press Tab to open/close.
/// Subscribes to PlayerInventory events to refresh the display
/// whenever the inventory changes. No Update() polling.
///
/// Each row calls item.GetInventoryDisplay(quantity) which each
/// ItemBase subclass overrides to show item-relevant information.
/// </summary>
public class InventoryPanelUI : MonoBehaviour
{
    [Header("References")]
    public PlayerInventory playerInventory;
    public TextMeshProUGUI inventoryRowPrefab;
    public Transform rowContainer;

    [Header("Toggle")]
    public KeyCode toggleKey = KeyCode.Tab;

    private bool _isOpen = false;

    // Tracks spawned row objects so we can clear and rebuild cleanly
    private readonly List<GameObject> _rows = new List<GameObject>();

    // ────────────────────────────────────────────────────────────────────
    private void Start()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("InventoryPanelUI: PlayerInventory not assigned.");
            return;
        }

        playerInventory.OnItemCollected += OnItemCollectedHandler;
        playerInventory.OnAmmoChanged += OnAmmoChangedHandler;
        playerInventory.OnKeyCountChanged += OnKeyCountChangedHandler;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (playerInventory == null) return;

        playerInventory.OnItemCollected -= OnItemCollectedHandler;
        playerInventory.OnAmmoChanged -= OnAmmoChangedHandler;
        playerInventory.OnKeyCountChanged -= OnKeyCountChangedHandler;
    }

    // ── Event handler shims ──────────────────────────────────────────────
    // Named methods so subscribe/unsubscribe references match correctly.
    private void OnItemCollectedHandler(ItemBase item) => RefreshInventory();
    private void OnAmmoChangedHandler(int amount) => RefreshInventory();
    private void OnKeyCountChangedHandler(int count) => RefreshInventory();

    // ────────────────────────────────────────────────────────────────────
    // Input check only — one line, acceptable Update use.
    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            SetPanelOpen(!_isOpen);
    }

    private void SetPanelOpen(bool open)
    {
        _isOpen = open;
        gameObject.SetActive(open);

        // Refresh when opening so display is always current
        if (open) RefreshInventory();
    }

    /// <summary>
    /// Clears all rows and rebuilds from the current inventory dictionary.
    /// Each item drives its own display string via GetInventoryDisplay().
    /// </summary>
    private void RefreshInventory()
    {
        foreach (GameObject row in _rows)
            Destroy(row);
        _rows.Clear();

        if (inventoryRowPrefab == null || rowContainer == null) return;

        foreach (var kvp in playerInventory.inventory)
        {
            ItemBase item = kvp.Key;
            int quantity = kvp.Value;

            TextMeshProUGUI row = Instantiate(inventoryRowPrefab, rowContainer);
            row.text = item.GetInventoryDisplay(quantity);
            _rows.Add(row.gameObject);
        }
    }
}