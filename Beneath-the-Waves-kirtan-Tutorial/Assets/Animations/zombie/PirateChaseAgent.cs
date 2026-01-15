using UnityEngine;
using UnityEngine.AI;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

[RequireComponent(typeof(NavMeshAgent))]
public class PirateChaseAgent : Agent
{
    [Header("References")]
    public Transform target;              // Assign Player transform here
    public Animator anim;                 // Optional (assign or auto-find)

    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float turnSpeed = 180f;        // degrees/sec
    public float forwardStep = 1.0f;      // how far ahead agent tries to move (NavMesh destination)

    [Header("Episode")]
    public float episodeTimeLimit = 15f;
    public float catchDistance = 1.5f;

    [Header("Rewards")]
    public float timePenalty = -0.001f;       // small penalty each step
    public float progressRewardScale = 0.05f; // reward for getting closer
    public float facingRewardScale = 0.005f;  // reward for facing target
    public float catchReward = 2.0f;
    public float failPenalty = -0.5f;

    private NavMeshAgent agent;
    private float timer;
    private float prevDistance;

    public override void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.angularSpeed = turnSpeed;
        agent.acceleration = 50f;
        agent.stoppingDistance = 0.0f;
        agent.autoBraking = false;

        if (anim == null) anim = GetComponent<Animator>();
    }

    public override void OnEpisodeBegin()
    {
        timer = 0f;

        // If training in a sandbox scene: randomize positions here.
        // If using in your real game: DO NOT teleport; just reset reward timers.
        prevDistance = Vector3.Distance(transform.position, target.position);

        // Ensure agent is alive on navmesh
        if (agent != null)
        {
            agent.isStopped = false;
            agent.ResetPath();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 4 observations total (match your trained model!)

        Vector3 toTarget = target.position - transform.position;
        float distance = toTarget.magnitude;

        Vector3 dir = distance > 0.0001f ? (toTarget / distance) : Vector3.zero;

        // (1) dir.x
        sensor.AddObservation(dir.x);

        // (2) dir.z
        sensor.AddObservation(dir.z);

        // (3) distance (you can normalize if you want, but keep same as training)
        sensor.AddObservation(distance);

        // (4) facing alignment [-1..1]
        float facing = Vector3.Dot(transform.forward, dir);
        sensor.AddObservation(facing);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (target == null)
        {
            AddReward(-1f);
            EndEpisode();
            return;
        }

        int a = actions.DiscreteActions[0];

        // ---- APPLY ACTIONS ----
        // 0 = idle
        // 1 = move forward (via NavMesh destination)
        // 2 = turn left
        // 3 = turn right

        bool moved = false;

        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.speed = moveSpeed;
            agent.angularSpeed = turnSpeed;

            if (a == 1)
            {
                Vector3 next = transform.position + transform.forward * forwardStep;

                // Only set destination if it’s valid
                if (NavMesh.SamplePosition(next, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                    moved = true;
                }
            }
            else if (a == 2)
            {
                transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
            }
            else if (a == 3)
            {
                transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
            }
        }

        // ---- ANIMATION (optional) ----
        if (anim != null)
        {
            float speed01 = 0f;

            if (agent != null && agent.enabled)
            {
                speed01 = agent.velocity.magnitude / Mathf.Max(0.001f, moveSpeed);
            }
            else
            {
                speed01 = moved ? 1f : 0f;
            }

            // These parameters MUST exist in your Animator Controller
            // If you don't have them, either add them or remove these lines
            if (HasParam(anim, "Speed")) anim.SetFloat("Speed", speed01);
            if (HasParam(anim, "IsChasing")) anim.SetBool("IsChasing", speed01 > 0.1f);
        }

        // ---- REWARDS ----
        float distance = Vector3.Distance(transform.position, target.position);

        // Reward for progress (getting closer than last step)
        float progress = prevDistance - distance;
        AddReward(progress * progressRewardScale);

        // Facing reward
        Vector3 dir = (target.position - transform.position).normalized;
        float facing = Vector3.Dot(transform.forward, dir);
        AddReward(facing * facingRewardScale);

        // Small time penalty to encourage fast catch
        AddReward(timePenalty);

        prevDistance = distance;

        // ---- SUCCESS ----
        if (distance <= catchDistance)
        {
            AddReward(catchReward);

            if (anim != null && HasParam(anim, "Attack"))
                anim.SetTrigger("Attack");

            EndEpisode();
            return;
        }

        // ---- TIMEOUT ----
        timer += Time.deltaTime;
        if (timer >= episodeTimeLimit)
        {
            AddReward(failPenalty);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var d = actionsOut.DiscreteActions;
        d[0] = 0;

        if (Input.GetKey(KeyCode.W)) d[0] = 1;
        if (Input.GetKey(KeyCode.A)) d[0] = 2;
        if (Input.GetKey(KeyCode.D)) d[0] = 3;
    }

    private bool HasParam(Animator animator, string paramName)
    {
        foreach (var p in animator.parameters)
            if (p.name == paramName) return true;
        return false;
    }
}