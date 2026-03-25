using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class HealthTracking : MonoBehaviour
{

    public int curHealth = 100;
    public int maxHealth = 100;

    public int keys;
    public int ammo;
    //Figure out how to Health into a Vector2. not super important, still a good idea.

    private Slider HealthBar;

    private UnityEvent _HealthChanged;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Health Pickup"))
        {
            _HealthChanged?.Invoke(); //Figure out amount to heal by. IMPORTANT. WILL ALTER SLIDER.
        }

        
    }



    //Need to add processing for actual Health Updates!!! IMPORTANT.


}
