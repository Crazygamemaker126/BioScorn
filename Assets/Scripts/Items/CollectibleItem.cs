// CollectibleItem.cs
using NUnit.Framework.Interfaces;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollectibleItem : MonoBehaviour
{
    [SerializeField] ItemBase itemData;
    [SerializeField] PlayerInventory inventory;

    void Start()
    {
        // Ensure the collider is a trigger
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        itemData.OnCollected(inventory); // polymorphic — no switch/if chain
        Destroy(gameObject);
    }
}