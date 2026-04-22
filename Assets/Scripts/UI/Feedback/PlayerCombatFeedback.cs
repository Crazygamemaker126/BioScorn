using UnityEngine;

/// <summary>
/// Attach to the Player GameObject.
///
/// Provides two public methods to be called by whatever handles combat:
///   - OnPlayerHitTarget(string targetName, float damage)  : player lands a hit
///   - OnPlayerHitByEnemy(float damage, float remainingHP) : player takes a hit
///
/// Each call pushes a timed message to FeedbackManager so the player always
/// has clear, immediate feedback without needing to watch the console.
/// </summary>
public class PlayerCombatFeedback : MonoBehaviour
{
    [Header("Message Colors")]
    [Tooltip("Color used when the player successfully hits a target.")]
    public Color hitColor = new Color(1f, 0.85f, 0f);      // gold

    [Tooltip("Color used when the player takes damage.")]
    public Color damagedColor = new Color(1f, 0.2f, 0.2f); // red

    [Tooltip("Color used when the player kills an enemy.")]
    public Color killColor = new Color(0.4f, 1f, 0.4f);    // green

    // ------------------------------------------------------------------ //
    //  Public API — call these from your weapon / health scripts          //
    // ------------------------------------------------------------------ //

    /// <summary>
    /// Call this when the player's attack lands on a target.
    /// </summary>
    /// <param name="targetName">Name of the object that was hit.</param>
    /// <param name="damage">How much damage was dealt.</param>
    public void OnPlayerHitTarget(string targetName, float damage)
    {
        string msg = $"Hit {targetName}! -{damage:0} HP";
        FeedbackManager.Instance?.ShowMessage(msg, hitColor);
        Debug.Log($"[Player] {msg}");
    }

    /// <summary>
    /// Call this when the player kills an enemy.
    /// </summary>
    /// <param name="targetName">Name of the defeated enemy.</param>
    public void OnPlayerKilledTarget(string targetName)
    {
        string msg = $"{targetName} defeated!";
        FeedbackManager.Instance?.ShowMessage(msg, killColor);
        Debug.Log($"[Player] {msg}");
    }

    /// <summary>
    /// Call this when the player takes damage from any source.
    /// </summary>
    /// <param name="damage">Damage amount received.</param>
    /// <param name="remainingHP">Player's HP after the hit.</param>
    public void OnPlayerHitByEnemy(float damage, float remainingHP)
    {
        string msg = $"Ouch! -{damage:0} HP  (HP: {remainingHP:0})";
        FeedbackManager.Instance?.ShowMessage(msg, damagedColor);
        Debug.Log($"[Player] {msg}");
    }

    /// <summary>
    /// Call this when the player is killed.
    /// </summary>
    public void OnPlayerDied()
    {
        string msg = "You were defeated...";
        FeedbackManager.Instance?.ShowMessage(msg, damagedColor);
        Debug.Log("[Player] Player died.");
    }
}