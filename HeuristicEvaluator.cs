using UnityEngine;

public class HeuristicEvaluator : MonoBehaviour
{
    public EnemyClassBase classBase;
    public EnemyRuntimeState runtime;

    public enum Decision
    {
        Idle,
        Rush,
        MeleeAttack,
        RangedAttack,
        Retreat,
        Search
    }

    public Decision Evaluate(Vector3 naajPosition)
    {
        float distance = Vector3.Distance(transform.position, naajPosition);

        //Retreat logic
        if (runtime.currHealth <= runtime.maxHealth * classBase.retreatThreshold)
            return Decision.Retreat;

        //Melee attack logic
        if (distance <= classBase.meleeAtkRange)
            return Decision.MeleeAttack;

        //Ranged attack logic
        if (distance <= classBase.rangedAtkRange)
            return Decision.RangedAttack;

        //Rush logic
        if (distance <= classBase.detectionRange)
            return Decision.Rush;

        //searchLogic
        if (runtime.isSearching)
            return Decision.Search;

        return Decision.Idle;
    }
}
