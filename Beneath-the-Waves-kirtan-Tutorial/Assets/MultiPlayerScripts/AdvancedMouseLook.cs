using UnityEngine;

public class AdvancedMouseLook : MonoBehaviour
{
    // Singleton instance
    public static AdvancedMouseLook instance;

    // Inspector-exposed settings
    [Header("Settings")]
    public Vector2 clampInDegrees = new Vector2(360, 180); // Clamp angles for rotation
    public bool lockCursor = true; // Whether to lock the cursor
    [Space]
    private Vector2 sensitivity = new Vector2(2, 2); // Mouse sensitivity
    [Space]
    public Vector2 smoothing = new Vector2(3, 3); // Mouse smoothing values

    [Header("First Person")]
    public GameObject characterBody; // Reference to the character body

    // Target direction for the camera and character body
    private Vector2 targetDirection;
    private Vector2 targetCharacterDirection;

    // Mouse movement values
    private Vector2 _mouseAbsolute;
    private Vector2 _smoothMouse;

    // Raw mouse input delta
    private Vector2 mouseDelta;

    // Scoped status
    [HideInInspector]
    public bool scoped;

    void Start()
    {
        instance = this; // Assign singleton instance

        // Set target direction to the camera's initial orientation
        targetDirection = transform.localRotation.eulerAngles;

        // Set target direction for the character body to its initial state
        if (characterBody)
            targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
        
        if (lockCursor)
            LockCursor(); // Lock cursor if specified
    }

    // Method to lock the cursor
    public void LockCursor()
    {
        // Make the cursor hidden and locked
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get the target orientations
        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        // Get raw mouse input
        mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // Scale input against the sensitivity and smoothing settings
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        // Interpolate mouse movement over time to apply smoothing delta
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

        // Find the absolute mouse movement value from point zero
        _mouseAbsolute += _smoothMouse;

        // Clamp and apply the local x value first
        if (clampInDegrees.x < 360)
            _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        // Then clamp and apply the global y value
        if (clampInDegrees.y < 360)
            _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        // Apply the rotation to the camera
        transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

        // If there's a character body that acts as a parent to the camera
        if (characterBody)
        {
            // Apply rotation to the character body
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
            characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
        }
        else
        {
            // Apply rotation to the camera itself
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
            transform.localRotation *= yRotation;
        }
    }
}
