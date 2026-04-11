using UnityEngine;

public class BarrelDestroyZone : MonoBehaviour
{
    public GameObject barrel;
    public UIManager ui;

    private PlayerItemHolder playerInZone;

    private bool canUseZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = other.GetComponent<PlayerItemHolder>();
            canUseZone = true;

            if (playerInZone != null && playerInZone.hasItem)
            {
                ui.ShowMessage("Press E to use item");
            }
            else
            {
                ui.ShowMessage("You need an item first");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = null;
            canUseZone = false;
            ui.HideMessage();
        }
    }

    private void Update()
    {
        if (canUseZone && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInZone != null && playerInZone.hasItem)
            {
                playerInZone.hasItem = false;

                Destroy(barrel);
                ui.ShowMessage("Boom! Item used");
            }
            else
            {
                ui.ShowMessage("You don't have the item");
            }
        }
    }
}