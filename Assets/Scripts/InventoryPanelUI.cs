using TMPro;
using UnityEngine;

/// <summary>
/// Toggleable inventory panel — press Tab (or reassign toggleKey) to open/close.
/// Subscribes to PlayerInventory.OnItemCollected to add a row for each new pickup.
/// No Update() polling beyond the single key check for the toggle.
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

    // ────────────────────────────────────────────────────────────────────
    private void Start()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("InventoryPanelUI: PlayerInventory not assigned.");
            return;
        }

        playerInventory.OnItemCollected += AddInventoryRow;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
            playerInventory.OnItemCollected -= AddInventoryRow;
    }

    // Input check only — one line, acceptable Update use.
    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            SetPanelOpen(!_isOpen);
    }

    // ────────────────────────────────────────────────────────────────────
    private void SetPanelOpen(bool open)
    {
        _isOpen = open;
        gameObject.SetActive(open);
    }

    private void AddInventoryRow(ItemBase item)
    {
        if (inventoryRowPrefab == null || rowContainer == null) return;

        TextMeshProUGUI row = Instantiate(inventoryRowPrefab, rowContainer);

        // Display pickup message if the designer set one, otherwise just the item name
        row.text = !string.IsNullOrEmpty(item.pickupMessage)
            ? item.pickupMessage
            : item.itemName;
    }
}
