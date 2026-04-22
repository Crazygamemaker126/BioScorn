using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Singleton HUD feedback manager.
/// Any script can call FeedbackManager.Instance.ShowMessage(...) to display
/// a timed on-screen message.
///
/// Setup:
///   1. Attach this script to your Canvas GameObject.
///   2. Assign the TextMeshProUGUI child to the feedbackText slot.
///   3. In Edit > Project Settings > Script Execution Order, set
///      FeedbackManager to -100 so it always Awakes before other scripts.
/// </summary>
public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance { get; private set; }

    [Header("HUD References")]
    [Tooltip("The TextMeshProUGUI element that displays feedback messages.")]
    public TextMeshProUGUI feedbackText;

    [Header("Display Settings")]
    [Tooltip("How long (seconds) each message stays visible.")]
    public float messageDuration = 2f;

    [Tooltip("How quickly the message fades out after its duration.")]
    public float fadeDuration = 0.4f;

    private Coroutine _activeRoutine;

    private void Awake()
    {
        // If another instance already exists, just remove this duplicate
        // but do NOT destroy the whole GameObject (it's a Canvas with children).
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[FeedbackManager] Duplicate detected — removing extra component.");
            Destroy(this);
            return;
        }

        Instance = this;

        // Validate the text reference immediately so any misconfiguration is
        // caught at startup rather than silently failing mid-game.
        if (feedbackText == null)
            Debug.LogError("[FeedbackManager] feedbackText is not assigned! " +
                           "Drag a TextMeshProUGUI into the slot in the Inspector.");
        else
        {
            // Hide by setting alpha to 0 — never call SetActive(false) here.
            // If feedbackText is a child, deactivating it means SetActive(true)
            // in DisplayRoutine can never bring it back (child can't reactivate
            // itself when a parent is inactive).
            Color c = feedbackText.color;
            c.a = 0f;
            feedbackText.color = c;
        }
    }

    private void OnDestroy()
    {
        // Clear the static reference when this object is destroyed
        // so a fresh scene can register a new instance correctly.
        if (Instance == this)
            Instance = null;
    }

    // ------------------------------------------------------------------ //
    //  Public API                                                         //
    // ------------------------------------------------------------------ //

    /// <summary>
    /// Display a message on the HUD. Interrupts any currently showing message.
    /// </summary>
    /// <param name="message">Text to display.</param>
    /// <param name="color">Optional tint color (defaults to white).</param>
    public void ShowMessage(string message, Color? color = null)
    {
        Debug.Log($"[FeedbackManager] ShowMessage called: \"{message}\"");

        if (feedbackText == null)
        {
            Debug.LogError("[FeedbackManager] feedbackText is NULL — assign it in the Inspector.");
            return;
        }

        Debug.Log($"[FeedbackManager] feedbackText OK. Canvas enabled: {GetComponent<Canvas>()?.isActiveAndEnabled}, " +
                  $"this GameObject active: {gameObject.activeInHierarchy}");

        if (_activeRoutine != null)
            StopCoroutine(_activeRoutine);

        _activeRoutine = StartCoroutine(DisplayRoutine(message, color ?? Color.white));
    }

    // ------------------------------------------------------------------ //
    //  Internal                                                           //
    // ------------------------------------------------------------------ //

    private IEnumerator DisplayRoutine(string message, Color color)
    {
        // Show — set alpha to 1, never touch SetActive so parent hierarchy stays intact
        color.a = 1f;
        feedbackText.text = message;
        feedbackText.color = color;

        yield return new WaitForSeconds(messageDuration);

        // Fade out smoothly by reducing alpha to 0
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            feedbackText.color = color;
            yield return null;
        }

        // Fully hidden — alpha 0, GameObject stays active
        color.a = 0f;
        feedbackText.color = color;
        _activeRoutine = null;
    }
}
