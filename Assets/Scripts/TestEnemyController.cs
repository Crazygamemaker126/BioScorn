using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(Rigidbody))]
public class TestEnemyController : MonoBehaviour
{
    
    public Transform waypoint;
    public BoxCollider sensor;
    public float moveSpeed;
    private Rigidbody rb;
    public float patrolTimer = 20f;
    
    private void Awake()
    {
        sensor = GetComponentInChildren<BoxCollider>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        patrolTimer -= Time.deltaTime;
        

        if(patrolTimer < 0) 
        {
            transform.rotation = Quaternion.identity * Quaternion.Euler(0, 0, 90);
        }
    }

    public void EnemyPatrol() 
    {
    
    }
}
