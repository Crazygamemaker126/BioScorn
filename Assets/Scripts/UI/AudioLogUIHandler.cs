using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles audio playback and subtitle display for collected audio logs.
/// 
/// Setup:
///   1. Attach to a persistent UI GameObject (e.g. your HUD root).
///   2. Assign playerInventory, audioSubtitles, and subtitleCanvasGroup
///      in the Inspector.
///   3. This component owns the single AudioSource used for all log playback.
/// </summary>
public class AudioLogUIHandler : MonoBehaviour
{
    [Header("References")]
    public PlayerInventory playerInventory;
    public TextMeshProUGUI audioSubtitles;
    public CanvasGroup subtitleCanvasGroup;

    [Header("Fade Settings")]
    public float fadeInDuration = 0.3f;
    public float fadeOutDuration = 0.5f;

    // Owned AudioSource — no AudioSource needed on the ScriptableObject
    private AudioSource _audioSource;
    private Coroutine _subtitleRoutine;

    // ── Lifecycle ────────────────────────────────────────────────────────────

    private void Awake()
    {
        // Create a dedicated AudioSource on this GameObject
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;

        // Hide subtitles on start
        if (subtitleCanvasGroup != null)
            subtitleCanvasGroup.alpha = 0f;
    }

    private void Start()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("AudioLogUIHandler: PlayerInventory not assigned.");
            return;
        }

        playerInventory.OnAudioLogCollected += OnAudioLogCollected;
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
            playerInventory.OnAudioLogCollected -= OnAudioLogCollected;
    }

    // ── Playback ─────────────────────────────────────────────────────────────

    private void OnAudioLogCollected(AudioLogData log)
    {
        if (log.audioClip == null)
        {
            Debug.LogWarning($"AudioLogUIHandler: '{log.itemName}' has no AudioClip assigned.");
            return;
        }

        // If a log is already playing, stop it cleanly before starting the new one
        if (_subtitleRoutine != null)
            StopCoroutine(_subtitleRoutine);

        _audioSource.Stop();
        _audioSource.clip = log.audioClip;
        _audioSource.Play();

        _subtitleRoutine = StartCoroutine(SubtitleRoutine(log));
    }

    // ── Subtitle coroutine ───────────────────────────────────────────────────

    private IEnumerator SubtitleRoutine(AudioLogData log)
    {
        // Set text
        if (audioSubtitles != null)
            audioSubtitles.text = log.subtitleText;

        // Fade in
        yield return StartCoroutine(FadeSubtitles(0f, 1f, fadeInDuration));

        // Wait for the clip to finish
        yield return new WaitForSeconds(log.audioClip.length);

        // Fade out
        yield return StartCoroutine(FadeSubtitles(1f, 0f, fadeOutDuration));

        if (audioSubtitles != null)
            audioSubtitles.text = string.Empty;

        _subtitleRoutine = null;
    }

    private IEnumerator FadeSubtitles(float from, float to, float duration)
    {
        if (subtitleCanvasGroup == null) yield break;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            subtitleCanvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        subtitleCanvasGroup.alpha = to;
    }
}
