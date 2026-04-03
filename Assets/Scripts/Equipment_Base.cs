using UnityEngine;

public class Equipment_Base : Item_Base
{
    
    public Transform sceneObject;
    public override void Pickup()
    {
        sceneObject.gameObject.SetActive(true);
        Destroy(gameObject);
    }
    public virtual void Use()
    { 
        
    }
    
      
    
}
