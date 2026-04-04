using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Rigidbody))]

//Extract methods
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpForce = 5f;

    public Vector3 moveDir;

    public LayerMask platformMask;
    public LayerMask groundMask;
    public LayerMask slopeMask;
    public float currentmaxLinVel;

    public float maxHoverDuration = 5f;
    public float hoveringTimer;
    public float currentHoverTimeLeft; // will use to beef up hover mechanic as deployment ensues
    public float slopeJumpOffset = 3f;
    public float originalAngularDamping;

    public float groundDrag = 5f;
    public float airDrag = 0f;
    public float slopeDrag = 10f;

    public Slider hoverTimerSlider;
    

    private Rigidbody rb;
    [SerializeField] private bool isGrounded; //Never returns to True when contacting slope while hovering 
    [SerializeField] private bool isHovering; //Turns to false when contacting slope
    [SerializeField] private bool onSlope; //Never returns to True when contacting slope while hovering 
    [SerializeField] private bool jumpingFromSlope;
    [SerializeField] private bool canHover = true;
    

    
    private Coroutine startHoverCo;
    private Coroutine regenHoverTimerCo;

    private void Awake()
    {
        rb = GetComponent<Rigidbody> ();
        rb.freezeRotation = true;
        currentmaxLinVel = rb.maxLinearVelocity;
        hoverTimerSlider.value = hoveringTimer;
        /*hoverTimerSlider.maxValue = maxHoverDuration;*/ //Leave this out, it breaks UI by setting the maxValue of the slider to 5 when it should just be 1.
        originalAngularDamping = rb.angularDamping;
        hoveringTimer = maxHoverDuration;
        currentHoverTimeLeft = maxHoverDuration;
        canHover = true;

        UpdateHoverTimer();
        

    }

    private void Update()
    {
        UpdateHoverTimer();


        

        
       
    }

    public void OnJump(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            if (isGrounded)
            {
                if (!onSlope)
                {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
                else
                {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    jumpingFromSlope = true;
                }
            }
            else if (!isHovering && canHover && hoveringTimer > 0)
            {

                StartHover();
            }
            else 
            {

                StopHover();
            }
            
        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
           
        
    }

    public void HandleMovement()
    {
        Vector3 move = (transform.forward * moveDir.z + transform.right * moveDir.x) * moveSpeed;
        Vector3 newVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
        rb.linearVelocity = newVelocity;

    }

    private void FixedUpdate()
    {

        HandleMovement();
        HandleHoverTimer();
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask);
        onSlope = Physics.Raycast(transform.position, Vector3.down, 1.5f, slopeMask);
       

     

        if (onSlope) 
        {
            isGrounded = true;
        }

        if (isGrounded || onSlope) 
        {
            rb.linearDamping = groundDrag;
            StopHover();
        }
        else 
        {
            rb.linearDamping = airDrag;
        }
    }

    public void StartHover() 
    {
        isHovering = true;
        canHover = false;
        rb.useGravity = false;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

       


    }



    public void StopHover() 
    {
        canHover = true;
        isHovering = false;

        if (!jumpingFromSlope)
        {
            rb.useGravity = true;

            rb.maxLinearVelocity = currentmaxLinVel;
        }
        else 
        {
            rb.useGravity = true;

            rb.maxLinearVelocity = currentmaxLinVel;
            jumpingFromSlope = false;
            rb.angularDamping = originalAngularDamping;
        }
        
       
       
    }
    
    public void HandleHoverTimer() 
    {
        if (isHovering) 
        { 
        hoveringTimer -= Time.deltaTime;
        }
        else 
        {
            hoveringTimer += Time.deltaTime;
        }

        hoveringTimer = Mathf.Clamp(hoveringTimer, 0, maxHoverDuration);
        
        if(hoveringTimer == 0) 
        {
            StopHover();
        }
    }

    public IEnumerator StartHoverCo() 
    {
        Debug.Log("Hover timer start");
        while (hoveringTimer > 0)
        { 
            hoveringTimer -= Time.deltaTime;
            
            yield return null;
        }

        StopHover();
        //Debug.Log("HoverCooldownCo");
        //hoveringTimer += Time.deltaTime;
        //canHover = false;
        //isHovering = false;
        //Debug.Log("Hover ability recovering");

        // if(hoveringTimer >= maxHoverDuration)
        //canHover = true;
        // hoverCooldownCo = null;
        // Debug.Log("Hover ability returned");


    }

    public IEnumerator RegenHoverTimerCo() 
    {
        Debug.Log("Hover timer regenerating");

        while (hoveringTimer < maxHoverDuration) 
        {
            hoveringTimer += Time.deltaTime;
            yield return null;
        }

    }

    public void UpdateHoverTimer() 
    {
        hoverTimerSlider.value = hoveringTimer/maxHoverDuration;

    }

   

    private void OnDrawGizmos()
    {
        
    }
}
