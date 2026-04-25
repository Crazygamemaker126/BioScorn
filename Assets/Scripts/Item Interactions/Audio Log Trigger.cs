using UnityEngine;

[RequireComponent(typeof(WorldItem))]
public class AudioLogTrigger : MonoBehaviour
{
    [Header("Optional Visual")]
    [Tooltip("Assign a child GameObject (e.g. a glow effect) to disable on pickup.")]
    public GameObject collectibleVisual;

    private WorldItem _worldItem;

    private void Awake()
    {
        _worldItem = GetComponent<WorldItem>();
    }

    // Example: disable a visual effect the moment the log is picked up.
    // WorldItem.OnTriggerEnter calls CollectItem then Destroy(gameObject),
    // so this fires just before the GameObject is destroyed.
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerInventory>() == null) return;

        if (collectibleVisual != null)
            collectibleVisual.SetActive(false);

        // Add any other one-shot pickup effects here (particle burst, SFX, etc.)
    }
}