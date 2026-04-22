using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class HealthTracking : MonoBehaviour
{
   
    [Header("Inspector Variables")]
    public int curHealth;
    public int maxHealth = 100;

    public int keys;
    public int ammo;

    [Header("Elements in Hierarchy")]
    public Slider healthBar;
    public TextMeshProUGUI keysCount;
    public TextMeshProUGUI ammoCount;
    public UnityEvent OnHealthChangedUnity; 
    public event Action<int> OnHealthChanged;

    
    public int CurrentHealth => Mathf.RoundToInt(healthBar != null ? healthBar.value : curHealth);

    
    private void MaxHealthChecker()
    {
        if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Health Pickup"))
        {
            MaxHealthChecker();
        }

        if (other.gameObject.CompareTag("Key"))
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
        curHealth = Mathf.Clamp(Mathf.RoundToInt(healthBar.value), 0, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    
    public void OnDamageTaken(int amount)
    {
        healthBar.value -= amount;
        curHealth = Mathf.Clamp(Mathf.RoundToInt(healthBar.value), 0, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth); 
    }
}
