#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor-only Gizmo companion for EnemyPatrol.
///
/// What it draws in the Scene view:
///   • A numbered sphere at each waypoint so you can see the order at a glance.
///   • Arrows connecting waypoints in patrol order — direction is always clear.
///   • A final arrow looping back to waypoint 0 when Loop mode is active,
///     OR a double-headed indicator at each end for PingPong.
///   • A label showing the patrol mode above the enemy so you know what
///     behaviour to expect without opening the Inspector.
///
/// No MonoBehaviour is needed — this is a pure Editor companion.
/// It draws automatically whenever the enemy (or any object in the scene) is selected,
/// and faintly when unselected, so you can always see the patrol layout.
/// </summary>
[CustomEditor(typeof(EnemyPatrol))]
public class EnemyPatrolGizmo : Editor
{
    // Visual settings — tweak these constants to taste
    private const float WaypointSphereRadius = 0.25f;
    private const float ArrowHeadLength = 0.35f;
    private const float ArrowHeadAngle = 25f;

    private static readonly Color WaypointColor = new Color(0.2f, 0.8f, 1f, 1f);   // cyan
    private static readonly Color LineColor = new Color(0.2f, 0.8f, 1f, 0.8f);
    private static readonly Color LoopBackColor = new Color(1f, 0.6f, 0.1f, 0.7f); // orange = "loop back"
    private static readonly Color LabelBackground = new Color(0f, 0f, 0f, 0.55f);

    // ------------------------------------------------------------------ //
    //  Custom Inspector                                                   //
    // ------------------------------------------------------------------ //

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnemyPatrol patrol = (EnemyPatrol)target;

        EditorGUILayout.Space(6);
        EditorGUILayout.HelpBox(
            "Patrol path is drawn in the Scene view.\n" +
            "• Cyan spheres = waypoints (numbered in order).\n" +
            "• Cyan arrows  = patrol direction.\n" +
            (patrol.patrolMode == EnemyPatrol.PatrolMode.Loop
                ? "• Orange arrow = loop-back to waypoint 0."
                : "• Patrol reverses direction at each end (PingPong)."),
            MessageType.Info);
    }

    // ------------------------------------------------------------------ //
    //  Scene-view drawing                                                 //
    // ------------------------------------------------------------------ //

    private void OnSceneGUI()
    {
        EnemyPatrol patrol = (EnemyPatrol)target;
        Vector3[] pts = patrol.GetWaypointPositions();

        if (pts == null || pts.Length < 2) return;

        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

        // Draw each waypoint sphere + index label
        for (int i = 0; i < pts.Length; i++)
        {
            Handles.color = WaypointColor;
            Handles.SphereHandleCap(0, pts[i], Quaternion.identity,
                WaypointSphereRadius * 2f, EventType.Repaint);

            // Index label
            Handles.Label(pts[i] + Vector3.up * 0.5f,
                $"  WP {i}", EditorStyles.whiteBoldLabel);
        }

        // Draw arrows between consecutive waypoints
        Handles.color = LineColor;
        for (int i = 0; i < pts.Length - 1; i++)
            DrawArrow(pts[i], pts[i + 1]);

        // Loop-back arrow (only in Loop mode)
        if (patrol.patrolMode == EnemyPatrol.PatrolMode.Loop)
        {
            Handles.color = LoopBackColor;
            DrawArrow(pts[pts.Length - 1], pts[0]);
        }
        else
        {
            // PingPong: draw a small "reverse" indicator at first and last waypoints
            Handles.color = LoopBackColor;
            DrawReverseIndicator(pts[0]);
            DrawReverseIndicator(pts[pts.Length - 1]);
        }

        // Mode label above the enemy
        GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel);
        labelStyle.normal.textColor = Color.white;
        Handles.Label(patrol.transform.position + Vector3.up * 2.2f,
            $"  Patrol: {patrol.patrolMode}", labelStyle);
    }

    // ------------------------------------------------------------------ //
    //  Drawing helpers                                                    //
    // ------------------------------------------------------------------ //

    private static void DrawArrow(Vector3 from, Vector3 to)
    {
        Handles.DrawLine(from, to);

        // Arrowhead at the midpoint so it never overlaps the sphere
        Vector3 mid = Vector3.Lerp(from, to, 0.55f);
        Vector3 dir = (to - from).normalized;
        DrawArrowHead(mid, dir);
    }

    private static void DrawArrowHead(Vector3 tip, Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion rot = Quaternion.LookRotation(direction);
        Vector3 left = rot * Quaternion.Euler(0, ArrowHeadAngle, 0) * Vector3.back * ArrowHeadLength;
        Vector3 right = rot * Quaternion.Euler(0, -ArrowHeadAngle, 0) * Vector3.back * ArrowHeadLength;

        Handles.DrawLine(tip, tip + left);
        Handles.DrawLine(tip, tip + right);
    }

    /// <summary>Draws a small double-arrow indicating "patrol reverses here".</summary>
    private static void DrawReverseIndicator(Vector3 pos)
    {
        float r = 0.4f;
        Handles.DrawWireArc(pos, Vector3.up, Vector3.right, 180f, r);
        Handles.DrawWireArc(pos, Vector3.up, Vector3.left, 180f, r);
    }
}
#endif