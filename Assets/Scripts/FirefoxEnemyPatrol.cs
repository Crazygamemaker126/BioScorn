using UnityEngine;
using System.Collections;

public class FirefoxEnemyPatrol : MonoBehaviour
{
    // Array to hold the waypoint Transforms. Assign these in the Unity Inspector.
    public Transform[] patrolPoints;
    public float moveSpeed = 3f; // Speed of the enemy movement
    public float rotationSpeed = 2f; // Speed of rotation
    public float reachDistance = 1f; // Distance to consider a waypoint "reached"
    public float waitTime = 1f; // Time to wait at each waypoint

    private int currentPointIndex;
    private Rigidbody rb;

    private Transform playerTransform;
    private bool playerInRange = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on the enemy GameObject!");
        }

        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (patrolPoints.Length == 0)
        {
            Debug.LogError("No patrol points assigned!");
            enabled = false; // Disable the script if no points are set
            return;
        }

        currentPointIndex = 0;
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            if (patrolPoints.Length == 0) yield break;

            Vector3 targetPosition = patrolPoints[currentPointIndex].position;
            Vector3 directionToTarget = targetPosition - transform.position;
            directionToTarget.y = 0; // Keep movement on the horizontal plane

            // Move the enemy towards the target position
            if (directionToTarget.magnitude > reachDistance)
            {
                // Calculate movement velocity
                Vector3 moveVelocity = directionToTarget.normalized * moveSpeed;
                rb.linearVelocity = moveVelocity; // Directly setting velocity for simple movement

                // Rotate towards the target
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else // Waypoint reached, move to next
            {
                rb.linearVelocity = Vector3.zero; // Stop movement

                // Wait at the waypoint
                yield return new WaitForSeconds(waitTime);

                // Move to the next point in the array, looping back to the start if necessary
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            }

            yield return null; // Wait for the next frame
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered detection range. Chasing!");
        }
    }

    // Called when the other collider leaves the trigger collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // Optional: Stop movement or switch to a wander state when player leaves
            rb.linearVelocity = Vector3.zero;
            Debug.Log("Player left detection range. Stopping chase.");
        }
    }
    void FixedUpdate()
    {
        if (playerInRange && playerTransform != null)
        {
            // Calculate the direction towards the player
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            // Use AddForce to move the enemy towards the player using physics
            // You can also use Rigidbody.MovePosition for more direct control
            rb.AddForce(direction * moveSpeed);

            // Alternative with MovePosition (requires Rigidbody to be kinematic or handled carefully with collisions)
            // Vector3 newPosition = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
            // enemyRigidbody.MovePosition(newPosition);
        }
    }

    // Optional: Draw lines in the editor to visualize the patrol path
    void OnDrawGizmos()
    {
        if (patrolPoints != null && patrolPoints.Length > 1)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null && patrolPoints[(i + 1) % patrolPoints.Length] != null)
                {
                    Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[(i + 1) % patrolPoints.Length].position);
                }
            }
        }
    }
}
