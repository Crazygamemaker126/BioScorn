using UnityEngine;

/// <summary>
/// Attach this to any physical pickup GameObject in the world.
/// Assign the matching ItemBase ScriptableObject asset in the Inspector.
///
/// When the player walks into the trigger collider, CollectItem is called
/// on their PlayerInventory, which delegates to the SO's OnCollected logic.
/// The world object then destroys itself.
/// </summary>
[RequireComponent(typeof(Collider))]
public class WorldItem : MonoBehaviour
{
    [Tooltip("The ScriptableObject that defines what this pickup is and does.")]
    public ItemBase data;

    private void Awake()
    {
        // Make sure the collider is a trigger
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory == null)
        {
            Debug.Log("No PlayerInventory found on: " + other.gameObject.name);
            return;
        }
        
        inventory.CollectItem(data);
        Destroy(gameObject);
    }
}
