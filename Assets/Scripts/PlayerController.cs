using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using UnityEngine.UI;
using System.IO;

[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpForce = 5f;
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

    private Coroutine hoverCooldownCo;

    private void Awake()
    {
        rb = GetComponent<Rigidbody> ();
        rb.freezeRotation = true;
        currentmaxLinVel = rb.maxLinearVelocity;
        hoverTimerSlider.maxValue = maxHoverDuration;
        hoverTimerSlider.value = hoveringTimer;
        originalAngularDamping = rb.angularDamping;
        hoveringTimer = maxHoverDuration;
        currentHoverTimeLeft = maxHoverDuration;
        canHover = true;

        UpdateHoverTimer();
        

    }

    private void Update()
    {
        UpdateHoverTimer();

        if (Input.GetButtonDown("Jump") && isGrounded)
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

        if(Input.GetButtonDown("Jump") && !isGrounded && transform.position.y > 0 && hoverCooldownCo == null) 
        {
         

            if (!isHovering && canHover)
            { 
                StartHover();
                if (!jumpingFromSlope)
                {
                    float currentYPosition = transform.position.y;

                    rb.useGravity = false;
                    rb.maxLinearVelocity = currentYPosition;
                }
                else
                {
                    float offsetYPosition = transform.position.y + slopeJumpOffset;

                    float offsetAngDrag = (rb.angularDamping * airDrag) - (jumpForce - transform.position.y);
                    rb.AddForce(Vector3.up, ForceMode.Impulse);
                    rb.useGravity = false;
                    rb.maxLinearVelocity = offsetYPosition;
                    rb.angularDamping = offsetAngDrag;
                    jumpingFromSlope = false;
                }
                
                

               

            }
            else if(Input.GetButtonDown("Jump") && isHovering)
            {
                StopHover();
                isHovering = false;
            }

        }

        if (isGrounded) 
        {
            isHovering = false;
        }

        if (isHovering)
        {
            hoveringTimer -= Time.deltaTime;

            if (hoveringTimer <= 0)
            {
                StopHover();
                isHovering = false;

            }
        }
        else 
        {
            hoveringTimer += Time.deltaTime;
            hoveringTimer = Mathf.Clamp(hoveringTimer, 0, maxHoverDuration);
            
        }

        
       
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = (transform.forward * v + transform.right * h) * moveSpeed;
        Vector3 newVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
        rb.linearVelocity = newVelocity;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask);
        onSlope = Physics.Raycast(transform.position, Vector3.down, 1.5f, slopeMask);

        if (onSlope) 
        {
            isGrounded = true;
        }

        if (isGrounded || onSlope) 
        {
            rb.linearDamping = groundDrag;
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

        if (currentHoverTimeLeft < 0)
        {
            StartCoroutine(HoverCooldownCo());
        }


    }

    public void StopHover() 
    {
        canHover = false;
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
        
        if (hoverCooldownCo != null)
        {
            StopCoroutine(hoverCooldownCo);

        }
        else
        {
            hoverCooldownCo = StartCoroutine(HoverCooldownCo());
        }
    }

    public void UpdateHoverTimer() 
    {
        hoverTimerSlider.value = hoveringTimer;

    }

   

    public IEnumerator HoverCooldownCo() 
    {
        hoveringTimer += Time.deltaTime;
        canHover = false;
        isHovering = false;
        Debug.Log("Cool those jets");

        
        yield return new WaitForSeconds(maxHoverDuration - hoveringTimer);

        if(hoveringTimer >= maxHoverDuration)
       canHover = true;
        hoverCooldownCo = null;


    }

    private void OnDrawGizmos()
    {
        
    }
}
