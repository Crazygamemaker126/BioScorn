using UnityEngine;

public class DestroyBarrelZone : MonoBehaviour
{
    public GameObject barrel;

    private void OnTriggerEnter(Collider other)
    {
        PlayerItemHolder inventory = other.GetComponent<PlayerItemHolder>();

        if (inventory != null && inventory.hasItem)
        {
            Destroy(barrel);
            Debug.Log("Barrel destroyed!");
        }
    }
}