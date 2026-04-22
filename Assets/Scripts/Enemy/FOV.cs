using UnityEngine;

public class FOV : MonoBehaviour
{
    [Header("FOV Settings")]
    public float fovAngle = 90f;                  // Field of view angle in degrees
    public GameObject target;                     // Currently detected target (Player)

    private SphereCollider sensorTrigger;         // Trigger collider to detect objects in range

    [Header("Raycast Settings")]
    public LayerMask obstacleMask;                // Layers that block line of sight

    void Start()
    {
        // Get or add a SphereCollider to act as a trigger for FOV detection
        sensorTrigger = GetComponent<SphereCollider>();
        if (sensorTrigger == null)
        {
            sensorTrigger = gameObject.AddComponent<SphereCollider>();
        }

        sensorTrigger.isTrigger = true;
    }

    /// <summary>
    /// Called every frame while another collider stays in the trigger.
    /// Sets target if visible and within FOV.
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        // Only consider Player objects
        if (other.CompareTag("Player") && IsColliderVisible(other))
        {
            target = other.gameObject;
            // Do not clear target here; OnTriggerExit will handle that
        }
    }

    /// <summary>
    /// Clears the target if it leaves the trigger area.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
        }
    }

    /// <summary>
    /// Checks if a collider is within FOV and line of sight.
    /// </summary>
    private bool IsColliderVisible(Collider other)
    {
        // Direction from FOV origin to the target
        Vector3 direction = (other.transform.position - transform.position).normalized;

        // Check angle between forward direction and target
        float angle = Vector3.Angle(direction, transform.forward);
        if (angle > fovAngle * 0.5f)
            return false; // Outside FOV angle

        // Offset origin slightly upward to avoid hitting own collider
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        RaycastHit hitInfo;

        // Raycast to see if target is visible (not blocked by obstacles)
        if (Physics.Raycast(origin, direction, out hitInfo, sensorTrigger.radius, ~obstacleMask))
        {
            if (hitInfo.transform == other.transform)
                return true;
        }

        return false;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Draws the FOV arc in the editor for debugging.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (sensorTrigger == null)
        {
            sensorTrigger = GetComponent<SphereCollider>();
            if (sensorTrigger == null) return; // Exit if no collider found
        }

        // Semi-transparent red color
        Color c = new Color(1, 0, 0, 0.15f);
        UnityEditor.Handles.color = c;

        // Project forward vector to horizontal plane for accurate arc
        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        // Start direction for the arc
        Vector3 rotatedForward = Quaternion.Euler(0, -fovAngle / 2, 0f) * forward;

        UnityEditor.Handles.DrawSolidArc(
            transform.position,
            Vector3.up,
            rotatedForward,
            fovAngle,
            sensorTrigger.radius
        );
    }
#endif
}