using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
  // Header for inspector properties
  [Header("Settings")]
  public float swayClamp = 0.09f; // Maximum amount of sway in each direction
  [Space]
  public float smoothing = 3f;  // Smoothness of the sway animation

  private Vector3 origin;  // Stores the initial local position of the object

  // Start is called before the first frame update
  void Start()
  {
    origin = transform.localPosition;  // Capture the initial local position
  }

  // Update is called once per frame
  void Update()
  {
    // Get raw mouse input (unprocessed)
    Vector2 input = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

    // Clamp mouse input to prevent excessive sway
    input.x = Mathf.Clamp(input.x, -swayClamp, swayClamp);
    input.y = Mathf.Clamp(input.y, -swayClamp, swayClamp);

    // Invert and negate mouse input for natural sway effect
    Vector3 target = new Vector3(-input.x, -input.y, 0);

    // Smoothly interpolate between current position and target position
    transform.localPosition = Vector3.Lerp(transform.localPosition, target + origin, Time.deltaTime * smoothing);
  }
}
