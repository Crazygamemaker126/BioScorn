using System.Collections;
using UnityEngine;

/// <summary>
/// Attach to any GameObject that should explode when damaged enough.
///
/// When the object's health reaches zero it triggers an explosion that:
///   1. Applies damage to every Collider in the blast radius via Physics.OverlapSphere.
///   2. Applies an AddExplosionForce impulse to every Rigidbody in range.
///   3. Notifies PlayerCombatFeedback if the player is caught in the blast.
///   4. Can trigger OTHER ExplosiveObjects nearby — enabling chain reactions.
///
/// A matching Gizmo is drawn in the Editor by ExplosiveObjectGizmo (Editor script).
/// </summary>
public class Explosive : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 30f;
    private float _currentHealth;

    [Header("Explosion Settings")]
    [Tooltip("World-space radius of the explosion.")]
    public float explosionRadius = 4f;

    [Tooltip("Maximum damage dealt at the center of the blast. Falls off to zero at the edge.")]
    public float maxDamage = 100f;

    [Tooltip("Force applied to Rigidbodies caught in the blast.")]
    public float explosionForce = 500f;

    [Tooltip("Delay in seconds between taking lethal damage and detonating. " +
             "Set to 0 for instant.")]
    public float fuseDelay = 0.1f;

    [Header("Layers")]
    [Tooltip("Which layers can receive explosion damage.")]
    public LayerMask damageMask = ~0; // default: everything

    [Header("VFX (optional)")]
    [Tooltip("Optional particle/effect prefab spawned at the explosion origin.")]
    public GameObject explosionEffectPrefab;

    // ------------------------------------------------------------------ //

    private bool _hasExploded;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    // ------------------------------------------------------------------ //
    //  Public damage entry point                                          //
    // ------------------------------------------------------------------ //

    /// <summary>
    /// Deal damage to this explosive. If health hits zero it begins detonation.
    /// Call this from bullets, melee scripts, or other explosions.
    /// </summary>
    public void TakeDamage(float amount)
    {
        if (_hasExploded) return;

        _currentHealth -= amount;
        Debug.Log($"[Explosive] {name} took {amount} damage ({_currentHealth}/{maxHealth} HP remaining).");

        if (_currentHealth <= 0f)
            StartCoroutine(Detonate());
    }

    // ------------------------------------------------------------------ //
    //  Explosion logic                                                    //
    // ------------------------------------------------------------------ //

    private IEnumerator Detonate()
    {
        if (_hasExploded) yield break;
        _hasExploded = true;

        if (fuseDelay > 0f)
            yield return new WaitForSeconds(fuseDelay);

        Explode();
    }

    private void Explode()
    {
        Debug.Log($"[Explosive] {name} EXPLODED at {transform.position}.");

        // Spawn visual effect
        if (explosionEffectPrefab != null)
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        // Find everything in radius
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, damageMask);

        foreach (Collider hit in hits)
        {
            float dist = Vector3.Distance(transform.position, hit.transform.position);
            float falloff = 1f - Mathf.Clamp01(dist / explosionRadius);   // 1 at center, 0 at edge
            float dmg = maxDamage * falloff;

            // --- Player feedback ---
            PlayerCombatFeedback playerFeedback = hit.GetComponentInParent<PlayerCombatFeedback>();
            if (playerFeedback != null)
            {
                // We need remaining HP — try to find a health component if one exists
                // (adapt the type name to whatever your player health script is called)
                playerFeedback.OnPlayerHitByEnemy(dmg, 0f);
            }

            // --- Chain reaction: damage other explosives ---
            Explosive otherExplosive = hit.GetComponentInParent<Explosive>();
            if (otherExplosive != null && otherExplosive != this && !otherExplosive._hasExploded)
            {
                Debug.Log($"[Explosive] Chain reaction: {name} is triggering {otherExplosive.name}!");
                otherExplosive.TakeDamage(dmg);
            }

            // --- Physics impulse ---
            Rigidbody rb = hit.attachedRigidbody;
            if (rb != null)
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f);

            Debug.Log($"[Explosive] Hit {hit.name} for {dmg:0.0} damage (falloff {falloff:0.00}).");
        }

        // Disable or destroy this object after exploding
        gameObject.SetActive(false);
    }

    // ------------------------------------------------------------------ //
    //  Utility — used by the Editor Gizmo tool                           //
    // ------------------------------------------------------------------ //

    /// <summary>
    /// Returns every ExplosiveObject whose explosion radius overlaps this one.
    /// Used by the Editor tool to preview chain reactions without guessing.
    /// </summary>
    public Explosive[] GetChainCandidates()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);
        var result = new System.Collections.Generic.List<Explosive>();
        foreach (var c in cols)
        {
            Explosive e = c.GetComponentInParent<Explosive>();
            if (e != null && e != this && !result.Contains(e))
                result.Add(e);
        }
        return result.ToArray();
    }
}