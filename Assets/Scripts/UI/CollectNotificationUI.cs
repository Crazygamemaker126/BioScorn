using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Shows a brief on-screen notification when the player collects any item.
/// Subscribes to PlayerInventory.OnItemCollected — no Update() polling.
///
/// Works with Item_Base (programmer's system) directly.
/// Uses item.itemName since that's the shared field on Item_Base.
/// Each of the six items gets a unique prompt via their itemName
/// and the optional collectMessage field on EquipmentItem subclasses.
/// </summary>
public class CollectNotificationUI : MonoBehaviour
{
    [Header("References")]
    public PlayerInventory playerInventory;
    public TextMeshProUGUI notificationText;
    public CanvasGroup     notificationCanvasGroup;

    [Header("Settings")]
    public float displayDuration = 2f;
    public float fadeDuration    = 0.5f;

    private Coroutine _activeRoutine;

    private void Start()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("CollectNotificationUI: PlayerInventory not assigned.");
            return;
        }

        playerInventory.OnItemCollected += ShowNotification;

        if (notificationCanvasGroup != null)
            notificationCanvasGroup.alpha = 0f;
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
            playerInventory.OnItemCollected -= ShowNotification;
    }

    private void ShowNotification(Item_Base item)
    {
        if (notificationText == null) return;

        // Check if item has a collectMessage (EquipmentItem subclass)
        // otherwise fall back to itemName
        EquipmentItem ei = item as EquipmentItem;
        notificationText.text = (ei != null && !string.IsNullOrEmpty(ei.collectMessage))
            ? ei.collectMessage
            : $"{item.itemName} collected!";

        if (_activeRoutine != null)
            StopCoroutine(_activeRoutine);

        _activeRoutine = StartCoroutine(NotificationRoutine());
    }

    private IEnumerator NotificationRoutine()
    {
        if (notificationCanvasGroup != null) notificationCanvasGroup.alpha = 1f;

        yield return new WaitForSeconds(displayDuration);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            if (notificationCanvasGroup != null)
                notificationCanvasGroup.alpha = 1f - (elapsed / fadeDuration);
            yield return null;
        }

        if (notificationCanvasGroup != null) notificationCanvasGroup.alpha = 0f;
        _activeRoutine = null;
    }
}
