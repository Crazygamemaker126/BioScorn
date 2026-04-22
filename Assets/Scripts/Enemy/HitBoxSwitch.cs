using UnityEngine;

public class HitBoxSwitch : MonoBehaviour
{
    public Collider hitBox;

    public void HitBoxOn()
    { 
       hitBox.enabled = true; 
    }

    public void HitBoxOff()
    {
        hitBox.enabled = false;
    }
}
