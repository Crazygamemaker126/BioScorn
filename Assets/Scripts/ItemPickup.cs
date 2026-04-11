using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public UIManager ui;
    private bool playerInRange = false;
    private PlayerItemHolder player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.GetComponent<PlayerItemHolder>();

            if (player != null)
            {
                ui.ShowMessage("Press E to pick up item");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            ui.HideMessage();
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (player != null)
            {
                player.GiveItem();
                ui.ShowMessage("Item collected!");
                Destroy(gameObject);
            }
        }
    }
}