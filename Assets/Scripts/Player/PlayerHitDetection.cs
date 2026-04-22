using UnityEngine;

public class PlayerHitDetection : MonoBehaviour
{
    private PlayerCombatFeedback _feedback;
    private HealthTracking _health;

    private void Awake()
    {
        _feedback = GetComponent<PlayerCombatFeedback>();
        _health = GetComponent<HealthTracking>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HitBox"))
        {
            int damage = 10; // swap this for actual damage value later

            _health?.OnDamageTaken(damage);
            _feedback?.OnPlayerHitByEnemy(damage, _health?.CurrentHealth ?? 0f);

            Debug.Log("Player was hit.");
        }
    }
}