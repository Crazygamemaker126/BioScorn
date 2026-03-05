using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpForce = 5f;
    public LayerMask groundMask;
    public LayerMask slopeMask;
    public float currentmaxLinVel;

    public float hoveringDuration = 5;
    public float hoveringTimer;
    public float currentHoverTimeLeft; // will use to beef up hover mechanic as deployment ensues

    public float groundDrag = 5f;
    public float airDrag = 0f;
    public float slopeDrag = 10f;
    

    private Rigidbody rb;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isHovering;
    [SerializeField] private bool onSlope;

    private void Awake()
    {
        rb = GetComponent<Rigidbody> ();
        rb.freezeRotation = true;
        currentmaxLinVel = rb.maxLinearVelocity;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if(Input.GetButtonDown("Jump") && !isGrounded && transform.position.y > 0) 
        {
            Hover();
            hoveringTimer = hoveringDuration;
           
            isHovering = true;
           

        }

        if (isHovering)
        {
           
            if (hoveringTimer <= 0)
            {
                StopHover();
                isHovering = false;

            }
        }

        hoveringTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = (transform.forward * v + transform.right * h) * moveSpeed;
        Vector3 newVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
        rb.linearVelocity = newVelocity;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask);
        onSlope = Physics.Raycast(transform.position, Vector3.down, 1.1f, slopeMask);

        if (isGrounded) 
        {
            rb.linearDamping = groundDrag;
        }
        else 
        {
            rb.linearDamping = airDrag;
        }
    }

    public void Hover() 
    {
        float currentYPosition = transform.position.y;
        
        rb.useGravity = false;
        rb.maxLinearVelocity = currentYPosition;

    }

    public void StopHover() 
    {
        
        rb.useGravity = true;
        rb.maxLinearVelocity = currentmaxLinVel;
    }

    private void OnDrawGizmos()
    {
        
    }
}
