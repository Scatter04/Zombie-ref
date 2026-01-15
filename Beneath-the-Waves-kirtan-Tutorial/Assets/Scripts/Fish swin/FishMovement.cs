using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float turnSpeed = 1.0f;
    public float swimRadius = 10.0f; // Radius of the area within which the fish will swim
    public float minSwimDistance = 1.0f; // Minimum distance to the target position before selecting a new target

    // Depth constraints
    public float minDepth = -25.0f; // Minimum depth the fish can swim to (y coordinate)
    public float maxDepth = -20.0f; // Maximum depth the fish can swim to (y coordinate)
    public float heightAboveGround = 0.5f; // Minimum distance above the ground

    private Vector3 targetPosition;

    void Start()
    {
        SetNewRandomTarget();
    }

    void Update()
    {
        MoveFish();
    }

    void SetNewRandomTarget()
    {
        // Generate a random point within a sphere
        Vector3 randomDirection = Random.insideUnitSphere * swimRadius;
        randomDirection += transform.position;

        // Constrain the random point within the swim area bounds
        targetPosition = new Vector3(randomDirection.x, Mathf.Clamp(randomDirection.y, minDepth, maxDepth), randomDirection.z);

        // Adjust target position to stay above the ground
        AdjustPositionAboveGround(ref targetPosition);
    }

    void MoveFish()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget <= minSwimDistance)
        {
            SetNewRandomTarget();
        }
        else
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            // Adjust current position to stay above the ground
            Vector3 adjustedPosition = transform.position;
            AdjustPositionAboveGround(ref adjustedPosition);
            transform.position = adjustedPosition;
        }
    }

    void AdjustPositionAboveGround(ref Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.down, out hit))
        {
            // Adjust position to be above the ground
            position.y = Mathf.Max(hit.point.y + heightAboveGround, position.y);
        }
    }
}
