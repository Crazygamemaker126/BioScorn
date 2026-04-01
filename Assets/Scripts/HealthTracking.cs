using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthTracking : MonoBehaviour //ENTIRE THING NEEDS MAJOR REFINING.
{
    [Header("Inspector Variables")]
    public int curHealth; //Figure out how to Health into a Vector2. not super important, still a good idea.
    public int maxHealth = 100;

    public int keys;
    public int ammo;


    [Header("Elements in Hierarchy")]
    public Slider healthBar;
    public TextMeshProUGUI keysCount;
    public TextMeshProUGUI ammoCount;
    public UnityEvent OnHealthChanged; //Figure out how to use this for proper health tracking
    



    private void MaxHealthChecker()
    {
        if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Health Pickup"))
        {
            //OnHealthIncreased(); //Needs reference to Health Pickup Item. 
            MaxHealthChecker();

        }

        if(other.gameObject.CompareTag("Key"))
        {
            keys++;
            keysCount.text = keys.ToString("Keys: ");
        }

        if (other.gameObject.CompareTag("Ammo"))
        {
            ammo++;
            ammoCount.text = ammo.ToString("Ammo: ");
        }

        
    }


    public void OnHealthIncreased(int amount)
    {
        healthBar.value += amount;
        
    }

    public void OnDamageTaken(int amount)
    {
        healthBar.value -= amount;
    }

    


}
