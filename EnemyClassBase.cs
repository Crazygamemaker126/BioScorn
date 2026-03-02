using UnityEngine;

[CreateAssetMenu(fileName = "EnemyClassBase", menuName = "BioScorn/EnemyClassBase")]
public class EnemyClassBase : ScriptableObject
{
    [Header("Movement instincts")]
    public float baseMoveSpeed = 2f;
    public float rushSpeed = 4f;
    public float erraticMovementFactor = 0.5f;

    [Header("Combat Instincts")]
    public float meleeAtkRange = 1.5f;
    public float rangedAtkRange = 6f;
    public float attackCooldown = 1.2f;
    public float aggressionLevel = 0.7f;

    [Header("Survival Instincts")]
    public float retreatThreshold = 0.15f; //will flee at 15% health
    public float painResponseDelay = 0.1f;
    public float staggerChance = 0.2f;

    [Header("Awareness Instincts")]
    public float detectionRange = 10f;
    public float curiosityFactor = 0.4f;
    public float decisionCooldown = 0.25f;

    [Header("Heuristics weights")]
    public float distanceWeight = 1f;
    public float healthWeight = 1f;
    public float targetWeaknessWeight = 1f;

     
}
