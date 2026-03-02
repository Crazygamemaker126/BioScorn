using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "EnemyRuntimeState")]
public class EnemyRuntimeState : ScriptableObject
{
    [Header("Health")]
    public float currHealth;
    public float maxHealth;

    [Header("Awareness")]
    public bool isAggro;
    public bool isSearching;
    public Vector3 lastSeenPosition;

    [Header("Timers")]
    public float timeSinceLastDamage;
    public float decisionCooldownTimer;
    public float attackCooldownTimer;
    public float staggerTimer;

    [Header("Status Flags")]
    public bool isStaggered;
    public bool isRetreating;

    [Header("Events")]
    public UnityEvent onDamaged;
    public UnityEvent onDeath;
    public UnityEvent onAggro;

    public void ResetState()
    {
        currHealth = maxHealth;
        isAggro = false;
        isSearching = false;
        isStaggered = false;
        isRetreating = false;

        timeSinceLastDamage = 0;
        decisionCooldownTimer = 0;
        attackCooldownTimer = 0;
        staggerTimer = 0;

        lastSeenPosition = Vector3.zero;
    }

}
