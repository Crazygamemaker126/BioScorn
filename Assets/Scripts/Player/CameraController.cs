using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 2f;
    private float xRotation = 0f;
    public float mouseX, mouseY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCameraRot();
    }

    private void HandleCameraRot()
    {
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.parent.Rotate(Vector3.up * mouseX);
    }

    public void OnLook(InputAction.CallbackContext context) 
    {
        mouseX = context.ReadValue<Vector2>().x;
        mouseY = context.ReadValue<Vector2>().y;
    }
}
