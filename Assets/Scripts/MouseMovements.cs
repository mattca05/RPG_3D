using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    private Vector2 mouseDelta;
    private PlayerControls controls;

    private bool cursorLocked = true; // Track if cursor is locked

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Look.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => mouseDelta = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        LockCursor(true);
    }

    private void Update()
    {
        // Optional: Press Escape to toggle cursor lock
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            cursorLocked = !cursorLocked;
            LockCursor(cursorLocked);
        }

        if (cursorLocked)
        {
            float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
            float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            yRotation += mouseX;

            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }

    // Helper to lock/unlock the cursor
    private void LockCursor(bool lockCursor)
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;   // Lock to center
            Cursor.visible = false;                     // Hide cursor
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;     // Free cursor
            Cursor.visible = true;                      // Show cursor
        }
    }
}