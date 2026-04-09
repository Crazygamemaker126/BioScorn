using TMPro;
using UnityEngine;


public class InventoryPanelUI : MonoBehaviour
{
    [Header("References")]
    public PlayerInventory playerInventory;
    public TextMeshProUGUI inventoryRowPrefab;
    public Transform       rowContainer;

    [Header("Toggle")]
    public KeyCode toggleKey = KeyCode.Tab;

    private bool _isOpen = false;

    private void Start()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("InventoryPanelUI: PlayerInventory not assigned.");
            return;
        }

        playerInventory.OnItemCollected += AddInventoryRow;
        //gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
            playerInventory.OnItemCollected -= AddInventoryRow;
    }

    // Input check only.
    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            SetPanelOpen(!_isOpen);
    }

    private void SetPanelOpen(bool open)
    {
        _isOpen = open;
        gameObject.SetActive(open);
    }

    private void AddInventoryRow(Item_Base item)
    {
        if (inventoryRowPrefab == null || rowContainer == null) return;

        TextMeshProUGUI row = Instantiate(inventoryRowPrefab, rowContainer);

        // Use EquipmentItem status text if available, otherwise just itemName
        EquipmentItem ei = item as EquipmentItem;
        row.text = ei != null ? ei.GetStatusText() : item.itemName;
    }
}
