using UnityEngine;


[ExecuteAlways]
public class Pathing : MonoBehaviour
{
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private Gradient _lineGradient;

    private void Update()
    {
        if(Application.isPlaying == true)
        {
            return;
        }

        

       Transform[] foundObjects = GetComponentsInChildren<Transform>();
        _patrolPoints = new Transform[foundObjects.Length - 1];
        int index = 0;
        for (int i = 0; i < foundObjects.Length; i++)
        {
            
            
            
                if (foundObjects[i] != transform)
                {
                    _patrolPoints[index] = foundObjects[i];
                    index++;
                }
            
        }

    }

    private void OnDrawGizmos()
    {
        if (_patrolPoints == null)
            return;

        for (int i = 0; i < _patrolPoints.Length; i++)
        {
            Gizmos.color = _lineGradient.Evaluate(i / (float)_patrolPoints.Length);
            Gizmos.DrawLine(_patrolPoints[i].position, _patrolPoints[(i + 1) % (_patrolPoints.Length)].position);
            /*Debug.Log("i: " + i + "," + ((i + 1) % (_patrolPoints.Length)));*/
        }
    }
}
