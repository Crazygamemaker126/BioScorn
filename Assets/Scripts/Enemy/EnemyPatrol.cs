using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Attach to an enemy that has a NavMeshAgent.
/// Moves the agent through an ordered list of waypoints, switching between
/// them automatically.  Works alongside EnemyAwareness — patrol pauses
/// while the enemy is aggro or searching, and resumes when calm.
///
/// Patrol modes:
///   Loop      — 0 → 1 → 2 → 0 → 1 … (circular)
///   PingPong  — 0 → 1 → 2 → 1 → 0 → 1 … (back-and-forth)
///
/// The patrol path is visualised in the Editor by EnemyPatrolGizmo.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPatrol : MonoBehaviour
{
    // ------------------------------------------------------------------ //
    //  Inspector                                                          //
    // ------------------------------------------------------------------ //

    [Header("Waypoints")]
    [Tooltip("Ordered list of patrol positions. Add Transform references or use " +
             "empty GameObjects as markers. At least 2 waypoints are needed.")]
    public Transform[] waypoints;

    [Header("Patrol Settings")]
    [Tooltip("How close the agent must get to a waypoint before moving to the next (metres).")]
    public float arrivalThreshold = 0.5f;

    [Tooltip("How long (seconds) the enemy waits at each waypoint before moving on.")]
    public float waitTime = 1f;

    public enum PatrolMode { Loop, PingPong }
    [Tooltip("Loop: circles the route. PingPong: reverses direction at each end.")]
    public PatrolMode patrolMode = PatrolMode.Loop;

    [Header("References")]
    [Tooltip("Optional — if assigned, patrol will pause while the enemy is aggro/searching.")]
    public EnemyAwareness awarenessModule;

    // ------------------------------------------------------------------ //
    //  Private state                                                      //
    // ------------------------------------------------------------------ //

    private NavMeshAgent _agent;
    private int _currentIndex;
    private int _direction = 1;     // +1 or -1 for PingPong
    private float _waitTimer;
    private bool _waiting;

    // ------------------------------------------------------------------ //
    //  Unity lifecycle                                                    //
    // ------------------------------------------------------------------ //

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (waypoints == null || waypoints.Length < 2)
        {
            Debug.LogWarning($"[EnemyPatrol] {name}: needs at least 2 waypoints. Patrol disabled.");
            enabled = false;
            return;
        }

        GoToWaypoint(_currentIndex);
    }

    private void Update()
    {
        // Pause patrol when enemy is alert
        if (awarenessModule != null &&
            (awarenessModule.runtimeState.isAggro || awarenessModule.runtimeState.isSearching))
            return;

        if (_waiting)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
            {
                _waiting = false;
                AdvanceWaypoint();
                GoToWaypoint(_currentIndex);
            }
            return;
        }

        // Check arrival
        if (!_agent.pathPending && _agent.remainingDistance <= arrivalThreshold)
        {
            _waiting = true;
            _waitTimer = waitTime;
        }
    }

    // ------------------------------------------------------------------ //
    //  Waypoint navigation                                                //
    // ------------------------------------------------------------------ //

    private void GoToWaypoint(int index)
    {
        if (waypoints[index] == null)
        {
            Debug.LogWarning($"[EnemyPatrol] {name}: waypoint {index} is null, skipping.");
            AdvanceWaypoint();
            return;
        }

        _agent.SetDestination(waypoints[index].position);
        Debug.Log($"[EnemyPatrol] {name} heading to waypoint {index} ({waypoints[index].name}).");
    }

    private void AdvanceWaypoint()
    {
        if (patrolMode == PatrolMode.Loop)
        {
            _currentIndex = (_currentIndex + 1) % waypoints.Length;
        }
        else // PingPong
        {
            _currentIndex += _direction;
            if (_currentIndex >= waypoints.Length - 1 || _currentIndex <= 0)
                _direction = -_direction;
            _currentIndex = Mathf.Clamp(_currentIndex, 0, waypoints.Length - 1);
        }
    }

    // ------------------------------------------------------------------ //
    //  Public utility (used by the Editor Gizmo)                         //
    // ------------------------------------------------------------------ //

    /// <summary>
    /// Returns the ordered list of waypoint positions for Gizmo drawing.
    /// Skips null entries so the Gizmo never throws.
    /// </summary>
    public Vector3[] GetWaypointPositions()
    {
        if (waypoints == null) return new Vector3[0];

        var valid = new System.Collections.Generic.List<Vector3>();
        foreach (var wp in waypoints)
            if (wp != null) valid.Add(wp.position);
        return valid.ToArray();
    }
}