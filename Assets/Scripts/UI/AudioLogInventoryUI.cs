using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Standalone Audio Log inventory panel.
/// Press Tab to open/close. Pauses the game while open via Time.timeScale.
///
/// Setup (Inspector):
///   - playerInventory     : your PlayerInventory component
///   - logButtonPrefab     : a Button prefab with a child TextMeshProUGUI
///   - logListContainer    : the ScrollRect's content transform (left column)
///   - subtitleDisplayText : TMP for the full transcript (right column)
///   - logTitleText        : (optional) TMP showing the selected log's name
///   - emptyStateText      : (optional) TMP shown when no logs collected yet
/// </summary>
public class AudioLogInventoryUI : MonoBehaviour
{
    [Header("References")]
    public PlayerInventory playerInventory;
    public Button logButtonPrefab;
    public Transform logListContainer;
    public TextMeshProUGUI subtitleDisplayText;
    public TextMeshProUGUI logTitleText;
    public TextMeshProUGUI emptyStateText;

    [Header("HUD Subtitle Panel")]
    [Tooltip("Assign your AudioLogUIHandler GameObject here so it hides while the inventory is open.")]
    public GameObject subtitlePanel;

    [Header("Toggle")]
    public KeyCode toggleKey = KeyCode.Tab;

    // Runtime state
    private bool _isOpen = false;
    public bool IsOpen => _isOpen;
    private readonly List<GameObject> _buttons = new List<GameObject>();
    private readonly List<AudioLogData> _collectedLogs = new List<AudioLogData>();
    private int _selectedIndex = -1;

    // ── Lifecycle ────────────────────────────────────────────────────────

    private void Start()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("AudioLogInventoryUI: PlayerInventory not assigned.");
            return;
        }

        playerInventory.OnAudioLogCollected += OnLogCollected;

        ClearDetail();
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
            playerInventory.OnAudioLogCollected -= OnLogCollected;

        // Safety: restore timescale if destroyed while open
        if (_isOpen)
            Time.timeScale = 1f;
    }

    // ── Input ────────────────────────────────────────────────────────────

    

    // ── Open / close ─────────────────────────────────────────────────────

    public void SetPanelOpen(bool open)
    {
        _isOpen = open;
        gameObject.SetActive(open);
        Time.timeScale = open ? 0f : 1f;

        // Hide the HUD subtitle panel while the inventory is open
        if (subtitlePanel != null)
            subtitlePanel.SetActive(!open);

        if (open)
            RefreshList();
    }

    // ── Inventory event ──────────────────────────────────────────────────

    private void OnLogCollected(AudioLogData log)
    {
        if (!_collectedLogs.Contains(log))
            _collectedLogs.Add(log);
    }

    // ── List building ────────────────────────────────────────────────────

    private void RefreshList()
    {
        foreach (GameObject btn in _buttons)
            Destroy(btn);
        _buttons.Clear();

        bool hasLogs = _collectedLogs.Count > 0;

        if (emptyStateText != null)
            emptyStateText.gameObject.SetActive(!hasLogs);

        if (!hasLogs)
        {
            ClearDetail();
            return;
        }

        for (int i = 0; i < _collectedLogs.Count; i++)
        {
            AudioLogData log = _collectedLogs[i];
            int capturedIndex = i;

            Button btn = Instantiate(logButtonPrefab, logListContainer);

            TextMeshProUGUI label = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (label != null)
                label.text = log.itemName;

            btn.onClick.AddListener(() => SelectLog(capturedIndex));
            _buttons.Add(btn.gameObject);
        }

        // Select whichever was last open, or default to first
        int selectTarget = (_selectedIndex >= 0 && _selectedIndex < _collectedLogs.Count)
            ? _selectedIndex
            : 0;

        SelectLog(selectTarget);
    }

    private void SelectLog(int index)
    {
        if (index < 0 || index >= _collectedLogs.Count) return;

        _selectedIndex = index;
        AudioLogData log = _collectedLogs[index];

        if (logTitleText != null)
            logTitleText.text = log.itemName;

        if (subtitleDisplayText != null)
            subtitleDisplayText.text = string.IsNullOrEmpty(log.subtitleText)
                ? "<i>No transcript available.</i>"
                : log.subtitleText;

        // Highlight selected button
        for (int i = 0; i < _buttons.Count; i++)
        {
            Button btn = _buttons[i].GetComponent<Button>();
            if (btn == null) continue;

            ColorBlock colors = btn.colors;
            colors.normalColor = (i == index) ? new Color(0.85f, 0.75f, 0.35f) : Color.white;
            btn.colors = colors;
        }
    }

    private void ClearDetail()
    {
        _selectedIndex = -1;

        if (logTitleText != null)
            logTitleText.text = string.Empty;

        if (subtitleDisplayText != null)
            subtitleDisplayText.text = string.Empty;
    }
}