using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Public variables accessible from the Unity Inspector
    public float walkSpeed = 8f; // Walking speed
    public float sprintSpeed = 14f; // Sprinting speed
    public float maxVelocityChange = 10f; // Maximum change in velocity per frame
    [Space]
    public float airControl = 0.5f; // Control factor while in the air

    [Space]
    public float jumpHeight = 30f; // Jump height

    private Vector2 input; // Input vector for movement
    private Rigidbody rb; // Reference to the Rigidbody component
    private bool sprinting; // Flag to check if the player is sprinting
    private bool jumping; // Flag to check if the player is jumping
    private bool grounded = false; // Flag to check if the player is grounded

    void Start()
    {
        // Get the Rigidbody component attached to this GameObject
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from the horizontal and vertical axes
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize(); // Normalize the input vector to prevent faster diagonal movement

        // Check if the sprint and jump buttons are pressed
        sprinting = Input.GetButton("Sprint");
        jumping = Input.GetButton("Jump");
    }

    private void OnTriggerStay(Collider other)
    {
        // Set grounded to true when the player is in contact with a collider
        grounded = true;
    }

    // This will be called by Unity every time there's a new physics update
    void FixedUpdate()
    {
        if (grounded)
        {
            if (jumping)
            {
                // Apply jump force
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpHeight, rb.linearVelocity.z);
            }
            else if (input.magnitude > 0.5f)
            {
                // Apply movement force
                rb.AddForce(CalculateMovement(sprinting ? sprintSpeed : walkSpeed), ForceMode.VelocityChange);
            }
            else
            {
                // Apply slight deceleration
                var velocity1 = rb.linearVelocity;
                velocity1 = new Vector3(velocity1.x * 0.98f, velocity1.y, velocity1.z * 0.98f);
                rb.linearVelocity = velocity1;
            }
        }
        else
        {
            if (input.magnitude > 0.5f)
            {
                // Apply movement force with reduced control in the air
                rb.AddForce(CalculateMovement(sprinting ? sprintSpeed * airControl : walkSpeed * airControl), ForceMode.VelocityChange);
            }
            else
            {
                // Apply slight deceleration in the air
                var velocity1 = rb.linearVelocity;
                velocity1 = new Vector3(velocity1.x * 0.98f, velocity1.y, velocity1.z * 0.98f);
                rb.linearVelocity = velocity1;
            }
        }
        // Reset grounded flag for the next frame
        grounded = false;
    }

    // Method to calculate movement vector
    Vector3 CalculateMovement(float _speed)
    {
        // Create target velocity vector based on input
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= _speed;

        // Get current velocity
        Vector3 velocity = rb.linearVelocity;

        // If there's significant input, calculate velocity change
        if (input.magnitude > 0.5f)
        {
            Vector3 velocityChange = targetVelocity - velocity;
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0; // No vertical change in velocity

            return velocityChange;
        }
        // If no significant input, return an empty vector
        else
        {
            return Vector3.zero;
        }
    }
}
