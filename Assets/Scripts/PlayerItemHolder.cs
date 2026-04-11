
using UnityEngine;

public class PlayerItemHolder : MonoBehaviour
{
    public bool hasItem = false;

    public void GiveItem()
    {
        hasItem = true;
        Debug.Log("Item collected!");
    }

    public void RemoveItem()
    {
        hasItem = false;
    }
}
