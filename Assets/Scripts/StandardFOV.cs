

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class StandardFOV  : MonoBehaviour
{
    [SerializeField, Range(0, 360)] private float _fovAngle = 60f;
    [SerializeField, Range(0, 100)] private float _viewDistance = 20f;
    [SerializeField] private Color _color = Color.red;
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = _color;

        Vector3 direction = Quaternion.AngleAxis((float)(-_fovAngle * 0.5), transform.up) * transform.forward;

        Handles.DrawSolidArc(transform.position, transform.up, direction, _fovAngle, _viewDistance);
    }
#endif
}
