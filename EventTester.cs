using UnityEngine;

public class EventTester : MonoBehaviour
{
    public void OnAggroTriggered()
    {
        Debug.Log("Aggro triggered");
    }

    public void OnDamagedTriggered()
    {
        Debug.Log("Damage taken");
    }

    public void OnDeathTriggered()
    {
        Debug.Log("Enemy dead");
    }
}
