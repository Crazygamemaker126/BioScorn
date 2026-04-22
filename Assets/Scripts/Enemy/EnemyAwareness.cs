using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Attach to an Enemy GameObject (alongside a NavMeshAgent).
///
/// Handles line-of-sight detection against the player and drives the
/// EnemyRuntimeState awareness flags (isAggro / isSearching).
/// Whenever the enemy spots or loses the player a HUD message is shown so
/// the player always knows the AI's current intent.
///
/// Requires: EnemyClassBase (SO), EnemyRuntimeState (SO), NavMeshAgent.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAwareness : MonoBehaviour
{
    [Header("Data")]
    public EnemyClassBase classData;
    public EnemyRuntimeState runtimeState;

    [Header("Detection")]
    [Tooltip("The Transform of the Player.")]
    public Transform player;

    [Tooltip("Layer mask for line-of-sight raycasts. Should include walls/obstacles.")]
    public LayerMask obstructionMask;

    [Tooltip("How many seconds the enemy searches before giving up.")]
    public float searchDuration = 5f;

    [Header("Feedback Colors")]
    public Color spottedColor = new Color(1f, 0.3f, 0.1f);  // orange-red
    public Color lostColor = new Color(0.6f, 0.85f, 1f); // light blue
    public Color searchingColor = new Color(1f, 0.9f, 0.3f); // yellow

    // ------------------------------------------------------------------ //

    private NavMeshAgent _agent;
    private float _searchTimer;
    private bool _hadLoS;          // LoS state last frame

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (classData == null || runtimeState == null || player == null) return;

        bool canSee = HasLineOfSight();

        // --- Transition: gained LoS ---
        if (canSee && !runtimeState.isAggro)
        {
            BecomeAggro();
        }

        // --- Transition: lost LoS while aggro ---
        if (!canSee && runtimeState.isAggro)
        {
            LosePlayer();
        }

        // --- Tick search timer ---
        if (runtimeState.isSearching)
        {
            _searchTimer -= Time.deltaTime;
            if (_searchTimer <= 0f)
                GiveUpSearch();
        }

        _hadLoS = canSee;
    }

    // ------------------------------------------------------------------ //
    //  State transitions                                                  //
    // ------------------------------------------------------------------ //

    private void BecomeAggro()
    {
        runtimeState.isAggro = true;
        runtimeState.isSearching = false;
        runtimeState.lastSeenPosition = player.position;

        string msg = $"{name} spotted you!";
        FeedbackManager.Instance?.ShowMessage(msg, spottedColor);
        Debug.Log($"[EnemyAwareness] {msg}");

        runtimeState.onAggro?.Invoke();
    }

    private void LosePlayer()
    {
        runtimeState.isAggro = false;
        runtimeState.isSearching = true;
        _searchTimer = searchDuration;
        // lastSeenPosition was already updated each frame while aggro

        string msg = $"{name} lost sight of you — searching...";
        FeedbackManager.Instance?.ShowMessage(msg, lostColor);
        Debug.Log($"[EnemyAwareness] {msg}");
    }

    private void GiveUpSearch()
    {
        runtimeState.isSearching = false;

        string msg = $"{name} gave up the search.";
        FeedbackManager.Instance?.ShowMessage(msg, searchingColor);
        Debug.Log($"[EnemyAwareness] {msg}");
    }

    // ------------------------------------------------------------------ //
    //  Line-of-sight check                                                //
    // ------------------------------------------------------------------ //

    /// <summary>
    /// Returns true when the player is within detection range AND
    /// no obstruction layer geometry blocks the ray.
    /// </summary>
    private bool HasLineOfSight()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > classData.detectionRange) return false;

        Vector3 dir = (player.position - transform.position).normalized;
        return !Physics.Raycast(transform.position, dir, dist, obstructionMask);
    }

    // ------------------------------------------------------------------ //
    //  Called by other systems (patrol, combat) to keep lastSeen fresh   //
    // ------------------------------------------------------------------ //

    /// <summary>Update the last known position while LoS is maintained.</summary>
    public void TrackPlayer()
    {
        if (runtimeState != null && player != null)
            runtimeState.lastSeenPosition = player.position;
    }
}