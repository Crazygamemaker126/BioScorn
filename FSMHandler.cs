using UnityEngine;
using UnityEngine.Events;


public class FSMHandler : MonoBehaviour
{
    public HeuristicEvaluator evaluator;
    public EnemyClassBase classBase;
    public EnemyRuntimeState runtime;


    public enum State
    {
        Idle,
        Rush,
        MeleeAttack,
        RangedAttack,
        Retreat,
        Search
    }

    public State currentState;

    private void Start()
    {
        currentState = State.Idle;
    }

    private void Update()
    {
        runtime.timeSinceLastDamage += Time.deltaTime;
        runtime.decisionCooldownTimer -= Time.deltaTime;
        runtime.attackCooldownTimer -= Time.deltaTime;
        runtime.staggerTimer -= Time.deltaTime;

        if (runtime.decisionCooldownTimer <= 0f)
        {
            var decision = evaluator.Evaluate(GetPlayerPosition());
            TransitionTo(decision);
            runtime.decisionCooldownTimer = classBase.decisionCooldown;

        }
        ExecuteState();


    }

    public void ApplyDamage(float amount)
    {
        runtime.currHealth -= amount;
        runtime.onDamaged?.Invoke();

        if (runtime.currHealth <= 0f)
        {
            runtime.onDeath?.Invoke();
        }



    }
    public void TransitionTo(HeuristicEvaluator.Decision decision)
    {
        switch (decision)
        {
            case HeuristicEvaluator.Decision.Idle:
                currentState = State.Idle;
                break;

            case HeuristicEvaluator.Decision.Rush:
                currentState = State.Rush;
                break;

            case HeuristicEvaluator.Decision.MeleeAttack:
                currentState = State.MeleeAttack;
                break;


            case HeuristicEvaluator.Decision.RangedAttack:
                currentState = State.RangedAttack;
                break;

            case HeuristicEvaluator.Decision.Retreat:
                currentState = State.Retreat;
                break;

            case HeuristicEvaluator.Decision.Search:
                currentState = State.Search;
                break;

        }
    }

    public void TriggerAggro()
    {
        runtime.isAggro = true;
        runtime.onAggro?.Invoke();
    }

    public void TriggerDeath()
    {
        runtime.onDeath?.Invoke();
    }

    public void TriggerDamage()
    {
        runtime.onDamaged?.Invoke();
    }

    public void ExecuteState()
    {
        //switch (currentState)
        {
            /*case State.Idle:
                Idle();
                break;
            case State.Rush:
                Rush();
                break;
            case State.MeleeAttack:
                MeleeAttack();
                break;
            case State.RangedAttack:
                RangedAttack();
                break;
            case State.Retreat:
                Retreat();
                break;
            case State.Search:
                SearchForPlayer();
                break;*/
        }
    }


    //Placeholders below, not meant to do anything yet

    /*public void Idle();
    public void Rush();
    public void MeleeAttack();
    public void RangedAttack();
    public void Retreat();
    public void SearchForPlayer();*/

    Vector3 GetPlayerPosition()
    {
        //This will contain player location reference data
        return Vector3.zero;
    }


}

