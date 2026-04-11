using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
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
    public float currentHoverTimeLeft;
    public float slopeJumpOffset = 3f;
    public float originalAngularDamping;

    public float groundDrag = 5f;
    public float airDrag = 0f;
    public float slopeDrag = 10f;

    // Optional — assign in Inspector only if you want PlayerController
    // to also drive a slider directly. Leaving it empty is fine.
    public Slider hoverTimerSlider;

    // Fired every frame the hover timer changes — HUDController subscribes to this
    public event Action<float> OnHoverTimerChanged;

    private Rigidbody rb;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isHovering;
    [SerializeField] private bool onSlope;
    [SerializeField] private bool jumpingFromSlope;
    [SerializeField] private bool canHover = true;

    private Coroutine startHoverCo;
    private Coroutine regenHoverTimerCo;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        currentmaxLinVel = rb.maxLinearVelocity;
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
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                if (onSlope) jumpingFromSlope = true;
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

        if (onSlope) isGrounded = true;

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
        rb.useGravity = true;
        rb.maxLinearVelocity = currentmaxLinVel;

        if (jumpingFromSlope)
        {
            jumpingFromSlope = false;
            rb.angularDamping = originalAngularDamping;
        }
    }

    public void HandleHoverTimer()
    {
        hoveringTimer += isHovering ? -Time.deltaTime : Time.deltaTime;
        hoveringTimer = Mathf.Clamp(hoveringTimer, 0, maxHoverDuration);

        if (hoveringTimer == 0)
            StopHover();

        // Fire every frame so HUDController can update the slider smoothly
        OnHoverTimerChanged?.Invoke(hoveringTimer / maxHoverDuration);
    }

    public void UpdateHoverTimer()
    {
        if (hoverTimerSlider != null)
            hoverTimerSlider.value = hoveringTimer / maxHoverDuration;
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

    private void OnDrawGizmos() { }
}
