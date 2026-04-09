using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;
using Unity.VisualScripting;
using System;

public class FSM : MonoBehaviour
{
    public enum States {
        Patrol,
        Chase,
        Attacking
    }

    public States currentState = States.Patrol;
    public NavMeshAgent agent;
    public List<Transform> waypoints = new List<Transform>();
    public Animator animator;
    public FOV fov;
    private int waypointIndex;
    private Coroutine attackCoro;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Act());
    }

    private IEnumerator Act()
    {
        while (true)
        {
            switch (currentState)
            {
                case States.Patrol:
                    PatrolAction();
                    animator.SetFloat("vertical", 1);
                    break;

                case States.Chase:
                    ChaseAction();
                    animator.SetFloat("vertical", 2);
                    break;

                case States.Attacking:
                    AttackAction();
                    animator.SetFloat("vertical", 0);
                    break;

            
            }
            yield return null;
        }
    }

    private void AttackAction()
    {

        if (attackCoro == null)
        {
            attackCoro = StartCoroutine(AttackCoro());
        }
    }

    private IEnumerator AttackCoro()
    {
        animator.Play("Attack");
        yield return new WaitForSeconds(1f);
        currentState = States.Chase;
        fov.target = null;
        attackCoro = null;
    }

    private void ChaseAction()
    {
        if (fov.target == null)
        {
            currentState = States.Patrol;
            agent.SetDestination(waypoints[0].position);
        }
        else
        {
            agent.SetDestination(fov.target.transform.position);
        }

        if (fov.target != null)
        {
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                currentState = States.Attacking;
            }
        }
    
    }

    private void PatrolAction()
    {
        if (fov.target != null)
        {
            currentState = States.Chase;
            return;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Count;
            agent.SetDestination(waypoints[waypointIndex].position);
        }
    }
}
