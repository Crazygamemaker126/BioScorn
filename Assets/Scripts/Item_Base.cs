using UnityEngine;

public abstract class Item_Base : MonoBehaviour, IPickupable
{
    public string itemName;

    public abstract void Pickup();
}
