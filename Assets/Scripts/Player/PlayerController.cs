using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Camera Settings")]
    public float mouseSensitivity = 2f;
    public float verticalClamp = 90f;
    public GameObject cameraHolder;

    [Header("Movement Settings")]
    public float moveSpeed;

    private float xRot, yRot;

    [Header("Animator Parameters")]
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCamera();
        HandleMovement();
    }

    void HandleCamera()
    {
        float mouseY = Input.GetAxis("Mouse X");
        float mouseX = Input.GetAxis("Mouse Y");

        xRot -= mouseX;
        xRot = Mathf.Clamp(xRot, -verticalClamp, verticalClamp);

        yRot += mouseY;

        cameraHolder.transform.localRotation = Quaternion.Euler(xRot, 0, 0);
        transform.rotation = Quaternion.Euler(0, yRot, 0);

    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        animator.SetFloat("vertical", moveZ);
        animator.SetFloat("horizontal", moveX);

        Vector3 moveDir = transform.right * moveX + transform.forward * moveZ;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
