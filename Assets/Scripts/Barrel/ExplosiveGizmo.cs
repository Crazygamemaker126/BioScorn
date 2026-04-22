#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor-only Gizmo companion for ExplosiveObject.
///
/// What it draws in the Scene view:
///   • A solid-color wire sphere showing the exact explosion radius.
///   • A distinct "danger zone" disc on the ground plane so it's obvious
///     at a glance how much floor area is covered.
///   • Every Collider caught by an OverlapSphere at edit-time is highlighted
///     so designers know EXACTLY what will be damaged — no guessing.
///   • Other ExplosiveObjects inside the radius are shown in a chain-reaction
///     color AND their own radii are drawn so you can see whether a cascade
///     will trigger (i.e. whether barrel B's radius catches barrel C, etc.).
///
/// Color legend drawn in the Inspector:
///   Red wire sphere    = this explosive's blast radius
///   Yellow highlights  = objects that WILL be damaged
///   Orange wire sphere = a nearby explosive that WILL chain-react
///   Pink wire sphere   = that chained explosive's own blast area
/// </summary>
[CustomEditor(typeof(Explosive))]
public class ExplosiveObjectGizmo : Editor
{
    // ----- Colors -----
    private static readonly Color RadiusColor = new Color(1f, 0.15f, 0.15f, 0.85f); // red
    private static readonly Color RadiusFill = new Color(1f, 0.15f, 0.15f, 0.08f);
    private static readonly Color HitObjectColor = new Color(1f, 0.95f, 0.1f, 0.9f);  // yellow
    private static readonly Color ChainSourceColor = new Color(1f, 0.5f, 0.0f, 1f);    // orange
    private static readonly Color ChainRadiusColor = new Color(1f, 0.5f, 0.85f, 0.55f); // pink

    // ------------------------------------------------------------------ //
    //  Inspector                                                          //
    // ------------------------------------------------------------------ //

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(6);

        // Color legend
        EditorGUILayout.LabelField("Scene View Color Legend", EditorStyles.boldLabel);
        DrawColorSwatch(RadiusColor, "This explosive's blast radius");
        DrawColorSwatch(HitObjectColor, "Objects that WILL be damaged");
        DrawColorSwatch(ChainSourceColor, "Nearby explosive — will chain-react");
        DrawColorSwatch(ChainRadiusColor, "That explosive's own blast area");

        EditorGUILayout.Space(4);
        EditorGUILayout.HelpBox(
            "The yellow highlights update in real-time as you move objects.\n" +
            "If two orange barrels overlap, both will detonate in sequence.",
            MessageType.Info);
    }

    // ------------------------------------------------------------------ //
    //  Scene-view drawing                                                 //
    // ------------------------------------------------------------------ //

    private void OnSceneGUI()
    {
        Explosive exp = (Explosive)target;
        Vector3 pos = exp.transform.position;
        float rad = exp.explosionRadius;

        // --- Primary blast radius ---
        Handles.color = RadiusFill;
        Handles.DrawSolidDisc(pos, Vector3.up, rad);   // ground shadow

        Handles.color = RadiusColor;
        Handles.DrawWireDisc(pos, Vector3.up, rad);
        Handles.DrawWireDisc(pos, Vector3.right, rad);
        Handles.DrawWireDisc(pos, Vector3.forward, rad);

        // --- Highlight objects in radius ---
        Collider[] hits = Physics.OverlapSphere(pos, rad, exp.damageMask);
        HashSet<Explosive> chainSet = new HashSet<Explosive>();

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == exp.gameObject) continue;

            Explosive otherExp = hit.GetComponentInParent<Explosive>();
            if (otherExp != null && otherExp != exp)
            {
                chainSet.Add(otherExp);
                continue; // drawn separately below
            }

            // Highlight non-explosive hit objects with a yellow bounding box
            Handles.color = HitObjectColor;
            Handles.DrawWireCube(hit.bounds.center, hit.bounds.size * 1.05f);

            Handles.Label(hit.bounds.center + Vector3.up * (hit.bounds.extents.y + 0.3f),
                $"  ⚡ {hit.name}", EditorStyles.miniLabel);
        }

        // --- Chain-reaction explosives ---
        foreach (Explosive chain in chainSet)
        {
            Vector3 cPos = chain.transform.position;

            // Orange sphere = this barrel WILL be triggered
            Handles.color = ChainSourceColor;
            Handles.DrawWireDisc(cPos, Vector3.up, chain.explosionRadius * 0.25f);
            Handles.DrawSolidDisc(cPos, Vector3.up, chain.explosionRadius * 0.25f);
            Handles.DrawLine(pos, cPos); // connecting line

            Handles.Label(cPos + Vector3.up * 0.6f,
                $"  🔥 CHAIN: {chain.name}", EditorStyles.whiteBoldLabel);

            // Pink sphere = the chained barrel's own blast area
            Handles.color = ChainRadiusColor;
            Handles.DrawWireDisc(cPos, Vector3.up, chain.explosionRadius);
            Handles.DrawWireDisc(cPos, Vector3.right, chain.explosionRadius);
            Handles.DrawWireDisc(cPos, Vector3.forward, chain.explosionRadius);
        }

        // Radius label
        Handles.color = Color.white;
        Handles.Label(pos + Vector3.right * rad + Vector3.up * 0.2f,
            $"  r = {rad:0.0}m", EditorStyles.miniLabel);
    }

    // ------------------------------------------------------------------ //
    //  Inspector helper                                                   //
    // ------------------------------------------------------------------ //

    private static void DrawColorSwatch(Color color, string label)
    {
        EditorGUILayout.BeginHorizontal();
        Rect swatchRect = GUILayoutUtility.GetRect(18, 14, GUILayout.Width(18));
        EditorGUI.DrawRect(swatchRect, color);
        EditorGUILayout.LabelField(label, EditorStyles.miniLabel);
        EditorGUILayout.EndHorizontal();
    }
}
#endif